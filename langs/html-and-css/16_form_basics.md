# 16 - Form Basics

## The form Element

```html
<form action="/signup" method="post">
  <!-- inputs go here -->
</form>
```

| Attribute | Purpose                                              |
| --------- | ---------------------------------------------------- |
| `action`  | The URL that receives the data                       |
| `method`  | `get` (data in the URL) or `post` (data in the body) |

Use `post` for anything private or that changes data.

---

## The input Element

```html
<input type="text" id="name" name="name" placeholder="e.g. Quincy Larson" />
```

| Attribute     | Purpose                                                                     |
| ------------- | --------------------------------------------------------------------------- |
| `type`        | The kind of input: `text`, `email`, `number`, `radio`, `checkbox`, and more |
| `name`        | The key used when the form is sent                                          |
| `id`          | Links the input to its label                                                |
| `value`       | The starting value. For a `button` type, the button text                    |
| `placeholder` | A hint shown while the field is empty                                       |
| `size`        | How many characters are visible                                             |

Radio buttons with the same `name` form a group. Only one can be picked.

---

## Form States

| State     | When                                       | CSS          |
| --------- | ------------------------------------------ | ------------ |
| Focus     | The user selected the input (click or Tab) | `:focus`     |
| Hover     | The cursor is over it                      | `:hover`     |
| Disabled  | Not editable and not sent                  | `:disabled`  |
| Read-only | Not editable but sent                      | `:read-only` |
| Required  | Must be filled                             | `:required`  |
| Valid     | Passes the rules                           | `:valid`     |
| Invalid   | Fails a rule                               | `:invalid`   |
| Checked   | A ticked checkbox or radio                 | `:checked`   |

Never remove the focus outline without a replacement. States help users know what is happening.

---

## Gotchas

- An input without a `name` is not sent with the form
- A placeholder is not a label. See [Labels, Buttons and Fieldsets](19_labels_buttons_and_fieldsets.md)
