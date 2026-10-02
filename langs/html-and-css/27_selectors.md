# 27 - Selectors

A selector picks which elements a rule applies to.

## Basic

| Selector | Matches                       |
| -------- | ----------------------------- |
| `p`      | Every `p` element             |
| `.card`  | Elements with `class="card"`  |
| `#title` | The element with `id="title"` |
| `*`      | Everything                    |
| `a, p`   | Either one (a list)           |

---

## Combinators

| Selector  | Matches                                     |
| --------- | ------------------------------------------- |
| `div p`   | Any `p` inside a `div` (descendant)         |
| `div > p` | A `p` that is a direct child of a `div`     |
| `h2 + p`  | A `p` right after an `h2`                   |
| `h2 ~ p`  | Every `p` after an `h2`, same parent        |
| `p.note`  | A `p` that also has class `note` (no space) |

---

## Attribute Selectors

| Selector          | Matches                    |
| ----------------- | -------------------------- |
| `[disabled]`      | Has the attribute          |
| `[type="email"]`  | Attribute equals the value |
| `[href^="https"]` | Value starts with          |
| `[href$=".pdf"]`  | Value ends with            |
| `[class*="btn"]`  | Value contains             |

---

## Universal Selector

`*` matches every element. Its specificity is zero.

```css
* { box-sizing: border-box; }
```

## Attribute Selector Examples

```css
a[href] { color: blue; }                 /* links with an href */
a[title="Home"] { font-weight: bold; }   /* exact title */
[lang="fr"] { quotes: "«" "»"; }         /* language */
[data-lang="en"] { color: green; }       /* custom data attribute */
ol[type="a"] { list-style-type: lower-alpha; }  /* ordered list type */
[href$=".pdf" i] { color: red; }         /* i = ignore case */
```

---

## Gotchas

- `div p` and `div > p` differ: the first also matches nested `p` elements
- Prefer classes over ids for styling, because ids are hard to override
