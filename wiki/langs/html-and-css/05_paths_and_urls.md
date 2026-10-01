# 05 - Paths and URLs

## Path Syntax

| Symbol | Means              |
| ------ | ------------------ |
| `/`    | Separates folders  |
| `.`    | The current folder |
| `..`   | The parent folder  |

```
public/index.html
./favicon.ico
../src/index.css
```

---

## Absolute URL

Full address with protocol and domain. Use for other websites.

```html
<a href="https://example.com/logo.svg">Logo</a>
```

---

## Relative Path

Location relative to the current file. Shorter, and keeps working if you move the site. Use for your own files.

```html
<!-- contact.html and about.html are in the same folder -->
<a href="about.html">About</a>

<!-- one folder up, then into images -->
<img src="../images/cat.jpg" alt="A cat" />
```

---

## Root-Relative Path

Starts with `/` and is read from the site root.

```html
<img src="/images/cat.jpg" alt="A cat" />
```

---

## Gotchas

- Paths are case-sensitive on most servers. `Cat.jpg` is not `cat.jpg`
- Always use forward slashes in HTML, even on Windows
- A path that works from `index.html` may break from a page in a subfolder
