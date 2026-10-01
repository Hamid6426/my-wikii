# 37 - Custom Properties

Custom properties (CSS variables) store a value you reuse.

## Define and Use

```css
:root {
  --brand: #3366ff;
  --space: 1rem;
}

.button {
  background: var(--brand);
  padding: var(--space);
}
```

- Names start with `--`
- `:root` is the top of the page, so the variable is available everywhere
- `var(--name, fallback)` gives a fallback if it is not set

---

## Scope

A variable set on an element is seen by that element and its children.

```css
.card.dark {
  --brand: #000;     /* only inside .dark */
}
```

---

## Dark Mode

```css
:root {
  --bg: white;
  --text: #111;
}

@media (prefers-color-scheme: dark) {
  :root {
    --bg: #111;
    --text: #eee;
  }
}

body {
  background: var(--bg);
  color: var(--text);
}
```

---

## Change With JavaScript

```js
document.documentElement.style.setProperty("--brand", "#ff0000");
```

---

## The @property Rule

`@property` gives a custom property a type, an initial value and inheritance control. Typed properties can be animated.

```css
@property --angle {
  syntax: "<angle>";
  inherits: false;
  initial-value: 0deg;
}
.spin { transform: rotate(var(--angle)); transition: --angle 1s; }
.spin:hover { --angle: 90deg; }
```

Fallback with `var()`: `color: var(--brand, #333);` uses `#333` if `--brand` is not set.

---

## Gotchas

- Names are case-sensitive: `--Brand` is not `--brand`
- Variables cannot be used in media query conditions
