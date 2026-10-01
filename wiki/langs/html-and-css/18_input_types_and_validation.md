# 18 - Input Types and Validation

## Common Types

| Type       | For                     |
| ---------- | ----------------------- |
| `text`     | Short text              |
| `email`    | An email address        |
| `password` | Hidden text             |
| `number`   | A number                |
| `date`     | A date picker           |
| `checkbox` | Yes or no               |
| `radio`    | One choice from a group |
| `file`     | A file upload           |

The right type gives mobile users the right keyboard and gives free checks.

---

## Validation Attributes

The browser checks these before it submits.

| Attribute    | Works on         | Rule                       |
| ------------ | ---------------- | -------------------------- |
| `required`   | Most inputs      | Must be filled             |
| `min`, `max` | `number`, `date` | Smallest and largest value |
| `minlength`  | Text inputs      | Fewest characters          |
| `maxlength`  | Text inputs      | Most characters            |
| `pattern`    | Text inputs      | Must match a regex         |

```html
<input type="text" name="name" minlength="5" maxlength="30" required />
<input type="number" name="quantity" min="2" max="10" />
```

---

## disabled and readonly

| Attribute  | Editable | Sent with form |
| ---------- | -------- | -------------- |
| `disabled` | No       | No             |
| `readonly` | No       | Yes            |

---

## Gotchas

- Browser checks can be bypassed. Always validate again on the server
