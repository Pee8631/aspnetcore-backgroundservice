namespace aspnetcore_backgroundservice.Data
{
    public class Store
    {
        public Store(int storeId)
        {
            StoreId = storeId;
            StoreWorth = 0;
        }

        public int StoreId { get; set; }
        public float StoreWorth { get; set; }

    }
}