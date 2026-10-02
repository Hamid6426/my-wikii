# 41 - CSS Conventions and Debugging

## Starter Reset

```css
*, *::before, *::after { box-sizing: border-box; }
body { margin: 0; line-height: 1.5; }
img { max-width: 100%; display: block; }
```

---

## Naming

BEM (Block, Element, Modifier) gives flat, predictable class names.

```css
.card { }
.card__title { }      /* part of card */
.card--featured { }   /* variation of card */
```

| Rule                                         | Why                               |
| -------------------------------------------- | --------------------------------- |
| Style with classes, not ids                  | Low specificity, easy to override |
| Name by purpose, not looks                   | `.error`, not `.red`              |
| Avoid deep selectors (`div ul li a`)         | They break when HTML changes      |
| One file per area, or one ordered file       | Easy to find things               |
| Use custom properties for colors and spacing | One place to change               |

---

## Order Inside a Rule

Pick one order and keep it, for example: layout, box, text, color, other.

---

## Debugging With DevTools

| Step                   | How                                       |
| ---------------------- | ----------------------------------------- |
| Inspect an element     | Right click, then Inspect                 |
| See which rules apply  | The **Styles** pane                       |
| See a crossed-out rule | Another rule won, or the value is invalid |
| See the box model      | The **Computed** tab diagram              |
| Try a value            | Edit it in the Styles pane                |

---

## Common Problems

| Problem                 | Check                                                     |
| ----------------------- | --------------------------------------------------------- |
| Style does not apply    | Typo, wrong selector, or a more specific rule wins        |
| `width` has no effect   | The element is `inline`                                   |
| `z-index` has no effect | The element is not positioned                             |
| Page scrolls sideways   | Something is wider than the screen. Look for fixed widths |
| Old styles still show   | Hard refresh with `Ctrl+Shift+R`                          |
