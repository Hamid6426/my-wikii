# 21 - Interactive Elements

Built-in widgets that work with no JavaScript.

## details and summary

A show and hide section.

```html
<details>
  <summary>What is HTML?</summary>
  <p>HyperText Markup Language.</p>
</details>
```

Add `open` to start expanded. Give several `details` the same `name` to make an accordion where only one stays open.

---

## dialog

A pop-up window.

```html
<dialog id="info">
  <p>Hello!</p>
  <form method="dialog">
    <button>Close</button>
  </form>
</dialog>

<button onclick="document.getElementById('info').showModal()">Open</button>
```

| Method        | Does                                            |
| ------------- | ----------------------------------------------- |
| `show()`      | Opens it as a normal element                    |
| `showModal()` | Opens it on top and blocks the rest of the page |
| `close()`     | Closes it                                       |

A form with `method="dialog"` closes the dialog on submit.

---

## Other Attributes

| Attribute          | Effect                                      |
| ------------------ | ------------------------------------------- |
| `hidden`           | Hides the element                           |
| `contenteditable`  | Lets the user edit the text in the element  |
| `tabindex="0"`     | Makes an element reachable with the Tab key |
| `draggable="true"` | Lets the user drag the element              |

---

## Gotchas

- Use a `button` for actions and an `a` for navigation. Do not make a `div` clickable
