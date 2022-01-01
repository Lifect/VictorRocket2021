using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ShopUI.DataAccess;
using ShopUI.Models;
using System.Collections.Generic;
using System.Linq;

namespace ShopUI.Pages
{
    public class IndexModel : PageModel
    {
        public List<Product> _products { get; set; }

        private readonly IDataAccess<Product> _productDataAccess;

        public readonly IDataAccess<Customer> _customerDataAccess;

        public IndexModel(IDataAccess<Product> productDataAccess, IDataAccess<Customer> customerDataAccess)
        {
            this._productDataAccess = productDataAccess;
            this._customerDataAccess = customerDataAccess;
        }

        public void OnGet()
        {
            //Get all products from JSON
            _products = _productDataAccess.GetAll();
            if (_products == null)
            {
                //If there are no products: create a new list to avoid a null exception
                _products = new List<Product>();
            }
        }
        [BindProperty]
        public int _loginId { get; set; }

        public IActionResult OnPostLogin()
        {
            //Set cookie with the logged in users id
            HttpContext.Session.SetInt32("LoginId", _loginId);

            return Page();
        }
        [BindProperty]
        public int _productId { get; set; }

        public IActionResult OnPostAddToCart()
        {
            //Check cookie if customer is logged in
            if (HttpContext.Session.GetInt32("LoginId") != 0 && HttpContext.Session.GetInt32("LoginId") != null)
            {
                //Get customer from cookie
                Customer customer = _customerDataAccess.GetById((int)HttpContext.Session.GetInt32("LoginId"));
                //Add item to customers cart
                customer._shoppingCart.AddItemToCart(_productDataAccess.GetById(_productId));

                //Seralize to JSON file to save changes
                List<Customer> updateCList = _customerDataAccess.GetAll();
                updateCList[customer._id - 1] = customer;
                _customerDataAccess.SerializeItems(updateCList);
            }

            return Page();
        }

        [BindProperty]
        public string _searchTerm { get; set; }
        public IActionResult OnPostSearch()
        {
            if (!string.IsNullOrEmpty(_searchTerm))
            {
                //Search for all items in JSON where title contains the searched word
                _products = _productDataAccess.GetAll().
                    Where(p => p._title.ToLower().
                    Contains(_searchTerm.ToLower())).ToList();
            }
            return Page();
        }


        [BindProperty]
        public string _sortTerm { get; set; }
        public IActionResult OnPostSort()
        {
            //Check so that the sort term is not set to the default value
            if (!string.IsNullOrEmpty(_sortTerm))
            {
                //Get the _products list
                OnGet();
                switch (_sortTerm)
                {
                    case "_hPrice":
                        _products = _products.OrderByDescending(o => o._price).ToList();
                        break;

                    case "_lPrice":
                        _products = _products.OrderBy(o => o._price).ToList();
                        break;

                    case "_AZProducts":
                        _products = _products.OrderBy(o => o._title).ToList();
                        break;

                    case "_ZAProducts":
                        _products = _products.OrderByDescending(o => o._title).ToList();
                        break;
                }
            }

            return Page();
        }


        public void OnPostSerialize()
        {
            _products = new List<Product>();
            _products.Add(new Product(1, "https://www.sveafireworks.se/wp-content/uploads/2021/12/999-Mega-Bag-swe-kampanj.png", "Mega Bang", "Fyrverkeribag med ett flertal större artiklar och sammanlagd krutvikt på över 1,3 kg! Denna familjeförpackning levereras dessutom i denna praktiska bag. Obs! Innehållet i förpackningen kan variera något från bilden.", 149));
            _products.Add(new Product(2, "https://www.sveafireworks.se/wp-content/uploads/2021/12/360P-Golden-Explosion.png", "Gold", "Unna dig något exklusivt. Tårta med 100 guldeffekter. Kaskader med kronor, peony, krysantemum, guldregn och kamuroeffekter. En riktig publikfavorit!", 399));
            _products.Add(new Product(3, "https://www.sveafireworks.se/wp-content/uploads/2021/12/359P-Oden.png", "Oden", "Ett kilo krut! Exklusiv tårta med kaskader av kronor, peony, krysantemum, guldregn och kamuroeffekter, allt i en härlig mix av färger. En glad överraskning med extra knallar på slutfinalen!", 599));
            _products.Add(new Product(4, "https://www.sveafireworks.se/wp-content/uploads/2021/12/287-Atlas.png", "Atlas", "En överdådig show, fullt med färger och fantastiska effekter i pastell. Kraftfulla palmer, kronor och blinkande stjärnor med en final utöver det vanliga.", 799));
            _products.Add(new Product(5, "https://www.sveafireworks.se/wp-content/uploads/2021/12/275-Loke-1.png", "Loke", "121 skott i en riktig Hi-Tech tårta. Denna tårta är mixad och innehåller flera överraskningar.", 2100));
            _products.Add(new Product(6, "https://www.sveafireworks.se/wp-content/uploads/2021/12/280-Valhall.png", "Valhall", "Ett batteri med 76 skott i 3 olika innerdiametrar, en spännande tårta med med många olika effekter. Innehåller mycket effekt i guldnyans.", 459));
            _products.Add(new Product(7, "https://www.sveafireworks.se/wp-content/uploads/2021/12/280-2-Spartacus-kampanj.png", "Spartacus", "76 skott 20, 25 och 30 mm i innerdiameter. En riktigt bra tårta som innehåller mycket färger och effekter", 995));
            _products.Add(new Product(8, "https://www.sveafireworks.se/wp-content/uploads/2021/12/314-Afrodite.png", "Afrodite", "Innehållsrik och kraftig eldbägare. Stor variation och ännu större effekter, vilket gör pjäsen till en riktig upplevelse.", 390));
            _products.Add(new Product(9, "https://www.sveafireworks.se/wp-content/uploads/2021/10/248-Neptunus-840-min.png", "Neptunus", "Stort innehåll i en liten förpackning, Hi-Tech tårta, som innehåller mycket glitter och som håller sig på varierande höjder. Tårtan har flera delfinaler.", 1999));
            _productDataAccess.SerializeItems(_products);



            List<Customer> customer = new List<Customer>();
            customer.Add(new Customer(1, "Lisa Persson", new List<Order>()));
            customer.Add(new Customer(2, "Teppas Fågelberg", new List<Order>()));
            customer.Add(new Customer(3, "Johanna Svensson", new List<Order>()));
            customer.Add(new Customer(4, "Pontus Aschberg", new List<Order>()));
            customer.Add(new Customer(5, "Ebba Larsson", new List<Order>()));
            _customerDataAccess.SerializeItems(customer);
        }
    }
}
