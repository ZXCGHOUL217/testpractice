using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using test_practice.Data;
using test_practice.Models;

namespace WebApplication1.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        //datas
        public List<Product> FeaturedProducts { get; set; } = new();
        public List<Category> Categories { get; set; } = new();

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            Categories = await _context.Categories.ToListAsync();

            FeaturedProducts = await _context.Products
                .Include(p => p.Category)
                .Take(6)
                .ToListAsync();
        }
    }
}
