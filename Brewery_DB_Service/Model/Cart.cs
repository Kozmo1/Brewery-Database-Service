namespace Brewery_DB_Service.Model
{
    public class Cart
    {
        public int UserId { get; set; }
        public int InventoryId { get; set; }
        public int Quantity { get; set; }
    }
}