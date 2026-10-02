# 38 - Responsive Design

A responsive page works on any screen size.

## The Viewport Tag

Required in `head`, or phones show a tiny desktop page.

```html
<meta name="viewport" content="width=device-width, initial-scale=1" />
```

---

## Media Queries

Apply rules only when a condition is true.

```css
.grid { display: grid; grid-template-columns: 1fr; }

@media (min-width: 768px) {
  .grid { grid-template-columns: 1fr 1fr; }
}
```

| Query                              | Means                         |
| ---------------------------------- | ----------------------------- |
| `(min-width: 768px)`               | 768 px or wider               |
| `(max-width: 600px)`               | 600 px or narrower            |
| `(orientation: landscape)`         | Wider than tall               |
| `(prefers-color-scheme: dark)`     | The user chose dark mode      |
| `(prefers-reduced-motion: reduce)` | The user wants less animation |

---

## Mobile First

Write the small-screen style first, then add `min-width` queries for larger screens. It is less code and loads faster on phones.

---

## Flexible Pieces

| Technique                  | Example                                  |
| -------------------------- | ---------------------------------------- |
| Fluid width                | `max-width: 100%`                        |
| Images that never overflow | `img { max-width: 100%; height: auto; }` |
| Fluid text                 | `font-size: clamp(1rem, 2.5vw, 2rem)`    |
| Grids that adapt           | `repeat(auto-fit, minmax(220px, 1fr))`   |
| Wrapping flex rows         | `flex-wrap: wrap`                        |

---

## Common Breakpoints

| Name    | Width       |
| ------- | ----------- |
| Phone   | under 600px |
| Tablet  | 768px       |
| Laptop  | 1024px      |
| Desktop | 1280px      |

Pick breakpoints where your design breaks, not by device names.

---

## Container Queries

Respond to the size of the parent instead of the screen.

```css
.card-wrap { container-type: inline-size; }

@container (min-width: 400px) {
  .card { display: flex; }
}
```
