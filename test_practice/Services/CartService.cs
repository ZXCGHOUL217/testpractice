using Microsoft.EntityFrameworkCore;
using test_practice.Data;
using test_practice.Models;

namespace test_practice.Services
{
    public class CartService
    {
        private readonly ApplicationDbContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CartService(ApplicationDbContext context, IHttpContextAccessor httpContextAccessor)
        {
            _context = context;
            _httpContextAccessor = httpContextAccessor;
        }

        private string GetCartId()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.Identity?.Name;

            if (!string.IsNullOrEmpty(userId))
            {
                return userId;
            }
            var sessionId = _httpContextAccessor?.HttpContext?.Session?.Id;
            return sessionId ?? Guid.NewGuid().ToString();
        }

        public async Task<List<CartItem>> GetCartItemsAsync()
        {
            var cartId = GetCartId();

            return await _context.CartItems
                .Include(ci => ci.Product)
                .ThenInclude(p => p.Category)
                .Where(ci => ci.UserId == cartId || ci.SessionId == cartId)
                .ToListAsync();
        }

        public async Task AddToCartAsync(int productId, int quantity = 1)
        {
            var cartId = GetCartId();
            var isUser = _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

            var existingItem = await _context.CartItems
                .FirstOrDefaultAsync(ci =>
                ci.ProductId == productId &&
                (isUser ? ci.UserId == cartId : ci.SessionId == cartId));

            if (existingItem != null)
            {
                existingItem.Quantity = quantity;
            }
            else
            {
                var cardItem = new CartItem
                {
                    ProductId = productId,
                    Quantity = quantity,
                    AddedDate = DateTime.Now
                };
                if (isUser)
                {
                    cardItem.UserId = cartId;
                }
                else
                {
                    cardItem.SessionId = cartId;
                }

                _context.CartItems.Add(cardItem);
            }

            await _context.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(int cartItemId)
        {
            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem != null)
            {
                _context.CartItems.Remove(cartItem);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateQuantityAsync(int cartItemId, int quantity)
        {
            if (quantity < 1) quantity = 1;

            var cartItem = await _context.CartItems.FindAsync(cartItemId);
            if (cartItem != null)
            {
                cartItem.Quantity = quantity;
                await _context.SaveChangesAsync();
            }
        }

        public async Task ClearCartAsync()
        {
            var cartId = GetCartId();
            var isUser = _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

            var items = await _context.CartItems
                .Where(ci => isUser ? ci.UserId == cartId : ci.SessionId == cartId)
                .ToListAsync();

            _context.CartItems.RemoveRange();
            await _context.SaveChangesAsync();
        }

        public async Task<decimal> GetTotalAmountAsync ()
        {
            var items = await GetCartItemsAsync();
            return items.Sum(item => item.Product?.Price * item.Quantity ?? 0);
        }

        public async Task<int> GetCartItemsCount()
        {
            var cartId = GetCartId();
            var isUser = _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;

            return await _context.CartItems
                .Where(ci => isUser ? ci.UserId == cartId : ci.SessionId == cartId)
                .SumAsync(ci => ci.Quantity);
        }


    }
}
