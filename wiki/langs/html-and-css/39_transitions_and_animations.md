# 39 - Transitions and Animations

## Transitions

Smoothly change a property when it changes.

```css
.button {
  background: #3366ff;
  transition: background 0.3s ease;
}
.button:hover {
  background: #1a44cc;
}
```

| Part     | Meaning                                 |
| -------- | --------------------------------------- |
| Property | What to animate. `all` means everything |
| Duration | `0.3s`                                  |
| Timing   | `ease`, `linear`, `ease-in-out`         |
| Delay    | Optional wait before it starts          |

---

## Keyframe Animations

Run on their own, and can repeat.

```css
@keyframes fade-in {
  from { opacity: 0; }
  to   { opacity: 1; }
}

.popup {
  animation: fade-in 0.5s ease-out;
}
```

| Property                    | Example                        |
| --------------------------- | ------------------------------ |
| `animation-duration`        | `1s`                           |
| `animation-iteration-count` | `3`, `infinite`                |
| `animation-direction`       | `alternate`                    |
| `animation-fill-mode`       | `forwards` keeps the end state |

Steps in between:

```css
@keyframes pulse {
  0%   { transform: scale(1); }
  50%  { transform: scale(1.1); }
  100% { transform: scale(1); }
}
```

---

## Respect Reduced Motion

```css
@media (prefers-reduced-motion: reduce) {
  * { animation: none; transition: none; }
}
```

---

## Gotchas

- Animate `transform` and `opacity`. They are smooth. Animating `width` or `top` is slow
- `display: none` cannot be animated
