---
name: lista
user-invocable: true
description: "Use when: making a list page (Razor Pages) that shows a table of entities, with simple empty-state handling. Keywords: list page, index page, pregled." 
---

# List Page Skill (Razor Pages)

## Goal
Create a new Razor Pages list page that renders a table of entities from the repository.

## Inputs
- Entity name (plural and singular)
- Repository method to fetch data
- Target route (folder under Pages)
- Columns to show

## Steps
1. Create `Pages/<EntityPlural>/Index.cshtml.cs` with a PageModel that loads the list from `IRepository` in `OnGet`.
2. Create `Pages/<EntityPlural>/Index.cshtml` with a table and an empty state.
3. Add a link to the page from UI if needed (optional).
4. Update `sitemap.md` with the new route (optional but recommended).

## Example (Producer list)

### PageModel
```csharp
using Microsoft.AspNetCore.Mvc.RazorPages;
using LS_Projekt_ASP_2026.Data;
using AudioProductionManagement.Model;
using System.Collections.Generic;

namespace LS_Projekt_ASP_2026.Pages.Producers
{
    public class IndexModel : PageModel
    {
        private readonly IRepository _repository;

        public List<Producer> Producers { get; set; } = new();

        public IndexModel(IRepository repository)
        {
            _repository = repository;
        }

        public void OnGet()
        {
            Producers = new List<Producer>(_repository.GetAllProducers());
        }
    }
}
```

### View
```cshtml
@page
@model LS_Projekt_ASP_2026.Pages.Producers.IndexModel
@{
    ViewData["Title"] = "Producenti";
}

<div class="container-fluid mt-4">
    <div class="row mb-4">
        <div class="col-md-8">
            <h1 class="display-6 fw-bold">Producenti</h1>
            <p class="text-muted">Lista svih producenata</p>
        </div>
    </div>

    @if (Model.Producers.Count == 0)
    {
        <div class="text-center py-5">
            <div class="display-1 mb-3">🎛️</div>
            <h3>Nema producenata</h3>
            <p class="text-muted">Trenutno nema zapisa u bazi.</p>
        </div>
    }
    else
    {
        <div class="table-responsive">
            <table class="table table-hover" style="background: rgba(31, 41, 55, 0.8); color: #F3F4F6;">
                <thead style="background: linear-gradient(135deg, #7C3AED, #6366F1);">
                    <tr>
                        <th style="color: white;">Ime</th>
                        <th style="color: white;">Email</th>
                        <th style="color: white;">Specijalizacija</th>
                        <th style="color: white;">Cijena/h</th>
                    </tr>
                </thead>
                <tbody>
                    @foreach (var producer in Model.Producers)
                    {
                        <tr>
                            <td>@producer.Name @producer.Surname</td>
                            <td>@producer.Email</td>
                            <td>@producer.Specialization</td>
                            <td>@producer.HourlyRate.ToString("C")</td>
                        </tr>
                    }
                </tbody>
            </table>
        </div>
    }
</div>
```
