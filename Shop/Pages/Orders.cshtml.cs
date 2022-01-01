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
    public class OrdersModel : PageModel
    {
        private readonly IDataAccess<Product> _productDataAccess;

        public readonly IDataAccess<Customer> _customerDataAccess;

        public OrdersModel(IDataAccess<Product> productDataAccess, IDataAccess<Customer> customerDataAccess)
        {
            this._productDataAccess = productDataAccess;
            this._customerDataAccess = customerDataAccess;
        }

        public List<Order> _allOrders { get; set; } = new List<Order>();


        public Customer _customer { get; set; }

        public IActionResult OnGet()
        {
            //Check cookie if customer is logged in
            if (HttpContext.Session.GetInt32("LoginId") != 0 && HttpContext.Session.GetInt32("LoginId") != null)
            {
                //Get customer from cookie
                _customer = _customerDataAccess.GetById((int)HttpContext.Session.GetInt32("LoginId"));
                //Add all orders from customer into _allOrders
                _allOrders.AddRange(_customer._orders);

                return Page();
            }
            return RedirectToPage("/Index");
        }

        [BindProperty]
        public int _orderId { get; set; }
        public IActionResult OnPostPay()
        {
            OnGet();
            foreach (Order order in _customer._orders)
            {
                //Find correct order to pay for
                if (order._id == _orderId)
                {
                    order._isPaid = true;

                    //Seralize to JSON file to save changes
                    List<Customer> updateCList = _customerDataAccess.GetAll();
                    updateCList[_customer._id - 1] = _customer;
                    _customerDataAccess.SerializeItems(updateCList);

                    return Page();
                }
            }
            return Page();
        }

        [BindProperty]
        public string _sortTerm { get; set; }
        public IActionResult OnPostSort()
        {
            OnGet();
            switch (_sortTerm)
            {
                case "_paid":
                    //Sort by isPaid
                    _allOrders = _allOrders.OrderByDescending(o => o._isPaid).ToList();
                    break;

                case "_notPaid":
                    _allOrders = _allOrders.OrderBy(o => o._isPaid).ToList();
                    break;
                default:
                    _allOrders = _allOrders.OrderBy(o => o._id).ToList();
                    break;
            }

            return Page();
        }


    }
}
