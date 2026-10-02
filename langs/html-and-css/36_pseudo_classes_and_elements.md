# 36 - Pseudo-classes and Pseudo-elements

## Pseudo-classes (one colon)

Select an element in a certain state or position.

| Selector           | Matches                           |
| ------------------ | --------------------------------- |
| `a:hover`          | The cursor is over it             |
| `a:focus`          | It has focus                      |
| `a:focus-visible`  | Focus shown by keyboard use       |
| `a:visited`        | A visited link                    |
| `button:active`    | Being pressed                     |
| `input:checked`    | A checked box or radio            |
| `input:disabled`   | A disabled input                  |
| `input:invalid`    | Fails validation                  |
| `li:first-child`   | The first item                    |
| `li:last-child`    | The last item                     |
| `li:nth-child(2n)` | Every even item                   |
| `p:not(.note)`     | A `p` without class `note`        |
| `:is(h1, h2)`      | Any in the list, shorter to write |

Link order that works: `:link`, `:visited`, `:focus`, `:hover`, `:active`.

---

## Pseudo-elements (two colons)

Style a part of an element, or add content.

| Selector          | Matches                          |
| ----------------- | -------------------------------- |
| `p::first-line`   | The first line of text           |
| `p::first-letter` | The first letter                 |
| `::before`        | Generated content before         |
| `::after`         | Generated content after          |
| `::selection`     | Text the user selected           |
| `::placeholder`   | The placeholder text of an input |

```css
.required::after {
  content: " *";
  color: red;
}
```

`content` is required for `::before` and `::after`.

---

## Pseudo-class Groups

| Group           | Examples                                                                    |
| --------------- | --------------------------------------------------------------------------- |
| User action     | `:hover` `:active` `:focus` `:focus-visible` `:focus-within`                |
| Input           | `:checked` `:disabled` `:required` `:valid` `:invalid` `:placeholder-shown` |
| Location        | `:link` `:visited` `:target` `:any-link`                                    |
| Tree-structural | `:first-child` `:last-child` `:nth-child(2n)` `:only-child` `:empty`        |
| Functional      | `:not()` `:is()` `:where()` `:has()`                                        |

Link states go in this order: `:link`, `:visited`, `:hover`, `:active` (LVHA). Never remove the focus outline without a replacement.

```css
a:hover, a:focus-visible { text-decoration: underline; }
li:not(:last-child) { border-bottom: 1px solid #ddd; }
```

---

## Gotchas

- Do not use `outline: none` without adding another focus style
- Text added with `content` may not be read by screen readers
