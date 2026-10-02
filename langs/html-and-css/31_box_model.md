# 31 - The Box Model

Every element is a box made of four layers.

```
+------------- margin -------------+
|  +--------- border ---------+    |
|  |  +------ padding ------+ |    |
|  |  |       content       | |    |
|  |  +---------------------+ |    |
|  +--------------------------+    |
+----------------------------------+
```

| Layer   | What it is                       |
| ------- | -------------------------------- |
| Content | The text or image                |
| Padding | Space inside, around the content |
| Border  | The line around the padding      |
| Margin  | Space outside, between elements  |

---

## Shorthand

```css
.box {
  padding: 10px;                 /* all sides */
  padding: 10px 20px;            /* top/bottom, left/right */
  padding: 10px 20px 30px 40px;  /* top, right, bottom, left (clockwise) */
  margin: 0 auto;                /* center a block with a set width */
  border: 1px solid black;
}
```

---

## box-sizing

| Value         | `width` covers                                   |
| ------------- | ------------------------------------------------ |
| `content-box` | Content only (default). Padding adds to the size |
| `border-box`  | Content, padding and border                      |

Use this on every page:

```css
*, *::before, *::after {
  box-sizing: border-box;
}
```

---

## Margin Collapse

Vertical margins of neighbouring blocks merge into the larger one, not the sum. Flex and grid items do not collapse.

---

## Sizing

```css
width: 300px;
max-width: 100%;
min-height: 100vh;
```

---

## CSS Reset

A reset removes browser default styles so every browser starts equal.

```css
*, *::before, *::after { box-sizing: border-box; }
* { margin: 0; padding: 0; }
img { max-width: 100%; display: block; }
```

Well-known ones: Eric Meyer's reset (removes everything), `normalize.css` (keeps useful defaults and fixes bugs), and Josh Comeau's modern reset.

## Filter

```css
img {
  filter: grayscale(100%);
  filter: blur(4px) brightness(120%);
}
```

| Function                                 | Effect                        |
| ---------------------------------------- | ----------------------------- |
| `blur()`                                 | Blurs                         |
| `brightness()` `contrast()` `saturate()` | Tone adjustments              |
| `grayscale()` `sepia()` `invert()`       | Color effects                 |
| `hue-rotate()`                           | Shifts hue                    |
| `drop-shadow()`                          | Shadow that follows the shape |

`filter` does not change layout, only how the element is painted. Overflow and transforms are in lessons 32 and 40.

---

## Gotchas

- Negative margins pull an element toward its neighbour
- Percent padding is based on the parent's width, even for top and bottom
