# 29 - Colors and Units

## Colors

```css
p {
  color: red;                    /* name */
  color: #ff0000;                /* hex */
  color: rgb(255 0 0);           /* red green blue */
  color: hsl(0 100% 50%);        /* hue saturation lightness */
  color: rgb(255 0 0 / 50%);     /* with transparency */
}
```

| Format | Good for                                               |
| ------ | ------------------------------------------------------ |
| Name   | Quick tests                                            |
| Hex    | Copying from a design tool                             |
| `hsl`  | Making lighter or darker shades by changing one number |

---

## Absolute Units

| Unit | Meaning       |
| ---- | ------------- |
| `px` | One CSS pixel |

## Relative Units

| Unit  | Relative to                         |
| ----- | ----------------------------------- |
| `%`   | The parent's value                  |
| `em`  | The font size of the element itself |
| `rem` | The font size of the root (`html`)  |
| `vw`  | 1% of the screen width              |
| `vh`  | 1% of the screen height             |
| `ch`  | The width of the character `0`      |
| `fr`  | A share of free space in a grid     |

Use `rem` for font sizes so the page respects the user's browser setting.

---

## calc and clamp

```css
width: calc(100% - 2rem);
font-size: clamp(1rem, 2.5vw, 2rem);   /* min, preferred, max */
```

---

## Color Theory in Brief

| Idea          | Meaning                                                     |
| ------------- | ----------------------------------------------------------- |
| Hue           | The color itself, as an angle on the color wheel (0 to 360) |
| Saturation    | How strong the color is                                     |
| Lightness     | How light or dark                                           |
| Complementary | Opposite colors on the wheel: strong contrast               |
| Analogous     | Neighbours on the wheel: calm and harmonious                |

Named colors (`tomato`, `rebeccapurple`) are handy for quick tests. About 140 exist. Use `rgb()`, `hsl()` or hex for exact brand colors.

## Gradients

```css
.a { background: linear-gradient(to right, #f06, #fc0); }
.b { background: linear-gradient(45deg, red 0%, blue 100%); }
.c { background: radial-gradient(circle at center, white, black); }
```

| Function          | Shape                       |
| ----------------- | --------------------------- |
| `linear-gradient` | Along a straight line       |
| `radial-gradient` | Outward from a center       |
| `conic-gradient`  | Around a center, like a pie |

---

## Gotchas

- `0` needs no unit
- Contrast matters. Check text against its background
