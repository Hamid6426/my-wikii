# 23 - ARIA

WAI-ARIA (Accessible Rich Internet Applications) adds attributes that tell assistive tools what an element is or does, when HTML alone cannot.

First rule: use a real HTML element if one exists. A `button` needs no ARIA. A `div` pretending to be a button does.

## Roles

`role` says what an element is.

```html
<div role="alert">Your changes were saved.</div>
<div role="navigation" aria-label="Main">...</div>
```

| Role                         | Means                                         |
| ---------------------------- | --------------------------------------------- |
| `alert`                      | An important message to announce at once      |
| `navigation`                 | A group of links (`nav` already has it)       |
| `button`                     | A clickable control (`button` already has it) |
| `dialog`                     | A pop-up window                               |
| `tablist`, `tab`, `tabpanel` | A tab interface                               |

---

## Names and Descriptions

| Attribute          | Use                                                   |
| ------------------ | ----------------------------------------------------- |
| `aria-label`       | A name for an element that has no visible text        |
| `aria-labelledby`  | Points to the `id` of the element that names this one |
| `aria-describedby` | Points to the `id` of extra help text                 |

```html
<button aria-label="Close">X</button>

<h2 id="billing">Billing</h2>
<section aria-labelledby="billing">...</section>

<input id="pw" type="password" aria-describedby="pw-help" />
<p id="pw-help">At least 8 characters.</p>
```

---

## Hiding and State

| Attribute            | Meaning                                               |
| -------------------- | ----------------------------------------------------- |
| `aria-hidden="true"` | Hide from screen readers. The element still shows     |
| `aria-expanded`      | Whether a menu or section is open                     |
| `aria-current`       | The current page in a menu (`aria-current="page"`)    |
| `aria-live`          | Announce changes to this area (`polite`, `assertive`) |

```html
<span aria-hidden="true">★</span>
<button aria-expanded="false" aria-controls="menu">Menu</button>
```

Hide an icon that only decorates, never a focusable control.

---

## Gotchas

- ARIA changes what is announced, not what the element does. You still add the keyboard code
- Wrong ARIA is worse than none
- `aria-hidden` on a button or link removes it from screen readers but it stays in the Tab order
