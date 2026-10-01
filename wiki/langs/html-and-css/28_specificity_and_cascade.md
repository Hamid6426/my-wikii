# 28 - Specificity and the Cascade

Two rules set the same property on one element. Which wins?

## Order of Checks

1. Importance: `!important` beats normal rules
2. Specificity: the more specific selector wins
3. Order: when equal, the rule written last wins

---

## Specificity

| Selector kind                                                | Weight |
| ------------------------------------------------------------ | ------ |
| Inline `style`                                               | 1000   |
| Id (`#title`)                                                | 100    |
| Class, attribute, pseudo-class (`.card`, `[type]`, `:hover`) | 10     |
| Element (`p`)                                                | 1      |

```css
p { color: black; }          /* 1 */
.note { color: green; }      /* 10 */
p.note { color: blue; }      /* 11, wins */
```

---

## Inheritance

Some properties pass from parent to child: `color`, `font-*`, `line-height`, `text-align`. Layout properties such as `margin` and `border` do not.

| Keyword   | Meaning                                           |
| --------- | ------------------------------------------------- |
| `inherit` | Take the parent's value                           |
| `initial` | Use the CSS default                               |
| `unset`   | `inherit` if it normally inherits, else `initial` |

---

## Cascade Layers

`@layer` groups rules so a layer's order beats specificity.

```css
@layer reset, base, components;
```

Later layers win over earlier ones.

---

## Gotchas

- Avoid `!important`. Fix the selector instead
- Keep selectors short and flat so they stay easy to override
