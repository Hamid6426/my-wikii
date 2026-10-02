# 19 - Labels, Buttons and Fieldsets

## label

Every input needs a label. Clicking the label focuses the input, and screen readers read it.

Explicit: match `for` to the input `id`.

```html
<label for="email">Email Address:</label>
<input type="email" id="email" />
```

Implicit: wrap the input.

```html
<label>
  Full Name:
  <input type="text" />
</label>
```

---

## button

```html
<button type="submit">Submit Form</button>
<button type="reset">Reset Form</button>
<button type="button">Show Form</button>
```

| Type     | Does                            |
| -------- | ------------------------------- |
| `submit` | Sends the form (the default)    |
| `reset`  | Clears the form                 |
| `button` | Nothing. Use it with JavaScript |

---

## fieldset and legend

Group related inputs. `legend` is the caption of the group.

```html
<fieldset>
  <legend>Was this your first time at our hotel?</legend>

  <label for="yes">Yes</label>
  <input id="yes" type="radio" name="hotel-stay" value="yes" />

  <label for="no">No</label>
  <input id="no" type="radio" name="hotel-stay" value="no" />
</fieldset>
```

For checkboxes, give each its own `name`.

```html
<fieldset>
  <legend>Why did you choose us? (Check all that apply)</legend>

  <label for="location">Location</label>
  <input type="checkbox" id="location" name="location" value="location" />

  <label for="price">Price</label>
  <input type="checkbox" id="price" name="price" value="price" />
</fieldset>
```

---

## Styling Forms

| Task                                              | Tip                                                                                   |
| ------------------------------------------------- | ------------------------------------------------------------------------------------- |
| Text inputs                                       | Set `padding`, `border`, `font: inherit`, and a visible `:focus` style                |
| Search input, checkbox, radio                     | `appearance: none` removes the native look so you can restyle it                      |
| Special inputs (`date`, `range`, `file`, `color`) | Need vendor pseudo-elements like `::-webkit-slider-thumb` or `::file-selector-button` |

```css
input[type="checkbox"] {
  appearance: none;
  width: 1.2rem; height: 1.2rem;
  border: 2px solid #333;
}
input[type="checkbox"]:checked { background: #333; }
```

Keep the label and a visible focus ring when you replace native controls.

---

## Gotchas

- A `button` inside a form submits it by default. Set `type="button"` if you do not want that
