namespace MvcMusicStore.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }
        public int OrderId { get; set; }
        public string Sku { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public string Name { get; set; }
        public decimal Discount { get; set; }

        public decimal LineTotal
        {
            get
            {
                return (this.UnitPrice - this.Discount) * this.Quantity;
            }
        }

    }
}
