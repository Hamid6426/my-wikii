# 14 - Semantic HTML

## Why It Matters

Semantic elements say what content is, not how it looks. Screen readers, search engines and other developers rely on that.

| Kind                        | Examples                           |
| --------------------------- | ---------------------------------- |
| Semantic                    | `header`, `nav`, `main`, `article` |
| Presentational (deprecated) | `center`, `big`, `font`            |

Use the right heading level too. Skipping levels breaks the outline for screen readers.

---

## Page Structure

```html
<body>
  <header>
    <h1>CatPhotoApp</h1>
    <nav>
      <ul>
        <li><a href="#photos">Photos</a></li>
        <li><a href="#videos">Videos</a></li>
      </ul>
    </nav>
  </header>

  <main>
    <section id="photos">
      <h2>Cat Photos</h2>
      <article>
        <h3>My First Post</h3>
        <p>Content of the post.</p>
      </article>
    </section>
  </main>

  <footer>
    <p>No Copyright</p>
  </footer>
</body>
```

---

## The Elements

| Element   | Use                                                             |
| --------- | --------------------------------------------------------------- |
| `header`  | Intro of the page or a section                                  |
| `nav`     | A group of navigation links                                     |
| `main`    | The main content. Once per page                                 |
| `section` | A themed part of the page, usually with a heading               |
| `article` | Self-contained content that makes sense alone (blog post, news) |
| `aside`   | Side content related to the main content                        |
| `footer`  | Closing info: copyright, links                                  |
| `figure`  | An image or diagram with a `figcaption`                         |

---

## Gotchas

- Use `div` only when no semantic element fits
- `section` without a heading is usually a `div`
