# 26 - Adding CSS

CSS (Cascading Style Sheets) controls how HTML looks.

## A Rule

```css
p {
  color: blue;
  font-size: 18px;
}
```

| Part        | Example        |
| ----------- | -------------- |
| Selector    | `p`            |
| Property    | `color`        |
| Value       | `blue`         |
| Declaration | `color: blue;` |

---

## Three Ways to Add It

| Way      | Where                             | Use                    |
| -------- | --------------------------------- | ---------------------- |
| External | A `.css` file linked in `head`    | Almost always          |
| Internal | A `style` element in `head`       | One-page demos         |
| Inline   | A `style` attribute on an element | Rare. Hard to override |

```html
<link rel="stylesheet" href="styles.css" />

<style>
  p { color: blue; }
</style>

<p style="color: blue;">Inline</p>
```

---

## Comments

```css
/* Not applied */
```

---

## Gotchas

- End each declaration with `;`
- A typo in one declaration is skipped, and the rest still work
