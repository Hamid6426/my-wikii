# 03 - Text Elements

## Headings

`h1` to `h6`. Lower number means more important.

```html
<h1>Page title</h1>
<h2>Section</h2>
<h3>Subsection</h3>
```

- One `h1` per page
- Do not skip levels (`h2` straight to `h4`). Screen readers use headings to navigate

---

## Paragraphs and Breaks

```html
<p>This is a paragraph.</p>
<p>Line one<br />Line two</p>
<hr />
```

| Element | Use                                  |
| ------- | ------------------------------------ |
| `p`     | A paragraph                          |
| `br`    | A line break inside text             |
| `hr`    | A thematic break (a horizontal line) |

---

## Emphasis and Importance

```html
<p>Cats <em>love</em> lasagna.</p>
<p><strong>Warning:</strong> wear goggles.</p>
```

| Element  | Meaning                      | Looks  |
| -------- | ---------------------------- | ------ |
| `em`     | Stress emphasis              | Italic |
| `strong` | Strong importance or urgency | Bold   |

For looks only, use CSS, not these elements. See [Text Semantics](15_text_semantics.md).

---

## Buttons

```html
<button>Click me</button>
```

More in [Labels, Buttons and Fieldsets](19_labels_buttons_and_fieldsets.md).
