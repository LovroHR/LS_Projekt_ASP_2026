# 🎵 LStudio - Preporuke za Ujedinjenu Tipografiju

## 📋 Analiza Postojećeg Stanja

**Sadašnje stanje:**
- ✓ Bootstrap 5 framework koristan
- ✓ Dark theme sa purple-blue gradijentom implementiran
- ✓ CSS custom properties korišteni u `dark-theme.css`
- ✗ Nedostaje jasna font hijerarhija
- ✗ Font-size nije konzistentan kroz stranice
- ✗ Nedostaje specifikacija za line-height i letter-spacing
- ✗ System font stack je generički

---

## 1️⃣ IZBOR TIPOGRAFIJE

### 🎯 Preporuka: Sans-Serif Stack (Moderan, Čitljiv)

#### **Primarna Tipografija (bez serifа)**
```css
--font-primary: -apple-system, BlinkMacSystemFont, "Segoe UI", "Helvetica Neue", Roboto, "Noto Sans", sans-serif;
```

**Razlog:**
- ✓ Čitljiva na dark background
- ✓ Profesionalna i moderna
- ✓ Odgovara audio industry standardima
- ✓ Odličan fallback kroz sve platforme
- ✓ Već korištena u Bootstrap 5

#### **Sekundarna Tipografija (za kôd/brojeve)**
```css
--font-mono: "SF Mono", Monaco, "Cascadia Code", "Roboto Mono", Menlo, Courier, monospace;
```

**Korištenje:**
- Timecode vrijednosti (00:00:15)
- Tehnički detaljи
- Verzije projekta (v1.2.3)

#### **Font za Branding (Heading)**
```css
/* Koristi primarna sans-serif sa weights */
/* LStudio logotip ostaje sa emoji ili ikonom */
```

---

## 2️⃣ FONT-SIZE HIJERARHIJA

### Bootstrap 5 Default + Custom Prilagodbe

```css
:root {
    /* Base Font Size */
    --fs-base: 16px;              /* 1rem */
    
    /* Display / Hero (Landing Page) */
    --fs-display-1: 4.5rem;       /* H1 na hero sekciji */
    --fs-display-2: 3.5rem;       /* Veliki naslovi */
    
    /* Heading Hierarchy */
    --fs-h1: 2.5rem;              /* Naslov stranice (h1) */
    --fs-h2: 2rem;                /* Sekcijski naslov (h2) */
    --fs-h3: 1.5rem;              /* Podnaslovi (h3) */
    --fs-h4: 1.25rem;             /* Kartice naslov (h4) */
    --fs-h5: 1.125rem;            /* Manji naslov (h5) */
    --fs-h6: 1rem;                /* Najmanji naslov (h6) */
    
    /* Body */
    --fs-body: 1rem;              /* 16px - obični tekst */
    --fs-body-sm: 0.95rem;        /* ~15px - malo manji */
    --fs-body-xs: 0.875rem;       /* 14px - mali tekst */
    
    /* Small / Helper Text */
    --fs-small: 0.8125rem;        /* ~13px - oznake, forme */
    --fs-tiny: 0.75rem;           /* 12px - metadata */
}
```

### Mapiranje u Bootstrap Klase

| HTML Element | Bootstrap Klasa | Size | Primjena |
|---|---|---|---|
| `<h1>` | `.display-1` | 4.5rem | Hero sekcija (Index) |
| `<h1>` | `.display-6` ili `.h1` | 2.5rem | Naslov stranice |
| `<h2>` | `.h2` | 2rem | Sekcijski naslovi |
| `<h3>` | `.h3` | 1.5rem | Cardovi, tabele |
| `<h4>` | `.h4` | 1.25rem | Manji naslovi |
| `<p>` | `.lead` | 1.25rem | Uvodni paragraf |
| `<p>` | `.body` | 1rem | Obični tekst |
| `<small>` | `.small` | 0.875rem | Pomoćni tekst |

---

## 3️⃣ FONT-WEIGHT DISTRIBUCIJA

```css
:root {
    /* Font Weights - Clarity Hierarchy */
    --fw-thin: 100;               /* Ne koristi za tekst */
    --fw-light: 300;              /* Lagane linije u herou */
    --fw-normal: 400;             /* Glavni tekst, čitljivo */
    --fw-medium: 500;             /* Labels, UI akcije */
    --fw-semibold: 600;           /* Naslovi, ikone */
    --fw-bold: 700;               /* Važni naslovi, CTA */
    --fw-extrabold: 800;          /* Branding, Logo */
}
```

### Preporuke po Elementima

| Tip Elementa | Weight | Primjena |
|---|---|---|
| **Body Text** | 400 | Obični paragraf, opisi |
| **Labels** | 500 | Form labele, badge-i |
| **Link/Button** | 600 | Navigacija, CTA |
| **H1/H2** | 700 | Naslovi stranice, sekcije |
| **Hero H1** | 700 | Branding naslov |
| **Small Text** | 400 | Metadata, timestamp |

### Primjeri Bootstrap Integracije

```html
<!-- Hero Heading -->
<h1 class="display-3 fw-bold">🎵 LStudio</h1>

<!-- Sekcijski Naslov -->
<h2 class="h2 fw-bold mb-4">Što nudimo?</h2>

<!-- Podnaslov/Lead -->
<p class="lead fw-normal">Rezervirajte studio...</p>

<!-- Form Label -->
<label class="form-label fw-semibold">📧 Email Adresa</label>

<!-- Badge/Status -->
<span class="badge bg-info fw-medium">Pending</span>

<!-- Regular Text -->
<p class="text-muted fw-normal">Upravljaj booking...</p>
```

---

## 4️⃣ LINE-HEIGHT ZA ČITLJIVOST

```css
:root {
    /* Line Heights - Readability */
    --lh-tight: 1.2;              /* Naslovi - kompaktan izgled */
    --lh-normal: 1.5;             /* Body tekst - čitljivo */
    --lh-relaxed: 1.75;           /* Dugi paragrafi, forme */
    --lh-loose: 2;                /* Lista, instrukcije */
}
```

### Primjena po Elementima

```css
/* Naslovi */
h1, h2, h3, h4, h5, h6 {
    line-height: var(--lh-tight);  /* 1.2 */
    margin-bottom: 0.5rem;
}

/* Body Tekst */
p, .body, a {
    line-height: var(--lh-normal); /* 1.5 */
}

/* Dugi tekstovi */
article, .long-form, .description {
    line-height: var(--lh-relaxed); /* 1.75 */
}

/* Liste */
ul, ol, li {
    line-height: var(--lh-loose);  /* 2 */
}

/* Form polja */
.form-control, .form-label {
    line-height: var(--lh-normal); /* 1.5 */
}
```

---

## 5️⃣ CSS CUSTOM PROPERTIES - INTEGRACIJA

### Dodaj u `wwwroot/css/dark-theme.css`

```css
/* ==========================================
   TYPOGRAPHY SYSTEM - LStudio
   ========================================== */

:root {
    /* Font Family Stack */
    --font-primary: -apple-system, BlinkMacSystemFont, "Segoe UI", "Helvetica Neue", Roboto, "Noto Sans", sans-serif;
    --font-mono: "SF Mono", Monaco, "Cascadia Code", "Roboto Mono", Menlo, Courier, monospace;
    
    /* Font Sizes */
    --fs-base: 16px;
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
    
    /* Font Weights */
    --fw-light: 300;
    --fw-normal: 400;
    --fw-medium: 500;
    --fw-semibold: 600;
    --fw-bold: 700;
    --fw-extrabold: 800;
    
    /* Line Heights */
    --lh-tight: 1.2;
    --lh-normal: 1.5;
    --lh-relaxed: 1.75;
    --lh-loose: 2;
    
    /* Letter Spacing (Profesionalan izgled) */
    --ls-tight: -0.02em;
    --ls-normal: 0;
    --ls-wide: 0.02em;
    
    /* Existing Color Variables */
    --primary-purple: #7C3AED;
    --primary-blue: #6366F1;
    /* ... ostalo ... */
}

/* ==========================================
   GLOBAL TYPOGRAPHY
   ========================================== */

html {
    font-size: 16px;
    font-family: var(--font-primary);
}

body {
    font-size: var(--fs-body);
    font-weight: var(--fw-normal);
    line-height: var(--lh-normal);
    color: var(--text-primary);
}

/* Headings */
h1, h2, h3, h4, h5, h6 {
    font-weight: var(--fw-bold);
    line-height: var(--lh-tight);
    letter-spacing: var(--ls-tight);
    margin-bottom: 0.5rem;
    color: var(--text-primary);
}

h1, .h1 {
    font-size: var(--fs-h1);
}

h2, .h2 {
    font-size: var(--fs-h2);
}

h3, .h3 {
    font-size: var(--fs-h3);
}

h4, .h4 {
    font-size: var(--fs-h4);
    font-weight: var(--fw-semibold);
}

h5, .h5 {
    font-size: var(--fs-h5);
    font-weight: var(--fw-semibold);
}

h6, .h6 {
    font-size: var(--fs-h6);
}

/* Display Classes (Hero sekcija) */
.display-1 {
    font-size: var(--fs-display-1);
    font-weight: var(--fw-bold);
    line-height: var(--lh-tight);
}

.display-2 {
    font-size: var(--fs-display-2);
    font-weight: var(--fw-bold);
    line-height: var(--lh-tight);
}

.display-6 {
    font-size: 3rem;
    font-weight: var(--fw-bold);
    line-height: var(--lh-tight);
}

/* Paragraphs */
p {
    font-size: var(--fs-body);
    line-height: var(--lh-normal);
    margin-bottom: 1rem;
}

.lead {
    font-size: 1.25rem;
    font-weight: var(--fw-normal);
    line-height: var(--lh-normal);
}

small, .small {
    font-size: var(--fs-small);
    font-weight: var(--fw-normal);
}

/* Text Variants */
.text-muted {
    color: var(--text-secondary);
    font-weight: var(--fw-normal);
}

.text-white {
    font-weight: var(--fw-semibold);
}

/* Forms */
.form-label {
    font-size: var(--fs-body-sm);
    font-weight: var(--fw-semibold);
    margin-bottom: 0.5rem;
}

.form-control,
.form-select {
    font-size: var(--fs-body);
    font-weight: var(--fw-normal);
    line-height: var(--lh-normal);
}

/* Cards */
.card-title {
    font-size: var(--fs-h4);
    font-weight: var(--fw-semibold);
}

.card-text {
    font-size: var(--fs-body-sm);
    line-height: var(--lh-relaxed);
}

/* Buttons */
.btn {
    font-weight: var(--fw-semibold);
    letter-spacing: var(--ls-wide);
    text-transform: none;
}

.btn-lg {
    font-size: 1.125rem;
}

/* Navigation */
.nav-link {
    font-size: var(--fs-body-sm);
    font-weight: var(--fw-medium);
}

.navbar-brand {
    font-size: 1.5rem;
    font-weight: var(--fw-extrabold);
}

/* Tables */
.table {
    font-size: var(--fs-body-sm);
}

.table thead {
    font-weight: var(--fw-semibold);
}

/* Badges */
.badge {
    font-size: var(--fs-small);
    font-weight: var(--fw-semibold);
    letter-spacing: var(--ls-wide);
}

/* Monospace (za timecode, verzije) */
code,
pre,
.mono {
    font-family: var(--font-mono);
    font-size: var(--fs-body-xs);
    letter-spacing: var(--ls-normal);
}

/* ==========================================
   RESPONSIVE ADJUSTMENTS
   ========================================== */

@media (max-width: 768px) {
    html {
        font-size: 15px;
    }
    
    h1, .h1, .display-6 {
        font-size: 2rem;
    }
    
    h2, .h2 {
        font-size: 1.5rem;
    }
    
    .display-1 {
        font-size: 3rem;
    }
    
    .display-2 {
        font-size: 2.5rem;
    }
}

@media (max-width: 576px) {
    html {
        font-size: 14px;
    }
    
    h1, .h1 {
        font-size: 1.75rem;
    }
    
    h2, .h2 {
        font-size: 1.25rem;
    }
    
    .display-1 {
        font-size: 2.5rem;
    }
    
    p {
        line-height: var(--lh-relaxed);
    }
}
```

---

## 6️⃣ PRAKTIČNA PRIMJENA U RAZOR PAGES

### A) Index.cshtml (Hero Sekcija)

```html
<!-- Hero Section -->
<section class="hero-section text-white py-5" 
         style="background: linear-gradient(135deg, #7C3AED 0%, #6366F1 50%, #4F46E5 100%); 
                 min-height: 500px; display: flex; align-items: center;">
    <div class="container">
        <div class="row align-items-center">
            <div class="col-lg-6">
                <!-- Main Heading -->
                <h1 class="display-3 fw-bold mb-4" 
                    style="text-shadow: 2px 2px 8px rgba(0, 0, 0, 0.5); 
                           color: white;
                           letter-spacing: var(--ls-tight);">
                    🎵 LStudio
                </h1>
                
                <!-- Subheading -->
                <h2 class="h3 fw-normal mb-4" 
                    style="text-shadow: 1px 1px 4px rgba(0, 0, 0, 0.4);
                           font-weight: var(--fw-light);
                           line-height: var(--lh-normal);">
                    Vaš partner za profesionalnu audio produkciju
                </h2>
                
                <!-- Lead Text -->
                <p class="lead fw-normal" 
                   style="text-shadow: 1px 1px 2px rgba(0, 0, 0, 0.3);
                          line-height: var(--lh-relaxed);">
                    Rezervirajte studijske termine, upravljajte projektima i surađujte 
                    s producentima u jednoj aplikaciji.
                </p>
                
                <!-- CTA Buttons -->
                <div class="d-flex gap-3">
                    <a href="/Bookings/Index" 
                       class="btn btn-light btn-lg fw-semibold">
                        📅 Rezerviraj termin
                    </a>
                    <a href="/Auth/Register" 
                       class="btn btn-outline-light btn-lg fw-semibold">
                        📝 Registruj se
                    </a>
                </div>
            </div>
        </div>
    </div>
</section>

<!-- Features Section -->
<section class="features-section py-5">
    <div class="container">
        <!-- Section Title -->
        <h2 class="h2 fw-bold text-center mb-5">
            Što nudimo?
        </h2>
        
        <div class="row g-4">
            <div class="col-md-6 col-lg-3">
                <div class="card border-0 h-100" 
                     style="background: rgba(31, 41, 55, 0.8); 
                            border: 1px solid rgba(124, 58, 237, 0.2);">
                    <div class="card-body text-center">
                        <div style="font-size: 48px; margin-bottom: 15px;">🎤</div>
                        
                        <!-- Card Title -->
                        <h3 class="card-title fw-semibold mb-2">
                            Profesionalni Studiji
                        </h3>
                        
                        <!-- Card Text -->
                        <p class="card-text text-muted fw-normal" 
                           style="line-height: var(--lh-relaxed);">
                            Rezervirajte premium studio prostore sa najnovijom opremom
                        </p>
                    </div>
                </div>
            </div>
        </div>
    </div>
</section>
```

### B) Bookings/Index.cshtml (Admin Dashboard)

```html
<div class="container-fluid mt-4">
    <!-- Header Sekcija -->
    <div class="row mb-4">
        <div class="col-md-8">
            <!-- Page Title -->
            <h1 class="display-6 fw-bold mb-2">
                📅 Moji Studijski Termini
            </h1>
            
            <!-- Page Description -->
            <p class="text-muted fw-normal" style="line-height: var(--lh-normal);">
                Upravljaj booking rezervacijama i dostupnim vremenima
            </p>
        </div>
        <div class="col-md-4 text-end">
            <a href="/Bookings/Create" class="btn btn-primary btn-lg fw-semibold">
                <i class="bi bi-plus-circle"></i> Nova Rezervacija
            </a>
        </div>
    </div>

    <!-- Card Sekcija -->
    <div class="card mb-4 border-0" 
         style="background: rgba(31, 41, 55, 0.8); 
                border: 1px solid rgba(124, 58, 237, 0.2) !important;">
        <div class="card-body">
            <div class="row g-3">
                <div class="col-md-4">
                    <!-- Label sa specifičnom tipografijom -->
                    <label class="form-label fw-semibold" 
                           style="color: #D1D5DB; font-size: var(--fs-body-sm);">
                        Pretraži po projektu ili klijentu
                    </label>
                    <input type="text" 
                           class="form-control form-control-lg" 
                           placeholder="Unesite naziv..." 
                           style="background: rgba(15, 23, 42, 0.8); 
                                  border: 1px solid rgba(124, 58, 237, 0.3); 
                                  color: #F3F4F6;
                                  font-weight: var(--fw-normal);">
                </div>
            </div>
        </div>
    </div>

    <!-- Table sa semantičkom tipografijom -->
    <div class="table-responsive">
        <table class="table table-dark table-hover">
            <thead>
                <tr>
                    <th class="fw-semibold" style="font-size: var(--fs-small);">Projekt</th>
                    <th class="fw-semibold" style="font-size: var(--fs-small);">Klijent</th>
                    <th class="fw-semibold" style="font-size: var(--fs-small);">Status</th>
                    <th class="fw-semibold" style="font-size: var(--fs-small);">Datum</th>
                </tr>
            </thead>
            <tbody>
                <tr>
                    <td class="fw-normal">Albumska produkcija</td>
                    <td class="text-muted fw-normal">John Doe</td>
                    <td>
                        <span class="badge bg-success fw-medium">Confirmed</span>
                    </td>
                    <td class="text-muted fw-normal">2026-04-15</td>
                </tr>
            </tbody>
        </table>
    </div>
</div>
```

### C) Auth/Login.cshtml (Forma)

```html
<div class="container mt-5">
    <div class="row justify-content-center">
        <div class="col-md-6 col-lg-5">
            <div class="card shadow-lg border-0" 
                 style="background: rgba(31, 41, 55, 0.9); 
                        border: 1px solid rgba(124, 58, 237, 0.2) !important;">
                
                <!-- Card Header -->
                <div class="card-header" 
                     style="background: linear-gradient(135deg, #7C3AED, #6366F1); 
                            border-bottom: 2px solid rgba(6, 182, 212, 0.3);">
                    <h3 class="mb-0 text-white fw-bold">🔐 Login</h3>
                </div>
                
                <div class="card-body p-4">
                    <form method="post">
                        <!-- Email Field -->
                        <div class="mb-3">
                            <label class="form-label fw-semibold" 
                                   style="color: #D1D5DB; font-size: var(--fs-body-sm);">
                                📧 Email Adresa
                            </label>
                            <input type="email" 
                                   class="form-control form-control-lg" 
                                   asp-for="Email" 
                                   placeholder="Unesi email" 
                                   required 
                                   style="background: rgba(15, 23, 42, 0.8); 
                                          border: 1px solid rgba(124, 58, 237, 0.3); 
                                          color: #F3F4F6;
                                          font-weight: var(--fw-normal);
                                          line-height: var(--lh-normal);">
                        </div>

                        <!-- Password Field -->
                        <div class="mb-4">
                            <label class="form-label fw-semibold" 
                                   style="color: #D1D5DB; font-size: var(--fs-body-sm);">
                                🔑 Lozinka
                            </label>
                            <input type="password" 
                                   class="form-control form-control-lg" 
                                   asp-for="Password" 
                                   placeholder="Unesi lozinku" 
                                   required 
                                   style="background: rgba(15, 23, 42, 0.8); 
                                          border: 1px solid rgba(124, 58, 237, 0.3); 
                                          color: #F3F4F6;
                                          font-weight: var(--fw-normal);
                                          line-height: var(--lh-normal);">
                        </div>

                        <!-- Submit Button -->
                        <button type="submit" 
                                class="btn btn-lg w-100 fw-semibold" 
                                style="background: linear-gradient(135deg, #7C3AED, #6366F1); 
                                       border: none; 
                                       color: white;
                                       letter-spacing: var(--ls-wide);">
                            ✓ Prijavi se
                        </button>
                    </form>

                    <hr class="my-4" style="border-color: rgba(124, 58, 237, 0.2);">

                    <!-- Helper Text -->
                    <div class="text-center">
                        <p class="text-muted mb-2 fw-normal" 
                           style="font-size: var(--fs-body-sm);">
                            Nemaš račun?
                        </p>
                        <a href="/Auth/Register" 
                           class="btn btn-outline-light btn-lg w-100 fw-semibold" 
                           style="border: 2px solid rgba(6, 182, 212, 0.5); 
                                  color: #06B6D4;
                                  letter-spacing: var(--ls-wide);">
                            📝 Registracija
                        </a>
                    </div>
                </div>
            </div>
        </div>
    </div>
</div>
```

---

## 7️⃣ SAŽETAK - QUICK REFERENCE

| Aspekt | Vrijednost | Bootstrap Klasa |
|---|---|---|
| **Font Family** | System sans-serif stack | `.body` |
| **Body Font Size** | 1rem (16px) | `.body`, `<p>` |
| **H1 Size** | 2.5rem | `.h1`, `<h1>` |
| **H2 Size** | 2rem | `.h2`, `<h2>` |
| **H3 Size** | 1.5rem | `.h3`, `<h3>` |
| **Body Line Height** | 1.5 | `.body` |
| **Heading Line Height** | 1.2 | `h1-h6` |
| **Body Font Weight** | 400 (normal) | - |
| **Button Font Weight** | 600 (semibold) | `.btn` |
| **Heading Font Weight** | 700 (bold) | `h1-h6` |
| **Small Text Size** | 0.875rem (14px) | `.small` |
| **Letter Spacing** | 0em (normal) | - |
| **Button Letter Spacing** | 0.02em (wide) | `.btn` |

---

## 8️⃣ IMPLEMENTACIJSKE KORAKE

### Faza 1: Setup CSS Variables
1. Dodaj `dark-theme.css` kompletan CSS iz sekcije 5️⃣
2. Testiraj da se svi headings i text renderiraju ispravno
3. Provjeri responsivnost na mobilnom i tabletnom

### Faza 2: Ažuriranje Razor Pages
1. Index.cshtml - primijeni primjer iz sekcije 6️⃣A
2. Bookings/Index.cshtml - primijeni iz 6️⃣B
3. Auth/*.cshtml - primijeni iz 6️⃣C

### Faza 3: Validacija
- [ ] Svi headings koriste `fw-bold` ili `fw-semibold`
- [ ] Body text ima `line-height: var(--lh-normal)` ili `1.5`
- [ ] Forms koriste `fw-semibold` za labele
- [ ] Buttons koriste `fw-semibold` i `letter-spacing: var(--ls-wide)`
- [ ] Testiranje na raznim screen size-ovima

### Faza 4: Consistency Audit
```bash
# Pretraži sve H1-H6 bez fw-bold/fw-semibold
# Pretraži sve P bez line-height specifikacije
# Pretraži sve form-label bez fw-semibold
```

---

## 9️⃣ DODATNE PREPORUKE

### Dark Mode Optimizacija
- ✓ Text contrast omjer min 4.5:1 za normal tekst
- ✓ Line-height 1.5+ za dugački tekst na dark backgroundu
- ✓ Letter-spacing 0.02em za buttons čini ih čitljivijima

### Audio Production Branding
- Koristi **monospace font** za timecode vrijednosti: `<code class="mono">00:15:30</code>`
- Koristi **semibold weights** za action labels (Bookmark, Approve, Comment)
- Koristi **light weight** samo za subtitles ili dimmed text

### Performance
- System fonts su ukupno 0KB dodatnog koda
- CSS custom properties se compajliraju u runtime
- Bez dodatnog HTTP requesta

---

## 🔟 CHECKLIST ZA IMPLEMENTACIJU

```markdown
- [ ] CSS custom properties dodani u dark-theme.css
- [ ] Index.cshtml ažuriran sa tipografijom
- [ ] Bookings/Index.cshtml ažuriran
- [ ] Auth/Login.cshtml ažuriran
- [ ] Auth/Register.cshtml ažuriran
- [ ] _Layout.cshtml headeri/footer provjeravani
- [ ] Font size responsive na mobile (<576px)
- [ ] Font size responsive na tablet (768px)
- [ ] Contrast ratio provjeran (WCAG AA standard)
- [ ] Testiranje na Chrome, Edge, Firefox
- [ ] Dark mode implementiran konzistentno
```

---

**Status**: Spreman za implementaciju  
**Datum**: 2026-04-15  
**Verzija**: 1.0
