using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopUI.DataAccess;
using ShopUI.Models;

namespace ShopUI.Pages
{
    public class CartModel : PageModel
    {

        public readonly IDataAccess<Customer> _customerDataAccess;

        public CartModel(IDataAccess<Customer> customerDataAccess)
        {
            this._customerDataAccess = customerDataAccess;
        }

        public Customer _customer { get; set; }

        public IActionResult OnGet()
        {
            //Check cookie if customer is logged in
            if(HttpContext.Session.GetInt32("LoginId") != 0 && HttpContext.Session.GetInt32("LoginId") != null)
            {
                //Get customer from cookie
                _customer = _customerDataAccess.GetById((int)HttpContext.Session.GetInt32("LoginId"));
                return Page();
            }
            //Else: return them to index
            return RedirectToPage("/Index");
        }


        public IActionResult OnPostOrder()
        {
            //Get customer from cookie
            OnGet();
            //If customer does not have any items in cart: skip everything below
            if(_customer._shoppingCart._products.Count == 0)
            {
                return Page();
            }

            //Ser dumt ut men är tvungen för att det ska fungera
            List<Product> _orderProducts = new List<Product>();
            //Add all customers products into a new list
            _orderProducts.AddRange(_customer._shoppingCart._products);
            //Clear customers cart
            _customer._shoppingCart._products.Clear();
            //Add the new order
            _customer._orders.Add(new Order(_customer._orders.Count, _orderProducts));

            //Seralize to JSON file to save changes
            List<Customer> updateCList = _customerDataAccess.GetAll();
            updateCList[_customer._id - 1] = _customer;
            _customerDataAccess.SerializeItems(updateCList);

            //Return to index
            return RedirectToPage("/Index");
        }





    }
}
