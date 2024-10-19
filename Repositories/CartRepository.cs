using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Art_Gallery.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IHttpContextAccessor _contextAccessor;

        public CartRepository(ApplicationDbContext dbContext, UserManager<IdentityUser> userManager, IHttpContextAccessor contextAccessor)
        {
            _dbContext = dbContext;
            _userManager = userManager;
            _contextAccessor = contextAccessor;
        }
        public async Task<int> AddItem(int artId, int qty)
        {
            string userId = GetUserId();
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                if (string.IsNullOrEmpty(userId))
                {
                    throw new UnauthorizedAccessException("User is not logged-in");
                }

                var cart = await GetCart(userId);
                if (cart is null)
                {
                    cart = new ShoppingCart { UserId = userId };
                    _dbContext.ShoppingCarts.Add(cart);
                }

                await _dbContext.SaveChangesAsync();

                var cartItem = await _dbContext.CartDetails
                                               .FirstOrDefaultAsync(a => a.ShoppingCartId == cart.ShoppingCartId && a.ArtId == artId);

                if (cartItem is not null)
                {
                    cartItem.Quantity += qty;
                }
                else
                {
                    var art = _dbContext.Arts.Find(artId);
                    cartItem = new CartDetail
                    {
                        ArtId = artId,
                        ShoppingCartId = cart.ShoppingCartId,
                        Quantity = qty,
                        UnitPrice = art.ArtPrice
                    };
                    _dbContext.CartDetails.Add(cartItem);
                }

                await _dbContext.SaveChangesAsync();
                await transaction.CommitAsync();

                var cartItemCount = await GetCartItemCount(userId);
                return cartItemCount;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the item.", ex);
            }

        }

        public async Task<int> RemoveItem(int artId)
        {
            string userId = GetUserId();
            try
            {

                if (string.IsNullOrEmpty(userId))
                {
                    throw new UnauthorizedAccessException("User is not logged-in");
                }
                var cart = await GetCart(userId);

                if (cart is null)
                {
                    throw new InvalidOperationException("Invalid Cart");
                }
                var cartItem = _dbContext.CartDetails.FirstOrDefault(a => a.ShoppingCartId == cart.ShoppingCartId && a.ArtId == artId);

                if (cartItem is null)
                {
                    throw new InvalidOperationException("No items in cart");
                }
                else if (cartItem.Quantity == 1)
                {
                    _dbContext.CartDetails.Remove(cartItem);
                }

                else
                {
                    cartItem.Quantity = cartItem.Quantity - 1;
                }
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            var cartItemCount = await GetCartItemCount(userId); ;
            return cartItemCount;

        }

        public async Task<ShoppingCart> GetUserCart()
        {
            var userId = GetUserId();
            if (userId == null)
            {
                throw new InvalidOperationException("Invalid User ID");
            }
            var shoppingCart = await _dbContext.ShoppingCarts
                .Include(a => a.CartDetails)
                .ThenInclude(a => a.Art)
                .ThenInclude(a => a.Stock)
                .Include(a => a.CartDetails)
                .ThenInclude(a => a.Art)
                .ThenInclude(a => a.Genre)
                .Where(a => a.UserId == userId).FirstOrDefaultAsync();

            return shoppingCart;
        }


        public async Task<ShoppingCart> GetCart(string userId)
        {
            var cart = await _dbContext.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;

        }

        public async Task<int> GetCartItemCount(string userId = "")
        {
            if (string.IsNullOrEmpty(userId))
            {
                userId = GetUserId();
            }

            if (string.IsNullOrEmpty(userId))
            {
                throw new InvalidOperationException("User ID is null or empty");
            }

            var count = await (from cart in _dbContext.ShoppingCarts
                               join cartDetail in _dbContext.CartDetails on cart.ShoppingCartId equals cartDetail.ShoppingCartId
                               where cart.UserId == userId
                               select cartDetail).CountAsync();

            return count;
        }

        public async Task<bool> DoCheckout(CheckoutModel model)
        {
            using var transaction = _dbContext.Database.BeginTransaction();
            try
            {

                var userId = GetUserId();

                if (string.IsNullOrEmpty(userId))
                    throw new UnauthorizedAccessException("User is not logged-in");

                var cart = await GetCart(userId);
                if (cart is null)
                    throw new InvalidOperationException("Invalid cart");
                var cartDetail = _dbContext.CartDetails
                                .Where(a => a.ShoppingCartId == cart.ShoppingCartId).ToList();
                if (cartDetail.Count == 0)
                    throw new InvalidOperationException("Cart is empty");
                var pendingRecord = _dbContext.orderStatuses.FirstOrDefault(s => s.StatusName == "Pending");
                if (pendingRecord is null)
                    throw new InvalidOperationException("Order status does not have Pending status"); 
                var order = new Order
                {
                    UserId = userId,
                    CreateDate = DateTime.UtcNow,
                    Name=model.Name,
                    Email = model.Email,
                    MobileNumber = model.MobileNumber,
                    PaymentMethod = model.PaymentMethod,
                    Address = model.Address,
                    IsPaid = false,
                    OrderStatusId = pendingRecord.StatusId,
                };
                _dbContext.Orders.Add(order);
                _dbContext.SaveChanges();
                foreach (var item in cartDetail)
                {
                    var orderDetail = new OrderDetail
                    {
                        ArtId = item.ArtId,
                        OrderId = order.OrderId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice,
                    };
                    _dbContext.OrderDetails.Add(orderDetail);

                    var stock = await _dbContext.Stocks.FirstOrDefaultAsync(a=>a.ArtId == item.ArtId);
                    if (stock == null)
                    {
                        throw new InvalidOperationException("Stock is null");
                    }
                    if (item.Quantity > stock.Quantity)
                    {
                        throw new InvalidOperationException($"Only {stock.Quantity} item(s) are available in the stock");
                    }
                    stock.Quantity -= item.Quantity;

                }

                // removing cart details
                _dbContext.CartDetails.RemoveRange(cartDetail);
                _dbContext.SaveChanges();
                transaction.Commit();
                return true;



            }
            catch (Exception)
            {
                return false;
            }
        }


        private string GetUserId()
        {
            var principal = _contextAccessor.HttpContext.User;
            if (principal == null)
            {
                throw new Exception("HttpContext or User is null");
            }

            return _userManager.GetUserId(principal);
        }

    }
}
