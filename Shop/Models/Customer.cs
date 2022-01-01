using System.Collections.Generic;

namespace ShopUI.Models
{
    public class Customer
    {
        public int _id { get; set; }
        public string _name { get; set; }       
        public List<Order> _orders { get; set; }
        public ShoppingCart _shoppingCart { get; set; }

        public Customer(int id, string name, List<Order> orders)
        {
            _id = id;
            _name = name;          
            _orders = orders;
            if (_shoppingCart == null)
            {
                this._shoppingCart = new ShoppingCart(id);
            }
            
        }

        public void ClearCart()
        {
            this._shoppingCart._products.Clear();
        }

        public void AddOrder()
        {
            _orders.Add(new Order(_orders.Count, _shoppingCart._products));
            ClearCart();
        }
    }
}
