# 42 - UI Design Fundamentals

Design is how a page looks and how easy it is to use. Developers need the vocabulary to work with designers.

## Common Terms

| Term         | Meaning                                             |
| ------------ | --------------------------------------------------- |
| Wireframe    | Rough layout sketch without color or images         |
| Mockup       | Detailed static picture of the final design         |
| Prototype    | Clickable mockup that shows how it behaves          |
| Style guide  | Colors, fonts and spacing the team reuses           |
| Component    | A reusable piece: button, card, form field          |
| Design brief | Short document with goals, audience and constraints |
| Breakpoint   | Screen width where the layout changes               |

## Core Principles

| Principle        | Rule of thumb                                                        |
| ---------------- | -------------------------------------------------------------------- |
| Contrast         | Text must stand out from its background (4.5:1 minimum)              |
| Visual hierarchy | Size, weight and color show what matters most                        |
| Scale            | Bigger means more important. Use a consistent size scale             |
| Alignment        | Align edges to a shared line. Mixed alignment looks messy            |
| Whitespace       | Empty space groups items and helps reading. Do not fill every gap    |
| Images           | Match the content, compress them, add `alt`, keep a consistent style |

## Progressive Enhancement

Build the basic version first with plain HTML that works everywhere. Add CSS for looks, then JavaScript for extras. If a layer fails, the page still works.

## User-Centered Design

Start from what users need.

1. Gather requirements and research users (interviews, surveys)
2. Design and prototype
3. Test with real users
4. Improve and repeat

## Pattern Best Practices

| Pattern                | Best practice                                                                   |
| ---------------------- | ------------------------------------------------------------------------------- |
| Dark mode              | Offer a toggle, respect `prefers-color-scheme`, keep contrast, avoid pure black |
| Breadcrumbs            | Show the path, make the last item plain text, use `nav` with `aria-label`       |
| Cards                  | One topic per card, a clear title, one main action, consistent size             |
| Infinite scroll        | Keep the footer reachable, save scroll position, offer a "load more" option     |
| Modal dialogs          | Use sparingly, trap focus, close with Esc, return focus on close                |
| Progress indication    | Show steps on long forms, with "step 2 of 5" and a way back                     |
| Shopping carts         | Always visible, easy edits, show the total and costs early                      |
| Progressive disclosure | Show the basics first, reveal advanced options on request                       |
| Deferred registration  | Let people try before asking them to sign up (lazy registration)                |

## Design Tools

| Tool                   | Use                                     |
| ---------------------- | --------------------------------------- |
| Figma                  | Shared design, prototypes, dev handoff  |
| Sketch, Adobe XD       | Interface design (Sketch is macOS only) |
| Photoshop, Illustrator | Photos and vector graphics              |
| Browser DevTools       | Check real spacing, colors and fonts    |

Read a design brief before coding, and ask when sizes, states (hover, error, empty) or mobile layouts are missing.
