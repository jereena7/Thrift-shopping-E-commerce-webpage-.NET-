using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using ThriftShop.DataAccessLayer;
using ThriftShop.Models;

namespace ThriftShop.Controllers
{
    [Authorize(Roles = "Admin")] // Ensure only admins can access this controller
    public class AdminController : Controller
    {
        private readonly ProductDAL _productDAL;
        private readonly string _imagePath;
        private readonly UserDAL _userDAL;
        public AdminController(IConfiguration configuration,UserDAL userDAL)
        {
            _productDAL = new ProductDAL(configuration);
            _imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "products");
            _userDAL = userDAL;
        }
        public IActionResult ViewOrders()
        {
            try
            {
                var orders = _productDAL.GetAllOrders(); // Uses the method added in ProductDAL.cs
                return View(orders);
            }
            catch
            {
                ViewBag.ErrorMessage = "An error occurred while retrieving orders.";
                return View(new List<OrderModel>());
            }
        }
        public IActionResult ManageUsers()
        {
            var users = _userDAL.GetAllUsers();
            return View(users);
        }

        [HttpGet]
        public IActionResult EditUser(int id)
        {
            var user = _userDAL.GetUserById(id);
            if (user == null) return RedirectToAction("ManageUsers");
            return View(user);
        }

        [HttpPost]
        public IActionResult EditUser(User user)
        {
            if (ModelState.IsValid)
            {
                _userDAL.UpdateUser(user);
                return RedirectToAction("ManageUsers");
            }
            return View(user);
        }

        [HttpGet]
        public IActionResult DeleteUser(int id)
        {
            var user = _userDAL.GetUserById(id);
            if (user == null) return RedirectToAction("ManageUsers");
            return View(user);
        }

        [HttpPost, ActionName("DeleteUser")]
        public IActionResult DeleteUserConfirmed(int id)
        {
            _userDAL.DeleteUser(id);
            TempData["DeleteSuccess"] = "User deleted successfully.";
            return RedirectToAction("ManageUsers");
        }

        // GET: Admin Dashboard
        public IActionResult Index()
        {
            return View();
        }

        // GET: Add Product Page
        [HttpGet]
        public IActionResult AddProduct()
        {
            return View();
        }

        // POST: Add Product
        [HttpPost]
        public IActionResult AddProduct(ProductModel product, IFormFile[] images)
        {
            try
            {
                // Validate input
                if (string.IsNullOrEmpty(product.ProductName) || product.Price <= 0 || images == null || images.Length == 0)
                {
                    ViewBag.ErrorMessage = "Please fill in all required fields and upload at least one image.";
                    return View();
                }

                // Save images and generate paths
                var imagePaths = new List<string>();
                foreach (var image in images)
                {
                    if (image != null && image.Length > 0)
                    {
                        // Generate a unique file name
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                        var filePath = Path.Combine(_imagePath, fileName);

                        // Save the file to the server
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            image.CopyTo(stream);
                        }

                        // Store the relative path
                        imagePaths.Add("/images/products/" + fileName);
                    }
                }

                // Combine image paths into a comma-separated string
                product.ImagePaths = string.Join(",", imagePaths);

                // Insert product into the database
                _productDAL.InsertProduct(product);

                ViewBag.SuccessMessage = "Product added successfully!";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while adding the product.";
                return View();
            }
        }

        // GET: Manage Products Page
        public IActionResult ManageProducts()
        {
            try
            {
                var products = _productDAL.GetAllProducts();
                return View(products);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while retrieving products.";
                return View(new List<ProductModel>());
            }
        }

        // GET: Edit Product Page
        [HttpGet]
        public IActionResult EditProduct(int id)
        {
            try
            {
                var product = _productDAL.GetProductById(id);
                if (product == null)
                {
                    ViewBag.ErrorMessage = "Product not found.";
                    return RedirectToAction("ManageProducts");
                }
                return View(product);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while retrieving the product.";
                return RedirectToAction("ManageProducts");
            }
        }

        // POST: Edit Product
        [HttpPost]
        public IActionResult EditProduct(ProductModel product, IFormFile[] images)
        {
            try
            {
                // Save new images if provided
                var imagePaths = new List<string>();
                if (images != null && images.Length > 0)
                {
                    foreach (var image in images)
                    {
                        if (image != null && image.Length > 0)
                        {
                            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                            var filePath = Path.Combine(_imagePath, fileName);

                            using (var stream = new FileStream(filePath, FileMode.Create))
                            {
                                image.CopyTo(stream);
                            }

                            imagePaths.Add("/images/products/" + fileName);
                        }
                    }

                    // Combine new image paths with existing ones
                    if (!string.IsNullOrEmpty(product.ImagePaths))
                    {
                        imagePaths.AddRange(product.ImagePaths.Split(','));
                    }

                    product.ImagePaths = string.Join(",", imagePaths);
                }

                // Update product in the database
                _productDAL.UpdateProduct(product);

                ViewBag.SuccessMessage = "Product updated successfully!";
                return View(product);
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while updating the product.";
                return View(product);
            }
        }
        [HttpPost]
        [Authorize]
        public IActionResult PlaceOrder(string address, string phoneNumber)
        {
            try
            {
                int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier).Value);

                var cartItems = _productDAL.GetCartItemsByUserId(userId);
                if (cartItems == null || !cartItems.Any())
                {
                    TempData["ErrorMessage"] = "Your cart is empty.";
                    return RedirectToAction("Cart");
                }

                decimal total = cartItems.Sum(x => x.Price * x.Quantity);

                // Insert into Orders table
                var order = new OrderModel
                {
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    Status = "Pending",
                    TotalAmount = total
                };
                int orderId = _productDAL.InsertOrder(order);

                // Insert each item into OrderItems
                foreach (var item in cartItems)
                {
                    int productId = _productDAL.GetProductIdByName(item.ProductName); // Replace with correct lookup
                    var orderItem = new OrderItemModel
                    {
                        OrderId = orderId,
                        ProductId = productId,
                        Price = item.Price,
                        Quantity = item.Quantity
                    };
                    _productDAL.InsertOrderItem(orderItem);
                }

                // Clear cart
                _productDAL.ClearCart(userId);

                TempData["SuccessMessage"] = "Order placed successfully!";
                return RedirectToAction("Cart");
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Something went wrong while placing the order.";
                return RedirectToAction("Cart");
            }
        }

        // POST: Delete Product
        [HttpPost]
        public IActionResult DeleteProduct(int id)
        {
            try
            {
                _productDAL.DeleteProduct(id);
                return RedirectToAction("ManageProducts");
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "An error occurred while deleting the product.";                  
                return RedirectToAction("ManageProducts");


            }
        }





       
    }
}