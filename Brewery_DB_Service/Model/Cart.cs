namespace Brewery_DB_Service.Model
{
    public class Cart
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int InventoryId { get; set; }
        public int Quantity { get; set; }
    }

    public class CartRequest
    {
        public int UserId { get; set; }
        public int InventoryId { get; set; }
        public int Quantity { get; set; }
    }
}