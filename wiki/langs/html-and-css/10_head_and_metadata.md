# 10 - Head and Metadata

## What Goes in head

Information for browsers and search engines. None of it is shown on the page.

```html
<head>
  <meta charset="utf-8" />
  <meta name="viewport" content="width=device-width, initial-scale=1" />
  <title>My Page</title>
  <meta name="description" content="A short summary of the page." />
  <link rel="stylesheet" href="./styles.css" />
</head>
```

---

## link and script

| Element  | Use                          |
| -------- | ---------------------------- |
| `link`   | Load a stylesheet or an icon |
| `script` | Load or embed JavaScript     |

```html
<link rel="stylesheet" href="./styles.css" />
<link rel="icon" href="./favicon.ico" />
<script src="./app.js" defer></script>
```

`rel` is the relationship of the file to the page. Prefer an external script file over code inside the page. `defer` runs the script after the HTML is read.

---

## SEO

SEO (Search Engine Optimization) makes a page easier to find. Start with a clear `title`, a `description`, one `h1`, and meaningful `alt` text.

---

## Open Graph

Controls how a link looks when shared on social media.

```html
<meta property="og:title" content="freeCodeCamp.org" />
<meta property="og:type" content="website" />
<meta property="og:image" content="https://example.com/preview.png" />
<meta property="og:url" content="https://example.com" />
```

| Property   | Sets                                          |
| ---------- | --------------------------------------------- |
| `og:title` | The title in the post                         |
| `og:type`  | The kind of content (article, website, video) |
| `og:image` | The preview image                             |
| `og:url`   | The link                                      |

---

## Gotchas

- Without the viewport meta tag, phones show a zoomed-out desktop layout
- Keep the description under about 160 characters
