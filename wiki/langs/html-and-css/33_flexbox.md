# 33 - Flexbox

Flexbox lays items out in one direction, a row or a column.

```html
<div class="row">
  <div>One</div>
  <div>Two</div>
  <div>Three</div>
</div>
```

```css
.row {
  display: flex;
  gap: 1rem;
}
```

---

## Container Properties

| Property          | Values                                                  | Does                            |
| ----------------- | ------------------------------------------------------- | ------------------------------- |
| `flex-direction`  | `row`, `column`                                         | The main axis                   |
| `justify-content` | `flex-start`, `center`, `space-between`, `space-around` | Spacing along the main axis     |
| `align-items`     | `stretch`, `center`, `flex-start`, `flex-end`           | Spacing across the main axis    |
| `flex-wrap`       | `nowrap`, `wrap`                                        | Allow items to go to a new line |
| `gap`             | `1rem`                                                  | Space between items             |

---

## Item Properties

| Property      | Does                                    |
| ------------- | --------------------------------------- |
| `flex-grow`   | Share of extra space the item takes     |
| `flex-shrink` | How much it shrinks when space is short |
| `flex-basis`  | Starting size                           |
| `flex`        | Shorthand: `flex: 1` means grow equally |
| `align-self`  | Override `align-items` for one item     |
| `order`       | Change the visual order                 |

---

## Center Anything

```css
.center {
  display: flex;
  justify-content: center;
  align-items: center;
}
```

---

## When to Use Flexbox

Use flexbox for one direction at a time (a row or a column): navbars, toolbars, card rows, centering. Use grid for rows and columns together (lesson 34).

---

## Gotchas

- With `column`, `justify-content` works vertically
- `align-items` needs the container to have height to show an effect
- `order` changes looks only. Screen readers keep the HTML order
