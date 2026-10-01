"use strict";

/*
 * Hamid Wiki
 *
 * Markdown is the source of truth.
 *
 * The browser:
 *
 *   1. Loads wiki/README.md
 *   2. Follows every relative Markdown link under wiki/
 *   3. Loads each Markdown page once
 *   4. Builds an in-memory search index
 *   5. Renders the selected page
 *
 * Later, this can easily be replaced by a generated
 * search-index.json without changing the UI.
 */

const CONFIG = {
  wikiRoot: "/wiki/",
  indexFile: "/wiki/README.md",
};

const state = {
  lessons: [],
  currentLessonIndex: -1,
  searchResults: [],
  selectedSearchResult: 0,
};

/* -------------------------------------------------- */
/* DOM */
/* -------------------------------------------------- */

const content = document.querySelector("#content");
const lessonNav = document.querySelector("#lesson-nav");
const breadcrumbs = document.querySelector("#breadcrumbs");
const pageNavigation = document.querySelector("#page-navigation");

const searchDialog = document.querySelector("#search-dialog");
const searchButton = document.querySelector("#search-button");
const searchClose = document.querySelector("#search-close");
const searchInput = document.querySelector("#search-input");
const searchStatus = document.querySelector("#search-status");
const searchResults = document.querySelector("#search-results");

const themeButton = document.querySelector("#theme-button");

/* -------------------------------------------------- */
/* Markdown */
/* -------------------------------------------------- */

marked.setOptions({
  gfm: true,
  breaks: false,
});

/* -------------------------------------------------- */
/* Utilities */
/* -------------------------------------------------- */

function escapeHtml(value) {
  return String(value)
    .replaceAll("&", "&amp;")
    .replaceAll("<", "&lt;")
    .replaceAll(">", "&gt;")
    .replaceAll('"', "&quot;")
    .replaceAll("'", "&#039;");
}

function slugify(value) {
  return value
    .toLowerCase()
    .trim()
    .replace(/[^\w\s-]/g, "")
    .replace(/\s+/g, "-");
}

function getLessonNumber(path) {
  const match = path.split("/").pop().match(/^(\d+)_/);
  return match ? match[1] : "";
}

function getTitleFromPath(path) {
  const parts = path.replace(/\.md$/, "").split("/");
  let name = parts.pop();

  if (name === "README") {
    name = parts.pop() || "Wiki";
  }

  return name
    .replace(/^\d+_/, "")
    .replace(/[-_]/g, " ")
    .replace(/\b\w/g, (letter) => letter.toUpperCase());
}

function getTitleFromMarkdown(markdown, path) {
  const match = markdown.match(/^#\s+(.+?)\s*$/m);

  if (!match) {
    return getTitleFromPath(path);
  }

  return match[1].replace(/^\d+\s*-\s*/, "").trim();
}

function getGroup(path) {
  const parts = path.split("/");

  parts.pop();

  return parts.join("/");
}

function getRoute() {
  const params = new URLSearchParams(window.location.search);
  return (
    params.get("page") ||
    decodeURIComponent(window.location.hash.slice(1)) ||
    null
  );
}

function setRoute(path) {
  const url = new URL(window.location.href);

  url.searchParams.set("page", path);

  try {
    window.history.pushState({}, "", url);
  } catch {
    /* Some browsers refuse pushState on file:// URLs. */
    window.location.hash = path;
  }
}

/*
 * Turn a link found in a page into a wiki path
 * (relative to wiki/), or null if it is not a local
 * Markdown page under wiki/.
 */
function resolveWikiPath(href, fromPath) {
  if (/^[a-z][a-z0-9+.-]*:/i.test(href) || href.startsWith("#")) {
    return null;
  }

  const clean = href.split("#")[0].split("?")[0];

  if (!clean.endsWith(".md")) {
    return null;
  }

  const base = new URL(
    `${CONFIG.wikiRoot}${fromPath}`,
    "http://wiki.local"
  );
  const resolved = new URL(clean, base).pathname;

  if (!resolved.startsWith(CONFIG.wikiRoot)) {
    return null;
  }

  const path = decodeURIComponent(
    resolved.slice(CONFIG.wikiRoot.length)
  );

  if (path.split("/").includes("tmp")) {
    return null;
  }

  return path;
}

/* -------------------------------------------------- */
/* Fetch */
/* -------------------------------------------------- */

async function fetchText(url) {
  /*
   * Opened from file://, fetch is blocked. scripts/build-web.sh
   * writes wiki-data.js with every page, keyed by path under wiki/.
   */
  if (window.WIKI_DATA) {
    const key = url.slice(CONFIG.wikiRoot.length);

    if (key in window.WIKI_DATA) {
      return window.WIKI_DATA[key];
    }

    throw new Error(`Could not load ${url} (not in wiki-data.js)`);
  }

  const response = await fetch(url);

  if (!response.ok) {
    throw new Error(
      `Could not load ${url} (${response.status})`
    );
  }

  return response.text();
}

/* -------------------------------------------------- */
/* Markdown links */
/* -------------------------------------------------- */

function findLinkedPaths(markdown, fromPath) {
  const paths = new Set();
  const pattern = /\]\(([^)\s]+)(?:\s+"[^"]*")?\)/g;

  let match;

  while ((match = pattern.exec(markdown)) !== null) {
    const path = resolveWikiPath(match[1], fromPath);

    if (path) {
      paths.add(path);
    }
  }

  return paths;
}

function fixMarkdownLinks(html, fromPath) {
  const container = document.createElement("div");
  container.innerHTML = html;

  for (const link of container.querySelectorAll("a")) {
    const href = link.getAttribute("href");

    if (!href) {
      continue;
    }

    if (href.startsWith("http://") || href.startsWith("https://")) {
      link.target = "_blank";
      link.rel = "noopener noreferrer";
      continue;
    }

    const path = resolveWikiPath(href, fromPath);

    if (path) {
      link.href = `?page=${encodeURIComponent(path)}`;
      link.dataset.page = path;
    }
  }

  return container.innerHTML;
}

/* -------------------------------------------------- */
/* Load lessons */
/* -------------------------------------------------- */

async function loadPage(path) {
  const markdown = await fetchText(`${CONFIG.wikiRoot}${path}`);
  const html = fixMarkdownLinks(marked.parse(markdown), path);

  const temporary = document.createElement("div");
  temporary.innerHTML = html;

  return {
    path,
    number: getLessonNumber(path),
    title: getTitleFromMarkdown(markdown, path),
    group: getGroup(path),
    markdown,
    html,
    plainText: temporary.textContent.replace(/\s+/g, " ").trim(),
  };
}

async function loadLessons() {
  /*
   * Breadth-first crawl from wiki/README.md. Each wave of
   * newly found pages is fetched in parallel.
   */
  const seen = new Set(["README.md"]);
  let wave = ["README.md"];
  const pages = [];

  while (wave.length > 0) {
    const loaded = await Promise.allSettled(wave.map(loadPage));
    const next = [];

    loaded.forEach((result, i) => {
      if (result.status === "rejected") {
        console.warn(result.reason);
        return;
      }

      const page = result.value;

      pages.push(page);

      for (const path of findLinkedPaths(page.markdown, page.path)) {
        if (!seen.has(path)) {
          seen.add(path);
          next.push(path);
        }
      }
    });

    wave = next;
  }

  if (pages.length === 0) {
    throw new Error("No pages were found under wiki/.");
  }

  /*
   * Sort by path so folders stay together. The root
   * README stays first.
   */
  state.lessons = pages.sort((a, b) => {
    if (a.path === "README.md") return -1;
    if (b.path === "README.md") return 1;
    return a.path.localeCompare(b.path, undefined, { numeric: true });
  });

  renderNavigation();
  buildSearchIndex();

  const requested = getRoute();

  const index = state.lessons.findIndex(
    (lesson) => lesson.path === requested
  );

  showLesson(index !== -1 ? index : 0, false);
}

/* -------------------------------------------------- */
/* Navigation */
/* -------------------------------------------------- */

function renderNavigation() {
  let group = null;
  let html = "";

  state.lessons.forEach((lesson, index) => {
    if (lesson.group !== group) {
      group = lesson.group;

      html += `<div class="nav-group">${escapeHtml(
        group || "Wiki"
      )}</div>`;
    }

    html += `
      <a
        class="lesson-link"
        href="?page=${encodeURIComponent(lesson.path)}"
        data-index="${index}"
      >
        ${
          lesson.number
            ? `<span class="lesson-number">${lesson.number}</span>`
            : ""
        }
        <span>${escapeHtml(lesson.title)}</span>
      </a>
    `;
  });

  lessonNav.innerHTML = html;
}

function updateActiveNavigation() {
  for (const link of lessonNav.querySelectorAll(".lesson-link")) {
    const index = Number(link.dataset.index);

    link.classList.toggle(
      "active",
      index === state.currentLessonIndex
    );
  }
}

/* -------------------------------------------------- */
/* Show lesson */
/* -------------------------------------------------- */

function showLesson(index, updateUrl = true) {
  const lesson = state.lessons[index];

  if (!lesson) {
    return;
  }

  state.currentLessonIndex = index;

  if (updateUrl) {
    setRoute(lesson.path);
  }

  content.innerHTML = lesson.html;

  /*
   * Add IDs to headings for future table-of-contents support.
   */
  for (const heading of content.querySelectorAll(
    "h2, h3, h4"
  )) {
    if (!heading.id) {
      heading.id = slugify(heading.textContent);
    }
  }

  /*
   * Markdown links to other lessons.
   */
  for (const link of content.querySelectorAll(
    "a[data-page]"
  )) {
    link.addEventListener("click", (event) => {
      event.preventDefault();

      const path = link.dataset.page;

      const lessonIndex = state.lessons.findIndex(
        (item) => item.path === path
      );

      if (lessonIndex !== -1) {
        showLesson(lessonIndex);
        window.scrollTo({
          top: 0,
          behavior: "smooth",
        });
      }
    });
  }

  const crumbs = lesson.group
    ? lesson.group.split("/").map(
        (part) => `
          <span>${escapeHtml(getTitleFromPath(`${part}/README.md`))}</span>
          <span>/</span>`
      ).join("")
    : "";

  breadcrumbs.innerHTML = `
    <a href="./">Home</a>
    <span>/</span>
    ${crumbs}
    <span>${escapeHtml(lesson.title)}</span>
  `;

  renderPageNavigation();
  updateActiveNavigation();

  document.title = `${lesson.title} · Hamid Wiki`;
}

function renderPageNavigation() {
  const index = state.currentLessonIndex;

  const previous = state.lessons[index - 1];
  const next = state.lessons[index + 1];

  const previousHtml = previous
    ? `
      <a
        class="page-nav-link"
        href="?lesson=${encodeURIComponent(previous.path)}"
        data-page-lesson="${previous.path}"
      >
        <span class="page-nav-label">Previous</span>
        <span class="page-nav-title">
          ← ${escapeHtml(previous.title)}
        </span>
      </a>
    `
    : "<div></div>";

  const nextHtml = next
    ? `
      <a
        class="page-nav-link next"
        href="?lesson=${encodeURIComponent(next.path)}"
        data-page-lesson="${next.path}"
      >
        <span class="page-nav-label">Next</span>
        <span class="page-nav-title">
          ${escapeHtml(next.title)} →
        </span>
      </a>
    `
    : "<div></div>";

  pageNavigation.innerHTML =
    previousHtml + nextHtml;

  for (const link of pageNavigation.querySelectorAll(
    "[data-page-lesson]"
  )) {
    link.addEventListener("click", (event) => {
      event.preventDefault();

      const path = link.dataset.pageLesson;

      const lessonIndex = state.lessons.findIndex(
        (item) => item.path === path
      );

      if (lessonIndex !== -1) {
        showLesson(lessonIndex);

        window.scrollTo({
          top: 0,
          behavior: "smooth",
        });
      }
    });
  }
}

/* -------------------------------------------------- */
/* Search index */
/* -------------------------------------------------- */

function buildSearchIndex() {
  /*
   * For a few hundred pages, keeping this entirely in memory
   * is simple and fast.
   */
}

/*
 * Normalize text for searching.
 */
function normalize(text) {
  return text
    .toLowerCase()
    .replace(/[^\p{L}\p{N}\s]/gu, " ")
    .replace(/\s+/g, " ")
    .trim();
}

function searchLessons(query) {
  const normalizedQuery = normalize(query);

  if (!normalizedQuery) {
    return [];
  }

  const terms = normalizedQuery.split(" ");

  return state.lessons
    .map((lesson) => {
      const title = normalize(lesson.title);
      const body = normalize(lesson.plainText);

      let score = 0;

      for (const term of terms) {
        /*
         * Title matches are deliberately weighted heavily.
         */
        if (title.includes(term)) {
          score += 20;
        }

        /*
         * Exact title word.
         */
        if (title.split(" ").includes(term)) {
          score += 10;
        }

        /*
         * Body occurrences.
         */
        let position = body.indexOf(term);

        while (position !== -1) {
          score += 1;
          position = body.indexOf(term, position + term.length);
        }
      }

      /*
       * Exact phrase gets another boost.
       */
      if (
        title.includes(normalizedQuery)
      ) {
        score += 25;
      }

      if (
        body.includes(normalizedQuery)
      ) {
        score += 5;
      }

      return {
        lesson,
        score,
      };
    })
    .filter((result) => result.score > 0)
    .sort((a, b) => b.score - a.score)
    .slice(0, 20);
}

/* -------------------------------------------------- */
/* Search UI */
/* -------------------------------------------------- */

function openSearch() {
  searchDialog.showModal();

  searchInput.value = "";

  searchResults.innerHTML = "";

  searchStatus.textContent =
    "Type to search the wiki.";

  state.searchResults = [];
  state.selectedSearchResult = 0;

  requestAnimationFrame(() => {
    searchInput.focus();
  });
}

function closeSearch() {
  searchDialog.close();
}

function createExcerpt(text, query) {
  const normalizedText = text.replace(/\s+/g, " ");
  const lowerText = normalizedText.toLowerCase();
  const lowerQuery = query.toLowerCase();

  const position = lowerText.indexOf(lowerQuery);

  if (position === -1) {
    return normalizedText.slice(0, 180) + "...";
  }

  const start = Math.max(0, position - 80);
  const end = Math.min(
    normalizedText.length,
    position + query.length + 100
  );

  let excerpt = normalizedText.slice(start, end);

  if (start > 0) {
    excerpt = "..." + excerpt;
  }

  if (end < normalizedText.length) {
    excerpt += "...";
  }

  return excerpt;
}

function highlight(text, query) {
  const escaped = escapeHtml(text);

  if (!query) {
    return escaped;
  }

  const escapedQuery = escapeHtml(query);

  return escaped.replace(
    new RegExp(
      `(${escapedQuery.replace(/[.*+?^${}()|[\]\\]/g, "\\$&")})`,
      "gi"
    ),
    "<mark>$1</mark>"
  );
}

function renderSearchResults(query) {
  const results = searchLessons(query);

  state.searchResults = results;
  state.selectedSearchResult = 0;

  if (!query.trim()) {
    searchStatus.textContent =
      "Type to search the wiki.";

    searchResults.innerHTML = "";

    return;
  }

  if (results.length === 0) {
    searchStatus.textContent =
      "No matching lessons.";

    searchResults.innerHTML = "";

    return;
  }

  searchStatus.textContent =
    `${results.length} result${results.length === 1 ? "" : "s"}`;

  searchResults.innerHTML = results
    .map(
      ({ lesson }, index) => {
        const excerpt = createExcerpt(
          lesson.plainText,
          query
        );

        return `
          <a
            class="search-result ${index === 0 ? "selected" : ""}"
            href="?lesson=${encodeURIComponent(lesson.path)}"
            data-result-index="${index}"
          >
            <div class="search-result-title">
              ${highlight(lesson.title, query)}
            </div>

            <div class="search-result-path">
              ${escapeHtml(lesson.path)}
            </div>

            <div class="search-result-excerpt">
              ${highlight(excerpt, query)}
            </div>
          </a>
        `;
      }
    )
    .join("");

  for (const result of searchResults.querySelectorAll(
    ".search-result"
  )) {
    result.addEventListener("click", (event) => {
      event.preventDefault();

      const index = Number(
        result.dataset.resultIndex
      );

      const lesson =
        state.searchResults[index]?.lesson;

      if (!lesson) {
        return;
      }

      const lessonIndex = state.lessons.findIndex(
        (item) => item.path === lesson.path
      );

      closeSearch();

      showLesson(lessonIndex);

      window.scrollTo({
        top: 0,
        behavior: "smooth",
      });
    });
  }
}

/* -------------------------------------------------- */
/* Search keyboard navigation */
/* -------------------------------------------------- */

function updateSearchSelection() {
  const results =
    searchResults.querySelectorAll(".search-result");

  results.forEach((result, index) => {
    result.classList.toggle(
      "selected",
      index === state.selectedSearchResult
    );
  });

  results[
    state.selectedSearchResult
  ]?.scrollIntoView({
    block: "nearest",
  });
}

/* -------------------------------------------------- */
/* Theme */
/* -------------------------------------------------- */

function loadTheme() {
  const saved =
    localStorage.getItem("hamid-wiki-theme");

  if (saved === "light") {
    document.documentElement.dataset.theme =
      "light";
  }
}

function toggleTheme() {
  const isLight =
    document.documentElement.dataset.theme ===
    "light";

  if (isLight) {
    delete document.documentElement.dataset.theme;

    localStorage.setItem(
      "hamid-wiki-theme",
      "dark"
    );
  } else {
    document.documentElement.dataset.theme =
      "light";

    localStorage.setItem(
      "hamid-wiki-theme",
      "light"
    );
  }
}

/* -------------------------------------------------- */
/* Events */
/* -------------------------------------------------- */

searchButton.addEventListener(
  "click",
  openSearch
);

searchClose.addEventListener(
  "click",
  closeSearch
);

themeButton.addEventListener(
  "click",
  toggleTheme
);

searchInput.addEventListener(
  "input",
  () => {
    renderSearchResults(
      searchInput.value
    );
  }
);

searchInput.addEventListener(
  "keydown",
  (event) => {
    if (
      event.key === "ArrowDown"
    ) {
      event.preventDefault();

      if (state.searchResults.length > 0) {
        state.selectedSearchResult =
          Math.min(
            state.selectedSearchResult + 1,
            state.searchResults.length - 1
          );

        updateSearchSelection();
      }
    }

    if (
      event.key === "ArrowUp"
    ) {
      event.preventDefault();

      if (state.searchResults.length > 0) {
        state.selectedSearchResult =
          Math.max(
            state.selectedSearchResult - 1,
            0
          );

        updateSearchSelection();
      }
    }

    if (
      event.key === "Enter"
    ) {
      event.preventDefault();

      const selected =
        state.searchResults[
          state.selectedSearchResult
        ];

      if (!selected) {
        return;
      }

      const index = state.lessons.findIndex(
        (lesson) =>
          lesson.path ===
          selected.lesson.path
      );

      closeSearch();

      showLesson(index);

      window.scrollTo({
        top: 0,
        behavior: "smooth",
      });
    }

    if (
      event.key === "Escape"
    ) {
      closeSearch();
    }
  }
);

document.addEventListener(
  "keydown",
  (event) => {
    /*
     * Cmd+K on macOS
     * Ctrl+K everywhere else
     */
    if (
      (event.metaKey || event.ctrlKey) &&
      event.key.toLowerCase() === "k"
    ) {
      event.preventDefault();

      if (searchDialog.open) {
        searchInput.focus();
      } else {
        openSearch();
      }
    }
  }
);

window.addEventListener(
  "popstate",
  () => {
    const path = getRoute();

    const index = state.lessons.findIndex(
      (lesson) =>
        lesson.path === path
    );

    if (index !== -1) {
      showLesson(index, false);
    }
  }
);

/*
 * Clicking a lesson in the sidebar.
 */
lessonNav.addEventListener(
  "click",
  (event) => {
    const link =
      event.target.closest(".lesson-link");

    if (!link) {
      return;
    }

    event.preventDefault();

    const index =
      Number(link.dataset.index);

    showLesson(index);

    window.scrollTo({
      top: 0,
      behavior: "smooth",
    });
  }
);

/*
 * Close dialog if the user clicks outside it.
 */
searchDialog.addEventListener(
  "click",
  (event) => {
    if (
      event.target === searchDialog
    ) {
      closeSearch();
    }
  }
);

/* -------------------------------------------------- */
/* Start */
/* -------------------------------------------------- */

loadTheme();

loadLessons().catch((error) => {
  console.error(error);

  content.innerHTML = `
    <div class="error">
      <strong>Could not load the wiki.</strong>
      <p>${escapeHtml(error.message)}</p>
    </div>
  `;

  lessonNav.innerHTML = `
    <div class="error">
      Wiki content could not be loaded.
    </div>
  `;
});
