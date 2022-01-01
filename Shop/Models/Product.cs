namespace ShopUI.Models
{
    public class Product
    {
        public int _id { get; set; }
        public string _image { get; set; }
        public string _title { get; set; }
        public string _description { get; set; }
        public int _price { get; set; }

        public Product(int id, string image, string title, string description, int price)
        {
            _id = id;
            _image = image;
            _title = title;
            _description = description;
            _price = price;
        }
    }
}