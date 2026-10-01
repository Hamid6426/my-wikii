# 24 - Accessible Content

## Images: alt Text

| Image                 | alt                                                   |
| --------------------- | ----------------------------------------------------- |
| A photo of a cat      | `alt="A tabby cat on a couch."`                       |
| A decoration          | `alt=""` (screen readers skip it)                     |
| A logo that is a link | `alt="Home"` (say where it goes)                      |
| A chart               | Give the key finding, and put the data in text nearby |

Do not start with "image of". The screen reader already says it is an image.

---

## Link Text

Link text should make sense when read alone. Screen reader users often list all links on a page.

| Bad          | Good                           |
| ------------ | ------------------------------ |
| `Click here` | `Read the accessibility guide` |
| `More`       | `More about our pricing`       |
| The raw URL  | A short description            |

---

## Tables

```html
<table>
  <caption>Conference schedule</caption>
  <thead>
    <tr>
      <th scope="col">Time</th>
      <th scope="col">Talk</th>
    </tr>
  </thead>
  <tbody>
    <tr>
      <th scope="row">9:00</th>
      <td>Welcome</td>
    </tr>
  </tbody>
</table>
```

- Use tables for data only, never for layout
- Add a `caption`
- Use `th` with `scope="col"` or `scope="row"` so each cell is tied to its headers

---

## Forms

- Every input has a `label`. Without one, a screen reader says only "edit text"
- Group related inputs with `fieldset` and `legend`
- Say what is wrong near the input, not by color alone
- Do not use `placeholder` as a label. It disappears when the user types

---

## Audio and Video

| Need                   | How                                                                             |
| ---------------------- | ------------------------------------------------------------------------------- |
| For deaf users         | Captions: `<track kind="captions" src="en.vtt" srclang="en" label="English" />` |
| For blind users        | Audio description, or a text description                                        |
| For audio-only content | A transcript on the page                                                        |
| For everyone           | Show `controls`. Do not autoplay with sound                                     |

---

## Keyboard Access

| Key         | Does                                   |
| ----------- | -------------------------------------- |
| `Tab`       | Move to the next link, button or input |
| `Shift+Tab` | Move back                              |
| `Enter`     | Activate a link or button              |
| `Space`     | Activate a button or tick a checkbox   |
| `Esc`       | Close a dialog or menu                 |

- Use native elements (`a`, `button`, `input`). They are focusable and work with the keyboard
- Keep a visible focus outline
- Keep the Tab order the same as the reading order
- Use `tabindex="0"` to add a custom control to the Tab order, never a positive number
- Add a skip link as the first element in `body`:

```html
<a href="#main">Skip to content</a>
<main id="main">...</main>
```
