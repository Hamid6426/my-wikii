# 13 - Responsive Images

## The Problem

A phone does not need a 3000 px photo. Give the browser several sizes or formats and let it choose.

---

## srcset and sizes

```html
<img
  src="cat-800.jpg"
  srcset="cat-400.jpg 400w, cat-800.jpg 800w, cat-1600.jpg 1600w"
  sizes="(max-width: 600px) 100vw, 600px"
  alt="A tabby cat on a couch."
/>
```

| Attribute | Meaning                                               |
| --------- | ----------------------------------------------------- |
| `srcset`  | The files and their widths (`400w` is 400 px wide)    |
| `sizes`   | How wide the image will be on screen, by screen width |
| `src`     | The fallback for old browsers                         |

`vw` is a percent of the screen width. `100vw` is the full width.

---

## picture

Pick a different format or a different crop.

```html
<picture>
  <source srcset="cat.avif" type="image/avif" />
  <source srcset="cat.webp" type="image/webp" />
  <img src="cat.jpg" alt="A tabby cat on a couch." />
</picture>
```

The browser uses the first `source` it supports. The `img` is required and holds the `alt`.

Art direction (a tighter crop on small screens):

```html
<picture>
  <source media="(max-width: 600px)" srcset="cat-square.jpg" />
  <img src="cat-wide.jpg" alt="A tabby cat on a couch." />
</picture>
```

---

## Lazy Loading

```html
<img src="cat.jpg" alt="A cat" loading="lazy" width="800" height="600" />
```

`loading="lazy"` loads the image only when it is near the screen. Do not use it on the first image the user sees.
