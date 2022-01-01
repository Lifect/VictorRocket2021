using System.Collections.Generic;

namespace ShopUI.Models
{
    public class Order
    {
        public int _id { get; set; }
        public bool _isPaid { get; set; }
        public List<Product> _product { get; set; }

        public Order(int id, List<Product> product)
        {
            _id = id;
            _product = product;
            _isPaid = false;
        }
    }   
}
