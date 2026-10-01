# 20 - Tables

Use tables for data, not for page layout.

## Elements

| Element   | Use                    |
| --------- | ---------------------- |
| `table`   | The table              |
| `caption` | The title of the table |
| `thead`   | Header group           |
| `tbody`   | Body group             |
| `tfoot`   | Footer group           |
| `tr`      | A row                  |
| `th`      | A header cell          |
| `td`      | A data cell            |

---

## Example

```html
<table>
  <caption>Exam Grades</caption>

  <thead>
    <tr>
      <th>Last Name</th>
      <th>First Name</th>
      <th>Grade</th>
    </tr>
  </thead>

  <tbody>
    <tr>
      <td>Davis</td>
      <td>Alex</td>
      <td>54</td>
    </tr>
    <tr>
      <td>Doe</td>
      <td>Samantha</td>
      <td>92</td>
    </tr>
  </tbody>

  <tfoot>
    <tr>
      <td colspan="2">Average Grade</td>
      <td>73</td>
    </tr>
  </tfoot>
</table>
```

---

## Spanning Cells

| Attribute | Effect                     |
| --------- | -------------------------- |
| `colspan` | Cell spans several columns |
| `rowspan` | Cell spans several rows    |

---

## Gotchas

- Use `th` for headers, not bold `td`. Add `scope="col"` or `scope="row"` for screen readers
- Style tables with CSS, not old attributes like `border` and `cellpadding`
