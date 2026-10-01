# 30 - Text and Fonts

## Font Properties

```css
body {
  font-family: "Inter", system-ui, sans-serif;
  font-size: 1rem;
  font-weight: 400;
  font-style: normal;
  line-height: 1.5;
}
```

| Property      | Sets                                                |
| ------------- | --------------------------------------------------- |
| `font-family` | Fonts in order. The last should be a generic family |
| `font-size`   | Size                                                |
| `font-weight` | `400` normal, `700` bold                            |
| `line-height` | Space between lines. `1.5` is a good start          |

Generic families: `serif`, `sans-serif`, `monospace`, `system-ui`.

---

## Text Properties

| Property          | Example                                                        |
| ----------------- | -------------------------------------------------------------- |
| `text-align`      | `left`, `center`, `right`                                      |
| `text-decoration` | `none`, `underline`                                            |
| `text-transform`  | `uppercase`, `capitalize`                                      |
| `letter-spacing`  | `0.05em`                                                       |
| `white-space`     | `nowrap`, `pre`                                                |
| `text-overflow`   | `ellipsis` (with `overflow: hidden` and `white-space: nowrap`) |

---

## Web Fonts

```css
@font-face {
  font-family: "MyFont";
  src: url("myfont.woff2") format("woff2");
  font-display: swap;
}
```

Or link a Google Fonts stylesheet in `head`. `font-display: swap` shows text right away in a fallback font.

---

## Typography Basics

| Term       | Meaning                                          |
| ---------- | ------------------------------------------------ |
| Typeface   | The design family (Helvetica)                    |
| Font       | One style of it (Helvetica Bold 16px)            |
| Serif      | Has small strokes on letter ends. Good for print |
| Sans-serif | No strokes. Common on screens                    |
| Monospace  | Every letter the same width. For code            |
| Leading    | Line spacing (`line-height`)                     |
| Tracking   | Space between letters (`letter-spacing`)         |

Best practices:
- Use at most two font families
- Body text 16px or larger, `line-height` 1.4 to 1.6
- Keep lines to about 45 to 75 characters (`max-width: 65ch`)
- Build contrast with size and weight, not many fonts

## Web Safe Fonts and Stacks

Web safe fonts are installed on nearly every device: Arial, Verdana, Times New Roman, Georgia, Courier New. Always end a `font-family` list with a generic family.

```css
body { font-family: "Inter", Arial, sans-serif; }
```

Sources for external fonts: Google Fonts (link a stylesheet) and Font Squirrel (download files, then use `@font-face`).

## text-shadow

```css
h1 { text-shadow: 2px 2px 4px rgb(0 0 0 / 40%); }  /* x y blur color */
```

---

## Gotchas

- Use `woff2` files. They are the smallest
- Do not set `line-height` with a unit. A plain number scales with the font
