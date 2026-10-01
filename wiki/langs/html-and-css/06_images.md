# 06 - Images

## The img Element

```html
<img src="lasagna.jpg" alt="A slice of lasagna on a plate." />
```

| Attribute         | Purpose                                                      |
| ----------------- | ------------------------------------------------------------ |
| `src`             | Where the image file is                                      |
| `alt`             | Text for screen readers and for when the image fails to load |
| `width`, `height` | Size in pixels. Stops the page from jumping as it loads      |

Use `alt=""` for a purely decorative image.

---

## figure and figcaption

Group an image with a caption.

```html
<figure>
  <img src="cats.jpg" alt="Two tabby kittens sleeping on a couch." />
  <figcaption>Cats <strong>hate</strong> other cats.</figcaption>
</figure>
```

---

## Image Formats

| Format     | Best for                                          |
| ---------- | ------------------------------------------------- |
| WEBP, AVIF | Photos and graphics on the modern web             |
| JPG        | Photos, wide support                              |
| PNG        | Sharp graphics and transparency                   |
| SVG        | Logos and icons. Scales to any size, never blurry |

Three levers keep files small: size, format, compression.

---

## SVG

Scalable Vector Graphics describe an image with paths and equations, not pixels, so it stays sharp at any size and is often tiny.

```html
<img src="logo.svg" alt="Company logo" />

<svg width="24" height="24" viewBox="0 0 24 24" role="img" aria-label="Heart">
  <path d="M12 21s-8-5-8-11a4.5 4.5 0 0 1 8-3 4.5 4.5 0 0 1 8 3c0 6-8 11-8 11z" fill="red" />
</svg>
```

| Use SVG for                        | Use WEBP or JPG for |
| ---------------------------------- | ------------------- |
| Logos, icons, simple illustrations | Photos              |

Put an `svg` inline when you want to style it with CSS or JavaScript.

---

## Licenses

| License                | Meaning                                     |
| ---------------------- | ------------------------------------------- |
| Public domain, CC0     | Free to use with no restriction             |
| Other Creative Commons | Free to use, with conditions such as credit |

Check the license before you use an image.
