# 15 - Text Semantics

Small elements that give inline text a meaning.

## Emphasis and Importance

| Element  | Meaning                                                           | Example                                 |
| -------- | ----------------------------------------------------------------- | --------------------------------------- |
| `em`     | Stress emphasis                                                   | `Never give up on <em>your</em> dreams` |
| `strong` | Strong importance                                                 | `<strong>Warning:</strong> allergens`   |
| `i`      | Different voice, foreign term, thought                            | `<i lang="fr">je ne sais quoi</i>`      |
| `b`      | Draw attention without extra importance (keywords, product names) | `the <b>SuperSound 3000</b>`            |
| `u`      | A non-text annotation, such as a spelling error                   | `<u>spling</u>`                         |
| `s`      | Content no longer accurate                                        | `<s>Meet at noon</s>`                   |

`i` and `em` carry meaning. If text only needs to look italic, use CSS.

### Which one

| Choose   | Over     | When                                                                   |
| -------- | -------- | ---------------------------------------------------------------------- |
| `em`     | `i`      | The meaning changes when you stress the word                           |
| `i`      | `em`     | Only the voice or type of text differs (a foreign phrase, a ship name) |
| `strong` | `b`      | The text is important, serious or urgent                               |
| `b`      | `strong` | You only want to draw the eye, such as a product name                  |

---

## Quotes and Sources

```html
<blockquote cite="https://example.com/book">
  Momentum is everything.
</blockquote>
<p>-Quincy Larson, <cite>Learn to Code</cite></p>

<p>As he said, <q cite="https://example.com/book">Keep going.</q></p>
```

| Element      | Use                              |
| ------------ | -------------------------------- |
| `blockquote` | A long quote from another source |
| `q`          | A short inline quote             |
| `cite`       | The title of the referenced work |

---

## Abbreviations, Contact and Time

```html
<abbr title="HyperText Markup Language">HTML</abbr>

<address>
  123 Main Street<br />
  Springfield, IL 62701
</address>

<time datetime="2026-10-01T20:00">Oct 1 at 8 pm</time>
```

`datetime` uses ISO 8601: `YYYY-MM-DDThh:mm:ss`. The `T` separates date and time.

---

## Superscript and Subscript

```html
<p>2<sup>2</sup> is 4.</p>
<p>CO<sub>2</sub></p>
```

---

## Code

```html
<p>Use the <code>print()</code> function.</p>

<pre><code>body {
  color: red;
}</code></pre>
```

`pre` keeps spaces and line breaks.

---

## Ruby Annotations

Pronunciation help, common in East Asian text. `rp` is a fallback for browsers without ruby support.

```html
<ruby>明日 <rp>(</rp><rt>Ashita</rt><rp>)</rp></ruby>
```
