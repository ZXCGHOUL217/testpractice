using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using test_practice.Data;
using test_practice.Models;

namespace test_practice.Pages.Catalog
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public List<Product> Products { get; set; }
        public List<Category> Categories { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? CategoryId { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SearchQuery { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string SortBy {  get; set; } = "name";

        public int CurrentPage { get; set; } = 1;
        public int TotalPages { get; set; }
        public int PageSize { get; set; } = 9;
        public int TotalProducts { get; set; }

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync(int page = 1)
        {
            CurrentPage = page;

            Categories = await _context.Categories.ToListAsync();

            var query = _context.Products
                .Include(p => p.Category)
                .AsQueryable();

            if (CategoryId.HasValue && CategoryId > 0)
            {
                query = query.Where(p => p.CategoryId == CategoryId);
            }
            if (!string.IsNullOrEmpty(SearchQuery))
            {
                query = query.Where(p => 
                    p.Name.Contains(SearchQuery) ||
                    p.Description.Contains(SearchQuery));
            }
            switch (SortBy)
            {
                case "price_asc":
                    query = query.OrderBy(p => p.Price);
                    break;
                case "price_desc":
                    query = query.OrderByDescending(p => p.Price);
                    break;

                default:
                    query = query.OrderBy(p => p.Name);
                    break;
            }

            TotalProducts = await query.CountAsync();
            TotalPages = (int)Math.Ceiling(TotalProducts / (double)PageSize);

            Products = await query
                .Skip((CurrentPage - 1) * PageSize)
                .Take(PageSize)
                .ToListAsync();


        }
    }
}
