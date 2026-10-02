# 01 - Getting Started

## What is HTML

HTML (HyperText Markup Language) describes the content and structure of a web page. CSS styles it. JavaScript makes it interactive.

---

## Your First Page

Save this as `index.html` and open it in a browser. No tools or server needed.

```html
<!DOCTYPE html>
<html lang="en">
  <head>
    <meta charset="utf-8" />
    <title>My Page</title>
  </head>
  <body>
    <h1>Hello, world</h1>
  </body>
</html>
```

---

## The Parts

| Part               | What it does                                             |
| ------------------ | -------------------------------------------------------- |
| `<!DOCTYPE html>`  | Tells the browser to use modern HTML                     |
| `<html lang="en">` | The root of the page. `lang` sets the language           |
| `<head>`           | Information about the page. Not shown on the page itself |
| `<meta charset>`   | Sets the character encoding. Use `utf-8`                 |
| `<title>`          | The text in the browser tab                              |
| `<body>`           | Everything the user sees                                 |

---

## UTF-8

A character encoding is how a computer stores letters as numbers. UTF-8 covers every language and emoji. Put the `charset` meta tag first in `<head>`, or some characters may show up garbled.

---

## Gotchas

- Without `<!DOCTYPE html>` the browser falls back to "quirks mode" and renders old-style
- Without `lang`, screen readers may read the page in the wrong language
