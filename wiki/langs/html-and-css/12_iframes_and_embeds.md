# 12 - Iframes and Embeds

## iframe

An inline frame shows another page inside yours.

```html
<iframe src="https://www.example.com" title="Example Site"></iframe>
```

Always set a `title`. Screen readers use it.

---

## Embed a Video

Copy the embed code from YouTube or Vimeo.

```html
<iframe
  width="560"
  height="315"
  src="https://www.youtube.com/embed/VIDEO_ID"
  title="YouTube video player"
  allowfullscreen
></iframe>
```

`allowfullscreen` lets the user go full screen.

---

## Replaced Elements

A replaced element gets its content from an outside resource, not from CSS. Examples: `iframe`, `img`, `video`, `embed`.

Some elements act as replaced elements only in some cases, such as `<input type="image">`.

```html
<input type="image" src="button.png" alt="Submit" />
```

---

## Gotchas

- Many sites refuse to be framed
- Only embed sources you trust. Add `sandbox` to limit what the frame can do
