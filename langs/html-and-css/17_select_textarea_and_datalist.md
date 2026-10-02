# 17 - Select, Textarea and Datalist

## select

A drop-down list.

```html
<label for="color">Color</label>
<select id="color" name="color">
  <option value="">Choose one</option>
  <option value="red">Red</option>
  <option value="blue" selected>Blue</option>
</select>
```

| Element    | Use                                           |
| ---------- | --------------------------------------------- |
| `option`   | One choice. `value` is what is sent           |
| `optgroup` | A labelled group of options                   |
| `multiple` | Attribute on `select`. Allows several choices |

---

## textarea

Multi-line text.

```html
<label for="msg">Message</label>
<textarea id="msg" name="msg" rows="4" cols="40" maxlength="500"></textarea>
```

The starting text goes between the tags, not in `value`.

---

## datalist

Suggestions for a text input. The user can still type something else.

```html
<input list="browsers" name="browser" />
<datalist id="browsers">
  <option value="Firefox"></option>
  <option value="Chrome"></option>
</datalist>
```

---

## Other Useful Elements

| Element    | Use                                          |
| ---------- | -------------------------------------------- |
| `output`   | Shows a result, such as a slider value       |
| `progress` | A progress bar (`value`, `max`)              |
| `meter`    | A value in a known range, such as disk usage |
