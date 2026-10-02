# 34 - Grid

Grid lays items out in rows and columns at once.

```css
.grid {
  display: grid;
  grid-template-columns: 1fr 2fr 1fr;
  gap: 1rem;
}
```

`fr` is a share of the free space. Here the middle column is twice as wide.

---

## Defining Tracks

| Property                | Example                             |
| ----------------------- | ----------------------------------- |
| `grid-template-columns` | `200px 1fr`, `repeat(3, 1fr)`       |
| `grid-template-rows`    | `auto 1fr auto`                     |
| `gap`                   | `1rem` or `1rem 2rem` (row, column) |

## Responsive Without Media Queries

```css
.cards {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(220px, 1fr));
  gap: 1rem;
}
```

---

## Placing Items

```css
.header { grid-column: 1 / -1; }   /* from line 1 to the last line */
.sidebar { grid-row: span 2; }
```

Named areas:

```css
.layout {
  display: grid;
  grid-template-columns: 200px 1fr;
  grid-template-areas:
    "header header"
    "nav    main"
    "footer footer";
}
header { grid-area: header; }
nav    { grid-area: nav; }
main   { grid-area: main; }
footer { grid-area: footer; }
```

---

## Flexbox or Grid

| Use     | When                                          |
| ------- | --------------------------------------------- |
| Flexbox | One direction: a menu, a row of buttons       |
| Grid    | Two directions: a page layout, a card gallery |

They combine well. Grid for the page, flexbox inside the pieces.

---

## Grid vs Flexbox

| Flexbox                    | Grid                              |
| -------------------------- | --------------------------------- |
| One dimension              | Two dimensions                    |
| Content decides the layout | Layout decides where content goes |

---

## Gotchas

- Grid lines are numbered from 1, and `-1` is the last line
- Items go in source order unless you place them
