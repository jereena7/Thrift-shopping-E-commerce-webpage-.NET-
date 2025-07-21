using Microsoft.AspNetCore.Mvc;
using ThriftShop.DataAccessLayer;
using ThriftShop.Models;

namespace ThriftShop.Controllers
{
    public class ShoppingController : Controller
    {
        //public IActionResult Index()
        //{
        //    return View();
        //}

        private readonly ProductDAL _productDAL;
        private readonly CartDAL _cartDAL;
        private readonly PurchaseDAL _purchaseDAL;
        public ShoppingController(PurchaseDAL purchaseDAL,ProductDAL productDAL , CartDAL cartDAL)
        {
            _productDAL = productDAL;
            _cartDAL = cartDAL;
            _purchaseDAL = purchaseDAL;
        }
        //[HttpPost]
        //public IActionResult ProcessPayment(string paymentMethod)
        //{
        //    try
        //    {
        //        int userId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
        //        string username = HttpContext.Session.GetString("Username");

        //        if (string.IsNullOrEmpty(paymentMethod))
        //        {
        //            TempData["ErrorMessage"] = "Please select a payment method.";
        //            return RedirectToAction("PaymentPage");
        //        }

        //        var cartItems = _cartDAL.GetCartItems(userId);

        //        foreach (var item in cartItems)
        //        {
        //            var purchase = new Purchase
        //            {
        //                Username = username,
        //                ProductName = item.ProductName,
        //                Price = item.Price,
        //                Quantity = item.Quantity,
        //                TotalPrice = item.TotalPrice,
        //                PurchaseDate = DateTime.Now
        //            };

        //            _purchaseDAL.AddPurchase(purchase);
        //        }

        //        _cartDAL.ClearCart(userId);

        //        ViewBag.EstimatedDeliveryDate = DateTime.Now.AddDays(7).ToString("MMMM dd, yyyy");
        //        return View("OrderConfirmation");
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = "An error occurred while processing your payment.";
        //        return RedirectToAction("PaymentPage");
        //    }
        //}
        //[HttpPost]
        //public IActionResult ProcessPayment(string paymentMethod)
        //{
        //    if (string.IsNullOrEmpty(paymentMethod))
        //        return RedirectToAction("PaymentPage");

        //    return RedirectToAction(paymentMethod);
        //}
        [HttpPost]
        public IActionResult ProcessPayment(string paymentMethod)
        {
            if (string.IsNullOrEmpty(paymentMethod))
            {
                TempData["ErrorMessage"] = "Please select a payment method.";
                return RedirectToAction("PaymentPage");
            }

            // Save selected payment method in TempData or Session
            TempData["SelectedPaymentMethod"] = paymentMethod;

            return RedirectToAction(paymentMethod); // e.g., RedirectToAction("UPI"), etc.
        }
        public IActionResult FinalizeOrder()
        {
            try
            {
                int userId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
                string username = HttpContext.Session.GetString("Username");

                var cartItems = _cartDAL.GetCartItems(userId);

                foreach (var item in cartItems)
                {
                    var purchase = new Purchase
                    {
                        Username = username,
                        ProductName = item.ProductName,
                        Price = item.Price,
                        Quantity = item.Quantity,
                        TotalPrice = item.TotalPrice,
                        PurchaseDate = DateTime.Now
                    };

                    _purchaseDAL.AddPurchase(purchase);
                }

                _cartDAL.ClearCart(userId);

                ViewBag.EstimatedDeliveryDate = DateTime.Now.AddDays(7).ToString("MMMM dd, yyyy");
                return View("OrderConfirmation");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while processing your order.";
                return RedirectToAction("PaymentPage");
            }
        }


        public IActionResult PaymentPage()
        {
            return View();
        }

        public IActionResult BankTransfer()
        {
            return View();
        }

        public IActionResult UPI()
        {
            return View();
        }

        public IActionResult CreditCard()
        {
            return View();
        }

        public IActionResult DebitCard()
        {
            return View();
        }

        public IActionResult NetBanking()
        {
            return View();
        }

        //[HttpPost]
        //public IActionResult ConfirmPayment(string method, IFormCollection form)
        //{
        //    // You can capture and process payment here
        //    ViewBag.Method = method;
        //    return View("PaymentSuccess");
        //}
        [HttpPost]
        public IActionResult ConfirmPayment(string method, IFormCollection form)
        {
            // Optional: Save payment confirmation info if needed
            TempData["PaymentMethodUsed"] = method;

            // After confirming payment, finalize the order
            return RedirectToAction("FinalizeOrder");
        }

        public IActionResult ViewPurchases()
        {
            try
            {
                string username = HttpContext.Session.GetString("Username");
                var purchases = _purchaseDAL.GetPurchasesByUsername(username);
                return View(purchases);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while retrieving your purchases.";
                return View(new List<Purchase>());
            }
        }

        public IActionResult AdminViewPurchases()
        {
            try
            {
                // Optional: Verify if the user is actually an admin
                string role = HttpContext.Session.GetString("UserRole");
                if (role != "Admin")
                {
                    return RedirectToAction("Index", "Home");
                }

                var allPurchases = _purchaseDAL.GetAllPurchases();
                return View(allPurchases);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while retrieving all purchases.";
                return View(new List<Purchase>());
            }
        }

        // GET: Product Listing with Filters
        public IActionResult ProductListing(string searchTerm, string sortBy, string sizeFilter, string conditionFilter,string categoryfilter)
        {
            try
            {
                // Fetch filtered products
                var products = _productDAL.GetFilteredProducts(searchTerm, sortBy, sizeFilter, conditionFilter,categoryfilter);

                // Pass filters to the view for retaining values
                ViewBag.SearchTerm = searchTerm;
                ViewBag.SortBy = sortBy;
                ViewBag.SizeFilter = sizeFilter;
                ViewBag.ConditionFilter = conditionFilter;
                ViewBag.categoryfilter = categoryfilter;
                return View(products);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while retrieving products.";
                return View(new List<ProductModel>());
            }
        }


        //public IActionResult ProductListing()
        //{
        //    var products = _productDAL.GetAllProducts();
        //    return View(products);
        //}

        //public IActionResult Index()
        //{

        //    try
        //    {
        //        var products = _productDAL.GetAllProducts();
        //        return View(products);
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.ErrorMessage = "An error occurred while retrieving products.";
        //        return View(new List<ProductModel>());
        //    }
        //}

        // GET: Product Details Page
        public IActionResult ProductDetails(int id)
        {
            try
            {
                var product = _productDAL.GetProductById(id);
                if (product == null)
                {
                    ViewBag.ErrorMessage = "Product not found.";
                    return RedirectToAction("Index");
                }
                return View(product);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while retrieving the product.";
                return RedirectToAction("Index");
            }
        }

       

        // POST: Add to Cart
        [HttpPost]
        public IActionResult AddToCart(int productId, int quantity)
        {
            try
            {
                // Get the current user's ID from session
                int userId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));

                // Add the product to the cart
                _cartDAL.AddToCart(userId, productId, quantity);

                TempData["SuccessMessage"] = "Product added to cart successfully!";
                return RedirectToAction("ProductDetails", new { id = productId });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while adding the product to the cart.";
                return RedirectToAction("ProductDetails", new { id = productId });
            }
        }

        // GET: View Cart
        public IActionResult ViewCart()
        {
            try
            {
                // Get the current user's ID from session
                int userId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));

                var cartItems = _cartDAL.GetCartItems(userId);
                decimal totalAmount = cartItems.Sum(item => item.TotalPrice);

                ViewBag.TotalAmount = totalAmount;
                return View(cartItems);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while retrieving the cart.";
                return View(new List<CartItemModel>());
            }
        }

        // POST: Remove from Cart
        [HttpPost]
        public IActionResult RemoveFromCart(int cartId)
        {
            try
            {
                // Remove the item from the cart
                _cartDAL.RemoveFromCart(cartId);

                TempData["SuccessMessage"] = "Item removed from cart successfully!";
                return RedirectToAction("ViewCart");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while removing the item from the cart.";
                return RedirectToAction("ViewCart");
            }
        }

        // GET: Buying Page
        public IActionResult BuyingPage()
        {
            return View();
        }


        //// POST: Place Order
        //[HttpPost]
        //public IActionResult PlaceOrder(string address, string phoneNumber)
        //{
        //    try
        //    {
        //        // Get the current user's ID from session
        //        int userId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));

        //        // Logic to process the order (e.g., save order details to the database)
        //        // For now, just clear the cart as an example
        //        _cartDAL.ClearCart(userId);

        //        TempData["SuccessMessage"] = "Order placed successfully!";
        //        return RedirectToAction("Index");
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = "An error occurred while placing the order.";
        //        return View("BuyingPage");
        //    }
        //}

        // POST: Place Order
        [HttpPost]
        public IActionResult PlaceOrder(string address, string phoneNumber)
        {
            try
            {
                // Validate input
                if (string.IsNullOrEmpty(address) || string.IsNullOrEmpty(phoneNumber))
                {
                    TempData["ErrorMessage"] = "Please provide your address and phone number.";
                    return View("BuyingPage");
                }

                // Save order details to the database (optional)
                // For now, just redirect to the payment page
                return RedirectToAction("PaymentPage");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while placing the order.";
                return View("BuyingPage");
            }
        }

        // GET: Payment Page
      

        // POST: Process Payment
        //[HttpPost]
        //public IActionResult ProcessPayment(string paymentMethod)
        //{
        //    try
        //    {
        //        int userId = Convert.ToInt32(HttpContext.Session.GetString("UserId"));
        //        // Simulate payment processing (e.g., validate payment method)
        //        if (string.IsNullOrEmpty(paymentMethod))
        //        {
        //            TempData["ErrorMessage"] = "Please select a payment method.";
        //            return RedirectToAction("PaymentPage");
        //        }

        //        // Simulate successful payment
        //        // Generate an estimated delivery date (7 days from today)
        //        DateTime estimatedDeliveryDate = DateTime.Now.AddDays(7);

        //        // Redirect to the confirmation page with the delivery date
        //        ViewBag.EstimatedDeliveryDate = estimatedDeliveryDate.ToString("MMMM dd, yyyy");
        //        _cartDAL.ClearCart(userId);
        //        return View("OrderConfirmation");
        //    }
        //    catch (Exception ex)
        //    {
        //        TempData["ErrorMessage"] = "An error occurred while processing your payment.";
        //        return RedirectToAction("PaymentPage");
        //    }
        //}
        public IActionResult SlimFitJeans()
        {
            return View();
        }

        public IActionResult LeatherJacket()
        {
            return View();
        }

        public IActionResult SummerDress()
        {
            return View();
        }
  
        
            public IActionResult MensFashion()
            {
                return View();
            }

            public IActionResult WomensFashion()
            {
                return View();
            }

            public IActionResult KidsFashion()
            {
                return View();
            }
        

        // GET: User Dashboard (Product Listing with Filters)
        public IActionResult Index(string searchTerm, string sortBy, string sizeFilter, string conditionFilter,string categoryfilter)
        {
            try
            {
                // Fetch filtered products
                var products = _productDAL.GetFilteredProducts(searchTerm, sortBy, sizeFilter, conditionFilter,categoryfilter);

                // Pass filters to the view for retaining values
                ViewBag.SearchTerm = searchTerm;
                ViewBag.SortBy = sortBy;
                ViewBag.SizeFilter = sizeFilter;
                ViewBag.ConditionFilter = conditionFilter;
                ViewBag.categoryilter = categoryfilter;
                return View(products);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while retrieving products.";
                return View(new List<ProductModel>());
            }
        }

    }
}
