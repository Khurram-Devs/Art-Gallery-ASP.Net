namespace Art_Gallery.Repositories
{
    public interface ICartRepository
    {
        Task<int> AddItem(int artId, int qty);
        Task<int> RemoveItem(int artId);
        Task<ShoppingCart> GetUserCart();
        Task<ShoppingCart> GetCart(string userId);
        Task<int> GetCartItemCount(string userId = "");
        Task<bool> DoCheckout(CheckoutModel model);

    }
}
