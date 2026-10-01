# 40 - Transforms

Move, resize, rotate or tilt an element without changing the layout around it.

```css
.box {
  transform: translate(20px, 10px) rotate(15deg) scale(1.2);
}
```

| Function          | Does                  |
| ----------------- | --------------------- |
| `translate(x, y)` | Move                  |
| `scale(n)`        | Resize. `1` is normal |
| `rotate(deg)`     | Turn                  |
| `skew(x, y)`      | Tilt                  |

Several functions apply right to left in effect, so order matters.

---

## transform-origin

The point the transform pivots around. Default is the center.

```css
.door { transform-origin: left center; transform: rotate(30deg); }
```

---

## Hover Lift

```css
.card {
  transition: transform 0.2s;
}
.card:hover {
  transform: translateY(-4px);
}
```

---

## 3D

```css
.scene { perspective: 600px; }
.card  { transform: rotateY(40deg); }
```

---

## Gotchas

- Other elements do not move when one is transformed. The space stays
- A transform creates a new stacking context, which can change `z-index` behaviour
