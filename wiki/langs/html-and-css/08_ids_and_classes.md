# 08 - IDs and Classes

## id

A unique name for one element. Use it once per page.

```html
<h1 id="title">Movie Review</h1>
<div id="red-box"></div>
```

No spaces. Use dashes or underscores.

---

## class

A group name. Reuse it on as many elements as you like. An element can have several.

```html
<div class="box red-box"></div>
<div class="box blue-box"></div>
```

---

## When to Use Which

| Need                                  | Use     |
| ------------------------------------- | ------- |
| Link to a spot on the page (`#title`) | `id`    |
| Find one element in JavaScript        | `id`    |
| Style many elements the same          | `class` |

In CSS: `#title { }` for an id, `.box { }` for a class.

---

## Grouping Elements

`div` is a generic block container. `span` is the inline version. Neither has meaning, so use them only when no semantic element fits (see [Semantic HTML](14_semantic_html.md)).

```html
<div class="card">
  <p>Total: <span class="price">$5</span></p>
</div>
```

---

## Gotchas

- Duplicate ids break links and scripts
- Name by purpose (`error-message`), not by look (`red-text`)
