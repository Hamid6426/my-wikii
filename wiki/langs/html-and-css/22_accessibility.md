# 22 - Accessibility

Accessibility (a11y) means everyone can use the page, whatever their abilities or tools. Good HTML does most of the work.

## Who Benefits

| People                  | How they use the web                                |
| ----------------------- | --------------------------------------------------- |
| Blind or low vision     | Screen readers, screen magnifiers, large text       |
| Deaf or hard of hearing | Captions and transcripts                            |
| Motor impairments       | Keyboard only, voice control, trackballs, joysticks |
| Cognitive differences   | Clear structure, plain language, less motion        |
| Everyone                | A bright screen, a broken mouse, a noisy room       |

---

## Assistive Technology

| Tool                        | What it does                                                      |
| --------------------------- | ----------------------------------------------------------------- |
| Screen reader               | Reads the page aloud (NVDA, JAWS, VoiceOver, Orca)                |
| Braille display             | Shows the page text in braille                                    |
| Screen magnifier            | Enlarges part of the screen                                       |
| Voice recognition           | Lets the user speak commands and dictate text                     |
| Alternative pointing device | Trackball, joystick or touchpad for people who cannot use a mouse |
| Large-text keyboard         | Keys with big, high-contrast letters                              |

These tools read the HTML structure. Semantic HTML is what makes them work.

---

## The Basics

| Rule                       | How                                               |
| -------------------------- | ------------------------------------------------- |
| Use semantic elements      | `nav`, `main`, `button`, not `div` for everything |
| Give images text           | `alt` on every `img`                              |
| Label every input          | `label` with `for`                                |
| Set the language           | `<html lang="en">`                                |
| Work with a keyboard       | Tab, Enter, Space                                 |
| Keep enough color contrast | At least 4.5:1 for normal text                    |
| Do not rely on color alone | Add text or an icon                               |

---

## Heading Structure

Screen reader users jump from heading to heading to scan a page.

- One `h1`
- Go down one level at a time: `h1`, `h2`, `h3`
- Pick the level by structure, not by size. Size is CSS

---

## Auditing Tools

| Tool            | Use                                                      |
| --------------- | -------------------------------------------------------- |
| Lighthouse      | Built into Chrome DevTools. Gives an accessibility score |
| axe DevTools    | Browser extension that lists problems                    |
| WAVE            | Shows problems on the page itself                        |
| W3C validator   | Finds invalid HTML that confuses assistive tools         |
| A screen reader | The best test. Try NVDA (Windows) or VoiceOver (Mac)     |

Tools find about a third of the problems. Test by hand too.

---

## CSS and Accessibility

- Check contrast with the browser DevTools color picker, WebAIM Contrast Checker or Lighthouse. WCAG AA needs 4.5:1 for normal text and 3:1 for large text
- Hide content for everyone with `display: none` or `visibility: hidden` (screen readers skip it too)
- Hide it visually but keep it for screen readers with a visually-hidden class:

```css
.visually-hidden {
  position: absolute;
  width: 1px; height: 1px;
  overflow: hidden;
  clip-path: inset(50%);
  white-space: nowrap;
}
```

---

## Gotchas

- Do not test only with a mouse. Unplug it and tab through the page
