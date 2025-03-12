namespace WpfApp20
{
    //Класс продуктов для их записи в список
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public DateTime LastUpdated { get; set; }

        public Product()
        {
            LastUpdated = DateTime.Now;
        }

        public override string ToString()
        {
            return $"{Name} - {Quantity} шт.";
        }
    }
}