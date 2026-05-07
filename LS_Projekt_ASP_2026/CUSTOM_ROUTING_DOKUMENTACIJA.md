# Custom Routing u ASP.NET Core - Dokumentacija i Analiza

## 📋 Pregled

Ovaj dokument opisuje sve custom route-ove i akcije kontrolera sa detaljnom analizom kako funkcioniraju.

---

## 🎯 BookingsController - 6 Akcija s Custom Routingom

### 1. **GetAllBookings** - Osnovna Custom Route
```
Route: GET /api/bookings/all
Atribut: [HttpGet("api/bookings/all")]
```
**Opis:** Vraća sve rezervacije iz baze.

**Primjer poziva:**
```
curl http://localhost:5001/api/bookings/all
```

**Odgovor:**
```json
{
  "success": true,
  "message": "Sve rezervacije su dohvaćene",
  "count": 6,
  "data": [...]
}
```

**Analiza Routinga:**
- Koristi atribut `[HttpGet()]` sa eksplicitnom rutom
- Ruta je potpuno prilagođena (`api/bookings/all`)
- Ne koristi default kontroler/akcija konvenciju

---

### 2. **GetBookingDetails** - Route s Parametrom
```
Route: GET /api/bookings/details/{id}
Atribut: [HttpGet("api/bookings/details/{id}")]
```
**Opis:** Dohvaća detaljne informacije o specifičnoj rezervaciji.

**Primjer poziva:**
```
curl http://localhost:5001/api/bookings/details/1
```

**Odgovor:**
```json
{
  "success": true,
  "message": "Detalji rezervacije su dohvaćeni",
  "data": {
    "id": 1,
    "startTime": "2026-04-03T10:00:00",
    "endTime": "2026-04-03T13:00:00",
    "purpose": "Vokal session za novi single",
    "status": "Confirmed",
    "totalPrice": 360.0
  }
}
```

**Analiza Routinga:**
- Koristi `{id}` kao segment u ruti
- Parametar `id` se automatski mapira iz URL-a
- Tip parametra se može eksplicitno definirati (npr. `{id:int}`)

---

### 3. **GetActiveBookings** - Custom Prefix Route
```
Route: GET /bookings-report/active
Atribut: [HttpGet("bookings-report/active")]
```
**Opis:** Vraća samo aktivne, pending i in-progress rezervacije.

**Primjer poziva:**
```
curl http://localhost:5001/bookings-report/active
```

**Analiza Routinga:**
- Koristi custom prefiks `bookings-report` umjesto standardnog `api/bookings`
- Pokazuje fleksibilnost u definiranju URL strukture
- Idealno za thematske grupe povezanih akcija

---

### 4. **GetBookingStatistics** - API Verzionisanje
```
Route: GET /api/v2/bookings/statistics
Atribut: [HttpGet("api/v2/bookings/statistics")]
```
**Opis:** Vraća detaljnu statistiku rezervacija.

**Primjer poziva:**
```
curl http://localhost:5001/api/v2/bookings/statistics
```

**Analiza Routinga:**
- `v2` u ruti označava verziju API-ja
- Omogućava održavanje različitih verzija API-ja
- Primjer: `v1` i `v2` mogu koexistati paralelno
- Važna praksa za backward compatibility

---

### 5. **GetBookingsByDate** - Route s Constraint-om
```
Route: GET /bookings-by-date/{date:datetime}
Atribut: [HttpGet("bookings-by-date/{date:datetime}")]
```
**Opis:** Vraća rezervacije za specifičan datum.

**Primjer poziva:**
```
curl http://localhost:5001/bookings-by-date/2026-04-05
```

**Analiza Routinga:**
- `{date:datetime}` je route constraint
- Osigurava da parametar bude validan DateTime
- Router automatski odbacuje nevaljane parametre (vraća 404)
- Ostali constraint-i: `:int`, `:double`, `:guid`, `:regex(pattern)`, itd.

---

### 6. **SearchBookings** - Query String Parametri
```
Route: GET /search-bookings
Atribut: [HttpGet("search-bookings")]
Parametri: [FromQuery] string? status
```
**Opis:** Traži rezervacije po statusu (query parametar).

**Primjer poziva:**
```
curl "http://localhost:5001/search-bookings?status=Confirmed"
curl "http://localhost:5001/search-bookings?status=Pending"
```

**Analiza Routinga:**
- `[FromQuery]` atribut mapira URL query parametre
- Parametri se proslijeđuju nakon `?` znaka
- Fleksibilan za opcijske filtere
- Query parametri se mogu kombinirati

---

## 🎼 ProjectsController - 6 Akcija s Custom Routingom

### 1. **GetAllProjects** - Class-level Route
```
Route: GET /api/projects
Atribut na klasi: [Route("api/[controller]")]
Akcija: [HttpGet]
```
**Opis:** Vraća sve audio projekte.

**Primjer poziva:**
```
curl http://localhost:5001/api/projects
```

**Analiza Routinga:**
- `[Route("api/[controller]")]` na klasi definira base rutu
- `[controller]` se zamjenjuje s "Projects" (minus "Controller")
- Sve akcije u klasi koriste ovaj prefix
- Čistiji i lakši za održavanje kod

---

### 2. **GetProjectById** - ID Parameter
```
Route: GET /api/projects/{id}
Atribut: [HttpGet("{id}")]
```
**Opis:** Vraća specifičan projekt.

**Primjer poziva:**
```
curl http://localhost:5001/api/projects/1
```

**Analiza Routinga:**
- Kombinira class-level rutu s akcijom
- Finalna ruta: `api/projects/1`
- `{id}` je obavezan parametar

---

### 3. **GetProjectsByType** - Nested Route
```
Route: GET /api/projects/by-type/{type}
Atribut: [HttpGet("by-type/{type}")]
```
**Opis:** Vraća projekte po tipu (Single, Album, EP, Podcast).

**Primjer poziva:**
```
curl http://localhost:5001/api/projects/by-type/Album
curl http://localhost:5001/api/projects/by-type/Podcast
```

**Analiza Routinga:**
- Zaglavlje od base rute: `/api/projects/by-type/{type}`
- Semantički jasno što akcija radi
- `by-type` je clarity segment koji navodi što slijedi

---

### 4. **GetProjectReport** - Admin Route
```
Route: GET /api/projects/admin/report
Atribut: [HttpGet("admin/report")]
```
**Opis:** Dohvaća detaljnu analizu i izvještaj o projektima (samo admin).

**Primjer poziva:**
```
curl http://localhost:5001/api/projects/admin/report
```

**Analiza Routinga:**
- `admin` prefiks jasno označava privilegirane operacije
- U praksi trebalo bi dodati `[Authorize(Roles = "Admin")]`
- Semantička struktura: `resource/permission/operation`

---

### 5. **AdvancedSearch** - Query Parametri (Više Filtara)
```
Route: GET /api/projects/advanced-search
Atribut: [HttpGet("advanced-search")]
Parametri: [FromQuery] string? genre, status, decimal? minBudget, maxBudget
```
**Opis:** Traži projekte po više kriterijuma.

**Primjer poziva:**
```
curl "http://localhost:5001/api/projects/advanced-search?genre=Rock&status=Active"
curl "http://localhost:5001/api/projects/advanced-search?minBudget=1000&maxBudget=5000"
curl "http://localhost:5001/api/projects/advanced-search?genre=Pop&status=Completed&minBudget=2500"
```

**Analiza Routinga:**
- Više query parametara za fleksibilnu pretragu
- Svi parametri su opcijski (`?`)
- Kombiniraju se s `&`
- URL encoding je automatski

---

### 6. **GetProjectsByClient** - Relacijska Route
```
Route: GET /api/projects/client/{clientId}
Atribut: [HttpGet("client/{clientId}")]
```
**Opis:** Vraća sve projekte određenog klijenta.

**Primjer poziva:**
```
curl http://localhost:5001/api/projects/client/1
curl http://localhost:5001/api/projects/client/2
```

**Analiza Routinga:**
- Semantička ruta koja povezuje dva resursa
- Pokazuje relaciju: "projekti OF klijenta"
- Alternativno bi se moglo: `/api/clients/1/projects`

---

## 📊 Usporedni Pregled Routing Strategija

| Strategija | Primjer | Prednosti | Mane |
|-----------|---------|-----------|------|
| **Attribute Routing** | `[HttpGet("api/bookings/all")]` | Fleksibilnost, eksplicitnost | Može biti verbose |
| **Conventional Routing** | `{controller}/{action}/{id}` | Jednostavnost | Manje fleksibilan |
| **API Versioning** | `/api/v1/` vs `/api/v2/` | Backward compatibility | Održavanje |
| **Route Constraints** | `{id:int}`, `{date:datetime}` | Validacija, sigurnost | Kompleksnost |
| **Query Parameters** | `?status=Active&genre=Pop` | Fleksibilni filteri | Kompleksniji URL |
| **Nested Routes** | `/api/projects/by-type/{type}` | Semantički jasno | Duži URL |

---

## 🔄 Redoslijed Evaluacije Ruta

ASP.NET Core evaluira rute u sljedećem redoslijedu:

1. **Exact match** - Najspecifičnije rute prvo
2. **Constraints** - Rute s ograničenjima
3. **Default routes** - Opće rute
4. **404** - Ako se ne poklapa

**Primjer:**
```
GET /api/bookings/all      → Точно mapira na GetAllBookings
GET /api/bookings/5        → Bi trebao mapirati na GetBookingDetails(5)
GET /api/bookings/xyz      → 404 (ne zadovoljava constraint)
```

---

## 🛡️ Best Practices za Custom Routing

### 1. **Konzistentnost**
```csharp
// ✅ Dobro - Konzistentan prefiks
[HttpGet("api/bookings/all")]
[HttpGet("api/bookings/details/{id}")]
[HttpGet("api/bookings/statistics")]

// ❌ Loše - Nekonzistentan
[HttpGet("bookings/all")]
[HttpGet("api/bookings/details/{id}")]
[HttpGet("appointments/statistics")]
```

### 2. **HTTP Metode**
```csharp
// ✅ RESTful
[HttpGet("api/bookings/{id}")]        // Dohvati
[HttpPost("api/bookings")]            // Kreiraj
[HttpPut("api/bookings/{id}")]        // Ažuriraj
[HttpDelete("api/bookings/{id}")]     // Obriši

// ❌ Loše - Non-RESTful
[HttpGet("api/bookings/create")]
[HttpGet("api/bookings/delete/{id}")]
```

### 3. **Verzionisanje**
```csharp
// Verzija 1 - Stara
[HttpGet("api/v1/bookings")]

// Verzija 2 - Nova s dodatnim poljem
[HttpGet("api/v2/bookings")]

// Obje verzije su dostupne
```

### 4. **Query vs Route**
```csharp
// ✅ Route - Obavezan parametar koji je dio URI-ja
[HttpGet("api/bookings/{id}")]

// ✅ Query - Opcijski filteri
[HttpGet("api/bookings/search?status=Active&genre=Pop")]

// ❌ Loše - Previše u query stringu
[HttpGet("api/bookings?operation=get&id=1")]
```

---

## 🧪 Testiranje Custom Ruta

### Korištenjem cURL:

```bash
# Test GetAllBookings
curl http://localhost:5001/api/bookings/all

# Test GetBookingDetails
curl http://localhost:5001/api/bookings/details/1

# Test GetActiveBookings
curl http://localhost:5001/bookings-report/active

# Test GetBookingStatistics
curl http://localhost:5001/api/v2/bookings/statistics

# Test GetBookingsByDate
curl http://localhost:5001/bookings-by-date/2026-04-05

# Test SearchBookings
curl "http://localhost:5001/search-bookings?status=Confirmed"

# Test GetAllProjects
curl http://localhost:5001/api/projects

# Test GetProjectsByType
curl http://localhost:5001/api/projects/by-type/Album

# Test GetProjectReport
curl http://localhost:5001/api/projects/admin/report

# Test AdvancedSearch
curl "http://localhost:5001/api/projects/advanced-search?genre=Rock&minBudget=1000"

# Test GetProjectsByClient
curl http://localhost:5001/api/projects/client/1
```

---

## 📝 Zaključak

Prilagođeno usmjeravanje (custom routing) u ASP.NET Core-u omogućava:

✅ Fleksibilnu i semantički jasnu API strukturu
✅ Verzionisanje i održavanje API-ja
✅ Preciznu kontrolu nad URL-ima
✅ RESTful konvencije
✅ Lakšu integraciju za front-end aplikacije

Implementirano je **12 akcija** (6 u BookingsController + 6 u ProjectsController) s različitim strategijama routinga.
