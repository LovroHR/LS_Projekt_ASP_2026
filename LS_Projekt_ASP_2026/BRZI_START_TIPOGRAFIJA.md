# 🎵 BRZI START - Tipografija Implementacija

## 📝 3 Koraka za Integraciju

### Korak 1: Dodaj CSS Link u _Layout.cshtml

Otvori: `Pages/Shared/_Layout.cshtml`

Dodaj redak **prije** `</head>` zatvorne oznake:

```html
<!-- Dodaj nakon ostalih CSS linkova -->
<link rel="stylesheet" href="~/css/typography.css" asp-append-version="true" />
```

**Primjer kompletnog `<head>` sekcije:**

```html
<head>
    <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title>@ViewData["Title"] - LStudio</title>
    
    <!-- Bootstrap CSS -->
    <link rel="stylesheet" href="~/lib/bootstrap/dist/css/bootstrap.min.css" />
    
    <!-- Site CSS -->
    <link rel="stylesheet" href="~/css/site.css" asp-append-version="true" />
    <link rel="stylesheet" href="~/css/dark-theme.css" asp-append-version="true" />
    
    <!-- ✅ NOVO - Typography System -->
    <link rel="stylesheet" href="~/css/typography.css" asp-append-version="true" />
    
    <link rel="stylesheet" href="~/LS_Projekt_ASP_2026.styles.css" asp-append-version="true" />
</head>
```

### Korak 2: Kopiraj CSS Datoteku

Kreiraj novu datoteku:
- **Putanja:** `wwwroot/css/typography.css`
- **Sadržaj:** Kopiraj kompletan sadržaj iz priloženog `typography.css` file-a

### Korak 3: Primijeni Tipografiju na Stranicama

Kopiraj relevantne primjere iz `TIPOGRAFIJA_PRIMJERI.html` u:
- `Pages/Index.cshtml`
- `Pages/Bookings/Index.cshtml`
- `Pages/Auth/Login.cshtml`
- `Pages/Auth/Register.cshtml`

---

## 🎯 Prioritet Primjene

### 🔴 Visokog Prioriteta (Obavezno)
```
1. Hero sekcija na Index.cshtml
2. Page title (display-6) na svim stranicama
3. Form labele sa fw-semibold
4. Button styling sa letter-spacing
5. Table header sa uppercase
```

### 🟡 Srednje Prioriteta (Preporučeno)
```
1. Card titles (fw-semibold)
2. Text body sa line-height
3. Alert messages
4. Badge styling
5. Nav links
```

### 🟢 Niskog Prioriteta (Dodatno)
```
1. Timecode comments
2. Version timeline
3. Footer
4. Responsive adjustments
```

---

## ✅ Checklist Validacije

Nakon primjene tipografije, provjeri:

```markdown
- [ ] Svi headings koriste fw-bold ili fw-semibold
- [ ] Body tekst ima line-height: 1.5 (ili var(--lh-normal))
- [ ] Form labele su fw-semibold
- [ ] Buttons imaju letter-spacing: var(--ls-wide)
- [ ] Table headers su uppercase sa fw-semibold
- [ ] Badges imaju fw-semibold
- [ ] Small text je sažet (fs-small)
- [ ] Hero sekcija ima text-shadow za kontrast
- [ ] Nema inline font-size CSS konflikta sa Bootstrap
- [ ] Responsive na mobile (<576px) testiran
- [ ] Contrast ratio je minimalno 4.5:1
- [ ] Dark mode izgleda konzistentan
```

---

## 🔍 Debugging - Uobičajeni Problemi

### Problem: Font se ne mijenja
**Rješenje:** 
- Provjeri je li `typography.css` link dodan prije `</head>`
- Očisti browser cache (Ctrl+Shift+R)
- Provjeri developer console za CSS errors

### Problem: Size je drugačiji nego očekivan
**Rješenje:**
- Provjeri Bootstrap klase (npr. `.h1` vs `<h1>`)
- Osiguraj da inline CSS ne override CSS properties
- Koristi `!important` kao zadnju opciju

### Problem: Line-height je loš
**Rješenje:**
- Dodaj `line-height: var(--lh-relaxed);` za dugačke tekstove
- Povećaj na `1.75` ili `2` za liste i forme
- Za naslove koristi `var(--lh-tight)` (1.2)

### Problem: Tekst se loše čita na dark backgroundu
**Rješenje:**
- Povećaj line-height na najmanje `1.5`
- Koristi `--text-primary` (#F3F4F6) umjesto bijele
- Dodaj `text-shadow` za hero sekcije
- Koristi `--text-secondary` za muted text

---

## 📊 Font Size Reference

```
Display 1: 4.5rem  (72px)  ← Hero heading
Display 2: 3.5rem  (56px)
Display 6: 3rem    (48px)
H1:        2.5rem  (40px)  ← Page title
H2:        2rem    (32px)  ← Section title
H3:        1.5rem  (24px)  ← Subsection
H4:        1.25rem (20px)  ← Card title
H5:        1.125rem(18px)
Body:      1rem    (16px)  ← Default
Body-sm:   0.95rem (15px)
Body-xs:   0.875rem(14px)
Small:     0.8125rem(13px) ← Labels
Tiny:      0.75rem (12px)  ← Metadata
```

---

## 🎨 Font Weight Reference

```
Light:     300  ← Subtle, subtitles
Normal:    400  ← Body text (default)
Medium:    500  ← Form labels
Semibold:  600  ← Headings, buttons
Bold:      700  ← Major headings
Extrabold: 800  ← Logo, branding
```

---

## 📱 Responsive Breakpoints

```
Desktop (1024px+):
- Body: 16px
- H1: 2.5rem
- H2: 2rem

Tablet (768px-1023px):
- Body: 16px
- H1: 2rem
- H2: 1.5rem

Mobile (576px-767px):
- Body: 15px
- H1: 1.75rem
- H2: 1.25rem

Small Mobile (<576px):
- Body: 14px
- H1: 1.75rem
- H2: 1.25rem
```

---

## 💡 Best Practices

### 1. Koristi CSS Custom Properties
```html
<!-- ✅ DOBRO -->
<h1 style="font-weight: var(--fw-bold); 
           font-size: var(--fs-h1);">

<!-- ❌ LOŠE -->
<h1 style="font-weight: 700; font-size: 40px;">
```

### 2. Bootstrap Klase za Frequency
```html
<!-- ✅ DOBRO - koristi Bootstrap klase -->
<p class="fw-normal">Text</p>

<!-- ⚠️ OK - inline za specifične vrijednosti -->
<p style="line-height: var(--lh-relaxed);">Text</p>
```

### 3. Dark Mode - Contrast First
```html
<!-- ✅ DOBRO - koristi CSS variables za boje -->
<p style="color: var(--text-primary);">

<!-- ❌ LOŠE - hardcoded boje -->
<p style="color: #000; background: #111;">
```

### 4. Timecode u Monospace
```html
<!-- ✅ DOBRO -->
<span style="font-family: var(--font-mono);">00:15:30</span>

<!-- ❌ LOŠE -->
<span style="font-family: Courier;">00:15:30</span>
```

### 5. Headings sa Text Shadow u Herou
```html
<!-- ✅ DOBRO - vidljivo na gradijentnom backgroundu -->
<h1 style="text-shadow: 2px 2px 8px rgba(0,0,0,0.5);">

<!-- ❌ LOŠE - nema kontrasta -->
<h1>
```

---

## 🚀 Napredne Integracije

### Timecode Comments Panel
```html
<span class="badge bg-info fw-medium" 
      style="font-family: var(--font-mono);
             font-size: var(--fs-small);">
    00:15:30
</span>
```

### Version Timeline
```html
<h4 class="fw-semibold" 
    style="font-size: var(--fs-h5);">
    v1.2 - Master Edit
</h4>
<small style="font-family: var(--font-mono);">
    2026-04-15 14:32:00
</small>
```

### Status Badges
```html
<span class="badge fw-semibold" 
      style="font-size: var(--fs-small);
             letter-spacing: var(--ls-wide);
             text-transform: uppercase;">
    Pending
</span>
```

---

## 📚 Datoteke za Pregled

1. **TIPOGRAFIJA_PREPORUKE.md** - Detaljne preporuke (čitaj prvo)
2. **TIPOGRAFIJA_PRIMJERI.html** - 29 praktičnih primjera
3. **typography.css** - CSS custom properties (kopiraj u wwwroot/css/)
4. Ovaj file - Brzi start guide

---

## ❓ FAQ

**P: Trebam li koristiti CSS variables?**
O: Preferira se jer je centralizirano, ali HTML inline style sa vrijednostima je OK.

**P: Mogu li miješati Bootstrap klase sa CSS variables?**
O: Da! `class="fw-bold"` + `style="line-height: var(--lh-normal);"` je OK.

**P: Gdje ide typography.css u liniji sa ostalim CSS-om?**
O: Nakon `dark-theme.css` i prije `LS_Projekt_ASP_2026.styles.css`.

**P: Je li potreban responsive CSS?**
O: Već je uključen u `typography.css` (@media queries).

**P: Trebam li dodati Google Fonts?**
O: Ne, system font stack je dovoljno dobar i brže je.

---

## 🎯 Sažetak

| Korak | Akcija | Status |
|---|---|---|
| 1 | Dodaj `typography.css` link u _Layout.cshtml | |
| 2 | Kreiraj `wwwroot/css/typography.css` | |
| 3 | Primijeni primjere na Index.cshtml | |
| 4 | Primijeni primjere na Bookings/Index.cshtml | |
| 5 | Primijeni primjere na Auth/*.cshtml | |
| 6 | Testiraj na mobile (<576px) | |
| 7 | Validiraj kontrast i čitljivost | |

---

**Status**: Spreman za primjenu  
**Verzija**: 1.0  
**Datum**: 2026-04-15
