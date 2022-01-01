using System.Collections.Generic;

namespace ShopUI.Models
{
    public class ShoppingCart
    {
        public int _id { get; set; }
        public List<Product> _products { get; set; }

        public ShoppingCart(int id)
        {
            _id = id;
            if (_products == null)
            {
                this._products = new List<Product>();
            }
            
        }
        public void AddItemToCart(Product item)
        {
            _products.Add(item);
        }
    }
}
