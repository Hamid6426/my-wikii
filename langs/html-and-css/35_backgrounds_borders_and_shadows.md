# 35 - Backgrounds, Borders and Shadows

## Backgrounds

```css
.hero {
  background-color: #f4f4f4;
  background-image: url("hero.jpg");
  background-size: cover;
  background-position: center;
  background-repeat: no-repeat;
}
```

| Property              | Common values                                     |
| --------------------- | ------------------------------------------------- |
| `background-size`     | `cover` (fill, crop), `contain` (fit whole image) |
| `background-position` | `center`, `top left`                              |
| `background-repeat`   | `repeat`, `no-repeat`                             |

Gradient:

```css
background: linear-gradient(to right, #ff7e5f, #feb47b);
```

---

## Borders

```css
.box {
  border: 2px solid #333;
  border-radius: 8px;       /* round corners */
}
.avatar { border-radius: 50%; }   /* circle on a square element */
```

Styles: `solid`, `dashed`, `dotted`, `none`.

`outline` is like a border but takes no space. Used for focus rings.

---

## Shadows

```css
.card {
  box-shadow: 0 2px 8px rgb(0 0 0 / 20%);
}
```

Order: horizontal offset, vertical offset, blur, color. Add `inset` for an inner shadow. `text-shadow` works the same way for text.

---

## Opacity

```css
.faded { opacity: 0.5; }
```

`opacity` fades the whole element and its children. For only the background, use a color with transparency.

---

## More Background Properties

```css
.hero {
  background-attachment: fixed;  /* scroll (default), fixed, local */
  background-size: 200px auto;   /* or cover, contain */
  background-repeat: repeat-x;   /* repeat, no-repeat, repeat-x, repeat-y */
  background-position: top right;
  background: url("a.png") center / cover no-repeat, linear-gradient(#fff, #ccc);
}
```

## Background Accessibility

- Text on a background image or gradient needs enough contrast everywhere. Add an overlay or a solid `background-color` fallback
- Background images are invisible to screen readers. Put meaningful images in `img` with `alt`

## Borders Around Images

```css
img { border: 4px solid #333; border-radius: 50%; padding: 4px; }
img { outline: 2px dashed gray; outline-offset: 4px; }
img { border-image: url("frame.png") 30 round; }
```

---

## Gotchas

- `cover` crops the image. Use `contain` when the whole image must show
- Set a `background-color` as a fallback and for text contrast
