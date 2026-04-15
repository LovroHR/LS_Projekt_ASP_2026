# 📋 Typography Quick Reference - Tabela Vrijednosti

## Font Family Stack

| Tip | Font Stack | Korištenje |
|---|---|---|
| **Primarna** | `-apple-system, BlinkMacSystemFont, "Segoe UI", "Helvetica Neue", Roboto, "Noto Sans", sans-serif` | Svi tekstи, naslovi, forme |
| **Monospace** | `"SF Mono", Monaco, "Cascadia Code", "Roboto Mono", Menlo, Courier, monospace` | Timecode (00:15:30), verzije (v1.2), brojevi |

---

## Font Size Hierarchy

| Element | CSS Varijabla | px | rem | Primjena | Bootstrap |
|---|---|---|---|---|---|
| **Display Hero** | `--fs-display-1` | 72px | 4.5rem | Največća hero heading | `.display-1` |
| **Display Veliki** | `--fs-display-2` | 56px | 3.5rem | Veliki naslov sekcije | `.display-2` |
| **Display** | `--fs-display-6` | 48px | 3rem | Naslov stranice | `.display-6` |
| **H1** | `--fs-h1` | 40px | 2.5rem | Page title | `.h1` |
| **H2** | `--fs-h2` | 32px | 2rem | Section title | `.h2` |
| **H3** | `--fs-h3` | 24px | 1.5rem | Subsection | `.h3` |
| **H4** | `--fs-h4` | 20px | 1.25rem | Card title | `.h4` |
| **H5** | `--fs-h5` | 18px | 1.125rem | Small heading | `.h5` |
| **H6** | `--fs-h6` | 16px | 1rem | Tiny heading | `.h6` |
| **Body** | `--fs-body` | 16px | 1rem | Obični tekst | `<p>`, `.body` |
| **Body-SM** | `--fs-body-sm` | 15px | 0.95rem | Malo manji tekst | - |
| **Body-XS** | `--fs-body-xs` | 14px | 0.875rem | Mali tekst | - |
| **Small** | `--fs-small` | 13px | 0.8125rem | Labels, badges, forme | `.small` |
| **Tiny** | `--fs-tiny` | 12px | 0.75rem | Metadata, helper text | - |

---

## Font Weight Distribution

| Weight | Vrijednost | Korištenje | Primjer |
|---|---|---|---|
| **Light** | 300 | Subtle tekst, subtitles, dimmed | Podnaslov u herou |
| **Normal** | 400 | Body tekst, default | Obični paragraf |
| **Medium** | 500 | Form labels, UI elementy | `<label class="form-label">` |
| **Semibold** | 600 | Naslovi, buttons, akcije | Card titles, buttons, nav |
| **Bold** | 700 | Veliki naslovi, emphasis | H1, H2, page title |
| **Extrabold** | 800 | Branding, logo | Navbar brand |

### Bootstrap Klase
```
.fw-light       → 300
.fw-normal      → 400 (default)
.fw-medium      → 500
.fw-semibold    → 600
.fw-bold        → 700
.fw-extrabold   → 800
```

---

## Line Height (Readability)

| Vrijednost | CSS Varijabla | Primjena |
|---|---|---|
| **1.2** | `--lh-tight` | Naslovi (H1-H6) - kompaktan izgled |
| **1.5** | `--lh-normal` | Body tekst - čitljivo i ugodno |
| **1.75** | `--lh-relaxed` | Dugi paragrafi - dodatni prostor |
| **2.0** | `--lh-loose` | Liste, instrukcije - najširi razmak |

### Mapiranje po Elementima

| Element | Line-Height | Razlog |
|---|---|---|
| `<h1>`, `<h2>`, `<h3>` | `--lh-tight` (1.2) | Kompaktan, visokovisibilna |
| `<p>`, body | `--lh-normal` (1.5) | Optimalna za čitljivost |
| Dugi tekst (article) | `--lh-relaxed` (1.75) | Dodatni prostor |
| `<ul>`, `<ol>` | `--lh-loose` (2.0) | Jasna separacija stavki |
| Form polja | `--lh-normal` (1.5) | Standardna forma |
| Table | `--lh-relaxed` (1.75) | Vertikalni prostor |

---

## Letter Spacing (Profesionalni Izgled)

| Vrijednost | CSS Varijabla | Primjena |
|---|---|---|
| **-0.02em** | `--ls-tight` | Naslovi - čvrst, elegantna |
| **0em** | `--ls-normal` | Body tekst - default |
| **0.02em** | `--ls-wide` | Buttons, badges - raspršeno, vidjljivo |

### Primjena

```html
<!-- Naslovi sa tight spacing -->
<h1 style="letter-spacing: var(--ls-tight);">Page Title</h1>

<!-- Normal body -->
<p style="letter-spacing: var(--ls-normal);">Body text...</p>

<!-- Buttons sa wide spacing -->
<button class="btn" style="letter-spacing: var(--ls-wide);">Click me</button>
```

---

## Composition Spacing (Ritam Dokaza)

| Element | Margin-Bottom | CSS Varijabla |
|---|---|---|
| Naslovi (H1-H6) | 0.5rem (8px) | `--spacing-heading-bottom` |
| Paragrafi `<p>` | 1rem (16px) | `--spacing-paragraph-bottom` |
| Sekcije `<section>` | 2rem (32px) | `--spacing-section-bottom` |

---

## Bootstrap Integration Reference

| Element | Klase | Font-Size | Font-Weight | Line-Height |
|---|---|---|---|---|
| **Page Title** | `.display-6 .fw-bold` | 3rem | 700 | 1.2 |
| **Section Title** | `.h2 .fw-bold` | 2rem | 700 | 1.2 |
| **Card Title** | `.card-title .fw-semibold` | 1.25rem | 600 | 1.2 |
| **Body** | `<p>` | 1rem | 400 | 1.5 |
| **Lead Text** | `.lead` | 1.25rem | 400 | 1.5 |
| **Small Text** | `.small` | 0.8125rem | 400 | 1.5 |
| **Form Label** | `.form-label .fw-semibold` | 0.95rem | 600 | 1.5 |
| **Button** | `.btn .fw-semibold` | 1rem | 600 | 1.5 |
| **Badge** | `.badge .fw-semibold` | 0.8125rem | 600 | 1.5 |
| **Table Head** | `.fw-semibold` | 0.8125rem | 600 | 1.5 |
| **Table Body** | `<td>` | 0.95rem | 400 | 1.75 |
| **Nav Link** | `.nav-link .fw-medium` | 0.95rem | 500 | 1.5 |
| **Navbar Brand** | `.navbar-brand .fw-extrabold` | 1.5rem | 800 | 1.5 |

---

## Kombinacije za Česte Elemente

### Hero Heading
```html
<h1 class="display-3 fw-bold" 
    style="letter-spacing: var(--ls-tight); 
           line-height: var(--lh-tight);">
    🎵 LStudio
</h1>
```
- Font-Size: 3rem (48px)
- Font-Weight: 700 (bold)
- Line-Height: 1.2
- Letter-Spacing: -0.02em

### Page Title
```html
<h1 class="display-6 fw-bold">
    Naslov Stranice
</h1>
```
- Font-Size: 3rem
- Font-Weight: 700
- Line-Height: 1.2 (default za h1)

### Section Title
```html
<h2 class="h2 fw-bold mb-5">
    Što nudimo?
</h2>
```
- Font-Size: 2rem
- Font-Weight: 700
- Line-Height: 1.2
- Margin-Bottom: 3rem (mb-5)

### Card Title
```html
<h5 class="card-title fw-semibold">
    Profesionalni Studiji
</h5>
```
- Font-Size: 1.25rem
- Font-Weight: 600
- Line-Height: 1.2

### Body Paragraph
```html
<p class="fw-normal" 
   style="line-height: var(--lh-normal);">
    Obični tekst paragraf...
</p>
```
- Font-Size: 1rem
- Font-Weight: 400
- Line-Height: 1.5

### Form Label
```html
<label class="form-label fw-semibold">
    📧 Email Adresa
</label>
```
- Font-Size: 0.95rem
- Font-Weight: 600
- Color: var(--text-secondary)

### Button
```html
<button class="btn btn-lg fw-semibold" 
        style="letter-spacing: var(--ls-wide);">
    ✓ Prijavi se
</button>
```
- Font-Size: 1.125rem (btn-lg)
- Font-Weight: 600
- Letter-Spacing: 0.02em

### Table Header
```html
<th class="fw-semibold" 
    style="font-size: var(--fs-small);
           text-transform: uppercase;
           letter-spacing: var(--ls-wide);">
    Projekt
</th>
```
- Font-Size: 0.8125rem
- Font-Weight: 600
- Text-Transform: uppercase
- Letter-Spacing: 0.02em

### Badge/Status
```html
<span class="badge bg-success fw-semibold" 
      style="font-size: var(--fs-small);">
    Confirmed
</span>
```
- Font-Size: 0.8125rem
- Font-Weight: 600
- Background: var(--bg-color)

### Timecode (Monospace)
```html
<span style="font-family: var(--font-mono); 
             font-size: var(--fs-body-xs);">
    00:15:30
</span>
```
- Font-Family: Monospace stack
- Font-Size: 0.875rem

---

## Responsive Font Size Scaling

| Device | Breakpoint | Body Font | H1 Size | H2 Size |
|---|---|---|---|---|
| **Desktop** | 1024px+ | 16px | 40px (2.5rem) | 32px (2rem) |
| **Tablet** | 768-1023px | 16px | 32px (2rem) | 24px (1.5rem) |
| **Mobile** | 576-767px | 15px | 28px (1.75rem) | 20px (1.25rem) |
| **Small Mobile** | <576px | 14px | 28px (1.75rem) | 20px (1.25rem) |

---

## Color + Typography Kombinacije (Dark Mode)

| Element | Color | Font Weight | Korištenje |
|---|---|---|---|
| **Main Text** | `var(--text-primary)` (#F3F4F6) | 400-700 | Svi naslovi i body tekst |
| **Secondary Text** | `var(--text-secondary)` (#D1D5DB) | 400 | Form labele, muted text |
| **Accent Color** | `var(--cyan-bright)` (#00D9FF) | 600 | Links, highlights |
| **White** | `white` | 600-700 | Hero sekcija, white text |
| **Button Text** | `white` | 600 | CTA buttons |

### Minimalni Contrast Ratio
```
Text na Dark Background:
- Normalni tekst: minimum 4.5:1 (WCAG AA)
- Duži tekstи: minimum 3:1 zadovoljava, ali 4.5:1 je bolje
- Small tekst: trebam 4.5:1 minimum

Korišteni boje:
- #F3F4F6 (primary) na #0f172a (bg) = 13.8:1 ✓✓✓ Excellent
- #D1D5DB (secondary) na #0f172a (bg) = 10.2:1 ✓✓✓ Excellent
- #00D9FF (accent) na #0f172a (bg) = 7.5:1 ✓✓✓ Excellent
```

---

## CSS Custom Properties - Copy-Paste

```css
/* Direkt kopiraj u :root { } */

--font-primary: -apple-system, BlinkMacSystemFont, "Segoe UI", "Helvetica Neue", 
                Roboto, "Noto Sans", sans-serif;
--font-mono: "SF Mono", Monaco, "Cascadia Code", "Roboto Mono", 
             Menlo, Courier, monospace;

--fs-display-1: 4.5rem;
--fs-display-2: 3.5rem;
--fs-h1: 2.5rem;
--fs-h2: 2rem;
--fs-h3: 1.5rem;
--fs-h4: 1.25rem;
--fs-h5: 1.125rem;
--fs-h6: 1rem;
--fs-body: 1rem;
--fs-body-sm: 0.95rem;
--fs-body-xs: 0.875rem;
--fs-small: 0.8125rem;
--fs-tiny: 0.75rem;

--fw-light: 300;
--fw-normal: 400;
--fw-medium: 500;
--fw-semibold: 600;
--fw-bold: 700;
--fw-extrabold: 800;

--lh-tight: 1.2;
--lh-normal: 1.5;
--lh-relaxed: 1.75;
--lh-loose: 2;

--ls-tight: -0.02em;
--ls-normal: 0em;
--ls-wide: 0.02em;

--spacing-heading-bottom: 0.5rem;
--spacing-paragraph-bottom: 1rem;
--spacing-section-bottom: 2rem;
```

---

## Checklist za QA/Validaciju

- [ ] Svi `<h1>` koriste `fw-bold` i `--fs-h1` (2.5rem)
- [ ] Svi `<h2>` koriste `fw-bold` i `--fs-h2` (2rem)
- [ ] Svi `<h3>` koriste `fw-bold` ili `fw-semibold` i `--fs-h3`
- [ ] Body paragraf `<p>` koristi `--fs-body` (1rem) i `--lh-normal`
- [ ] Form labele koriste `fw-semibold` i `--fs-body-sm`
- [ ] Buttons koriste `fw-semibold` i `--ls-wide`
- [ ] Table header koristi `fw-semibold`, `text-transform: uppercase`, `--fs-small`
- [ ] Badges koriste `fw-semibold` i `--fs-small`
- [ ] Small/helper tekst koristi `--fs-small` ili `--fs-tiny`
- [ ] Timecode koristi `font-family: var(--font-mono)` i `--fs-body-xs`
- [ ] Nema hardcoded `px` vrijednosti - koristi `rem` ili CSS variables
- [ ] Dark mode kontrast je minimalno 4.5:1
- [ ] Responsive na <576px testiran

---

## Copy-Paste Kombinacije

### "Gotovi Set" - Najčešće Korišteni Stilovi

```html
<!-- HERO HEADING -->
<h1 class="display-3 fw-bold" style="letter-spacing: var(--ls-tight);">

<!-- PAGE TITLE -->
<h1 class="display-6 fw-bold">

<!-- SECTION TITLE -->
<h2 class="h2 fw-bold mb-5">

<!-- BODY TEXT -->
<p class="fw-normal" style="line-height: var(--lh-normal);">

<!-- FORM LABEL -->
<label class="form-label fw-semibold" style="font-size: var(--fs-body-sm);">

<!-- BUTTON -->
<button class="btn btn-lg fw-semibold" style="letter-spacing: var(--ls-wide);">

<!-- SMALL/HELPER TEXT -->
<small style="font-size: var(--fs-small); color: var(--text-secondary);">

<!-- BADGE -->
<span class="badge fw-semibold" style="font-size: var(--fs-small);">

<!-- TABLE HEADER -->
<th class="fw-semibold" style="font-size: var(--fs-small); 
   text-transform: uppercase; letter-spacing: var(--ls-wide);">

<!-- TIMECODE -->
<span style="font-family: var(--font-mono); font-size: var(--fs-body-xs);">
```

---

**Verzija**: 1.0  
**Datum**: 2026-04-15  
**Audio Production Management System**
