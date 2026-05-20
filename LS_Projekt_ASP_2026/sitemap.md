# Semanticki model usmjeravanja (sitemap)

## Razor Pages (UI)
- / -> Pages/Index (PageModel: IndexModel, handler: OnGet) -> view: Pages/Index.cshtml
- /Privacy -> Pages/Privacy (PageModel: PrivacyModel, handler: OnGet) -> view: Pages/Privacy.cshtml
- /Error -> Pages/Error (PageModel: ErrorModel, handler: OnGet) -> view: Pages/Error.cshtml
- /Auth/Login -> Pages/Auth/Login (PageModel: LoginModel, handlers: OnGet, OnPost) -> view: Pages/Auth/Login.cshtml
- /Auth/Register -> Pages/Auth/Register (PageModel: RegisterModel, handlers: OnGet, OnPost) -> view: Pages/Auth/Register.cshtml
- /Bookings -> Pages/Bookings/Index (PageModel: IndexModel, handler: OnGet) -> view: Pages/Bookings/Index.cshtml
- /Bookings/Create -> Pages/Bookings/Create (PageModel: CreateModel, handlers: OnGet, OnPost) -> view: Pages/Bookings/Create.cshtml
- /Projects -> Pages/Projects/Index (PageModel: IndexModel, handler: OnGet) -> view: Pages/Projects/Index.cshtml
- /Producers -> Pages/Producers/Index (PageModel: IndexModel, handler: OnGet) -> view: Pages/Producers/Index.cshtml

## API endpoints (Controllers, bez view-a)

### BookingsController
- /api/bookings/all -> BookingsController.GetAllBookings (HTTP GET) -> JSON
- /api/bookings/details/{id} -> BookingsController.GetBookingDetails (HTTP GET) -> JSON
- /bookings-report/active -> BookingsController.GetActiveBookings (HTTP GET) -> JSON
- /api/v2/bookings/statistics -> BookingsController.GetBookingStatistics (HTTP GET) -> JSON
- /bookings-by-date/{date:datetime} -> BookingsController.GetBookingsByDate (HTTP GET) -> JSON
- /search-bookings?status=... -> BookingsController.SearchBookings (HTTP GET) -> JSON

### ProjectsController (prefix /api/projects)
- /api/projects -> ProjectsController.GetAllProjects (HTTP GET) -> JSON
- /api/projects/{id} -> ProjectsController.GetProjectById (HTTP GET) -> JSON
- /api/projects/by-type/{type} -> ProjectsController.GetProjectsByType (HTTP GET) -> JSON
- /api/projects/admin/report -> ProjectsController.GetProjectReport (HTTP GET) -> JSON
- /api/projects/advanced-search?genre=...&status=...&minBudget=...&maxBudget=... -> ProjectsController.AdvancedSearch (HTTP GET) -> JSON
- /api/projects/client/{clientId} -> ProjectsController.GetProjectsByClient (HTTP GET) -> JSON

## Konvencionalni MVC route
- MapControllerRoute: {controller=Home}/{action=Index}/{id?}
- U projektu trenutno nema HomeController ni drugih MVC kontrolera s view-ovima, pa ovaj route ne mapira dodatne UI stranice.
