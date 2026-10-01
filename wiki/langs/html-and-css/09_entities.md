# 09 - Entities

## What is an Entity

A code for a character that HTML would otherwise read as markup, or that is hard to type.

```html
<p>This is an &lt;img /&gt; element</p>
```

Shows: This is an `<img />` element

---

## Common Entities

| Entity   | Shows | Name               |
| -------- | ----- | ------------------ |
| `&lt;`   | <     | Less than          |
| `&gt;`   | >     | Greater than       |
| `&amp;`  | &     | Ampersand          |
| `&quot;` | "     | Double quote       |
| `&nbsp;` |       | Non-breaking space |
| `&copy;` | ©     | Copyright          |

---

## Numeric Form

Any character can be written by its Unicode number.

```html
<p>&#169; and &#x1F600;</p>
```

---

## Gotchas

- Always escape `<` and `&` in text, or the browser may read them as markup
- With UTF-8 you can usually type `©` directly
- Do not use `&nbsp;` for layout. Use CSS spacing
