namespace aspnetcore_backgroundservice.Data
{
    public class StoreItem
    {
        public StoreItem(int storeItemId, string name, int stock, float price)
        {
            StoreItemId = storeItemId;
            Name = name;
            Stock = stock;
            Price = price;
        }

        public int StoreItemId { get; set; }
        public string Name { get; set; }
        public int Stock { get; set; }
        public float Price { get; set; }
    }
}