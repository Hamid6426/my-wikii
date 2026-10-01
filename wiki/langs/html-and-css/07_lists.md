# 07 - Lists

## Unordered List

Bullets. Order does not matter.

```html
<ul>
  <li>catnip</li>
  <li>laser pointers</li>
</ul>
```

---

## Ordered List

Numbers. Order matters.

```html
<ol>
  <li>Boil water</li>
  <li>Add pasta</li>
</ol>
```

Useful attributes: `start="5"`, `reversed`, `type="a"`.

---

## Description List

Terms and their meanings.

```html
<dl>
  <dt>HTML</dt>
  <dd>HyperText Markup Language</dd>
  <dt>CSS</dt>
  <dd>Cascading Style Sheets</dd>
</dl>
```

| Element | Meaning         |
| ------- | --------------- |
| `dl`    | The list        |
| `dt`    | The term        |
| `dd`    | The description |

---

## Nested Lists

Put the inner list inside an `li`.

```html
<ul>
  <li>
    Fruit
    <ul>
      <li>Apple</li>
    </ul>
  </li>
</ul>
```

---

## Styling Lists

```css
ul {
  list-style: square inside;   /* type, position */
  list-style-type: none;       /* remove markers, for menus */
  padding-left: 0;             /* remove the default indent */
}
li { margin-bottom: 0.5rem; }  /* or line-height: 1.6 */
```

| Property              | Values                                        |
| --------------------- | --------------------------------------------- |
| `list-style-type`     | `disc`, `circle`, `square`, `decimal`, `none` |
| `list-style-position` | `outside` (default), `inside`                 |
| `list-style-image`    | `url(...)`                                    |

Space items with `margin` or `line-height`, not empty `li` elements. For links styled as list items, see lesson 04.

---

## Gotchas

- Only `li` elements are allowed directly inside `ul` and `ol`
- Navigation menus are usually a `ul` of links
