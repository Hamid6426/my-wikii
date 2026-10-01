# 02 - Elements and Attributes

## Elements

Most elements have an opening tag, content, and a closing tag.

```html
<p>Some text</p>
```

Elements nest inside each other. Close the inner one first.

```html
<p>This is <strong>important</strong>.</p>
```

---

## Void Elements

A void element has no content and no closing tag. Examples: `img`, `meta`, `br`, `hr`, `input`, `link`.

```html
<img src="cat.jpg" alt="A cat" /> <br />
```

The closing slash (`/>`) is optional. Both `<br>` and `<br />` are valid.

---

## Attributes

An attribute sits in the opening tag and adds information.

```html
<a href="https://example.com" title="Example">Visit</a>
```

| Part  | Example                 |
| ----- | ----------------------- |
| Name  | `href`                  |
| Value | `"https://example.com"` |

### Boolean attributes

Present means true. Absent means false. Examples: `disabled`, `readonly`, `required`, `checked`.

```html
<input type="text" required />
```

---

## Comments

```html
<!-- Not shown on the page -->
```

---

## Gotchas

- Always quote attribute values
- Tag names are case-insensitive, but write them in lowercase
- Writing `disabled="false"` still disables the element. Remove the attribute instead
