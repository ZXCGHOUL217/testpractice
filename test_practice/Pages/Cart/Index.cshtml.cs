using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using test_practice.Services;

namespace test_practice.Pages.Cart
{
    public class IndexModel : PageModel
    {
        private readonly CartService _cartService;

        public List<Models.CartItem> CartItems { get; set; } = new();
        public decimal TotalAmount { get; set; }

        public IndexModel(CartService cartService)
        {
            _cartService = cartService;
        }

        public async Task OnGetAsync()
        {
            CartItems = await _cartService.GetCartItemsAsync();
            TotalAmount = await _cartService.GetTotalAmountAsync();
        }

        public async Task<IActionResult> OnPostUpdateAsync(int cartItemId, int quantity)
        {
            await _cartService.UpdateQuantityAsync(cartItemId, quantity);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostRemoveAsync(int cartItemId)
        {
            await _cartService.RemoveFromCartAsync(cartItemId);
            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostClearAsync()
        {
            await _cartService.ClearCartAsync();
            return RedirectToPage();
        }
    }
}