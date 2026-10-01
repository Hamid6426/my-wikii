# 32 - Display and Position

## display

| Value          | Behaviour                                               |
| -------------- | ------------------------------------------------------- |
| `block`        | Takes the full width, starts on a new line (`div`, `p`) |
| `inline`       | Flows with text. Ignores width and height (`span`, `a`) |
| `inline-block` | Flows with text, but accepts width and height           |
| `none`         | Removed from the page                                   |
| `flex`, `grid` | Layout containers. See the next lessons                 |

`visibility: hidden` hides an element but keeps its space.

---

## position

| Value      | Behaviour                                                       |
| ---------- | --------------------------------------------------------------- |
| `static`   | Normal flow (default)                                           |
| `relative` | Normal flow, can shift with `top`, `left`. Anchors children     |
| `absolute` | Removed from flow, placed against the nearest positioned parent |
| `fixed`    | Placed against the screen. Stays when scrolling                 |
| `sticky`   | Normal until it hits an edge, then sticks                       |

```css
.card { position: relative; }
.badge {
  position: absolute;
  top: 0;
  right: 0;
}

header {
  position: sticky;
  top: 0;
}
```

---

## z-index

Controls which positioned element is on top. A higher number is in front.

```css
.modal { position: fixed; z-index: 100; }
```

---

## Overflow

| Value     | Effect                      |
| --------- | --------------------------- |
| `visible` | Spills out (default)        |
| `hidden`  | Clips the extra             |
| `auto`    | Scrollbars only when needed |

---

## Float

`float` pushes an element left or right so text wraps around it. It was used for page layout before flexbox. Today use it only for wrapping text around an image.

```css
img { float: left; margin: 0 1rem 1rem 0; }
.after { clear: both; }   /* stop wrapping here */
```

A parent holding only floated children collapses to zero height. Fix with `display: flow-root` on the parent.

## Static, Relative, Absolute, Fixed, Sticky

| Value      | Behavior                                                        |
| ---------- | --------------------------------------------------------------- |
| `static`   | Default. Normal flow. `top` and `z-index` have no effect        |
| `relative` | Normal flow, can be nudged. Becomes the anchor for children     |
| `absolute` | Removed from flow. Placed against the nearest positioned parent |
| `fixed`    | Stays in place on screen while scrolling                        |
| `sticky`   | Scrolls normally, then sticks at an offset such as `top: 0`     |

---

## Gotchas

- `absolute` with no positioned parent is placed against the page
- `z-index` works only on positioned elements and flex or grid items
