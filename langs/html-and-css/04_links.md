# 04 - Links

## The a Element

```html
<a href="https://example.com">Visit Example</a>
```

`href` is where the link goes.

---

## target

Where to open the link.

| Value     | Opens in                                     |
| --------- | -------------------------------------------- |
| `_self`   | The current tab (default)                    |
| `_blank`  | A new tab                                    |
| `_parent` | The parent of the current frame              |
| `_top`    | The top-most window, even from nested frames |

```html
<a href="https://example.com" target="_blank" rel="noopener">New tab</a>
```

---

## Phone and Email

```html
<a href="tel:+11234567890">Call us</a>
<a href="mailto:example@email.com">Email us</a>
```

---

## Links Inside the Page

Give the target an `id`, then link to `#id`.

```html
<a href="#about">Go to About</a>

<section id="about">
  <h2>About</h2>
</section>
```

Good for skip links and tables of contents.

---

## Link States

Style each state with CSS.

| State      | When                    |
| ---------- | ----------------------- |
| `:link`    | Not visited yet         |
| `:visited` | The user has been there |
| `:hover`   | The cursor is over it   |
| `:focus`   | It has keyboard focus   |
| `:active`  | It is being clicked     |

Write them in this order in CSS: link, visited, focus, hover, active.

---

## Gotchas

- Write link text that makes sense alone. Avoid "click here"
- Add `rel="noopener"` with `target="_blank"` for external links (modern browsers do this by default)
