# 11 - Audio and Video

## audio

```html
<audio src="song.mp3" controls></audio>
```

## video

```html
<video src="clip.mp4" controls width="620" poster="cover.jpg"></video>
```

Formats: audio supports mp3, wav, ogg. Video supports mp4, ogg, webm.

---

## Attributes

All are boolean unless noted.

| Attribute  | Effect                                                  |
| ---------- | ------------------------------------------------------- |
| `controls` | Show the player controls. Without it nothing is visible |
| `autoplay` | Start as soon as possible                               |
| `loop`     | Replay forever                                          |
| `muted`    | Start silent                                            |
| `poster`   | An image shown before play (`video` only). Takes a URL  |

---

## Several Formats

Browsers support different formats. List several `source` elements and the browser picks the first it understands.

```html
<audio controls>
  <source src="audio.ogg" type="audio/ogg" />
  <source src="audio.mp3" type="audio/mpeg" />
  Your browser does not support audio.
</audio>
```

---

## Gotchas

- Browsers block `autoplay` with sound. Add `muted` if you need it
- Add captions with `<track kind="captions" src="en.vtt" srclang="en" />` for accessibility
