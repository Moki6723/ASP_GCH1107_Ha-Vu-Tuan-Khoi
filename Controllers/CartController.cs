using Asmmvc1670.Data;
using Asmmvc1670.Extension;
using Asmmvc1670.Models.ViewModels;
using Asmmvc1670.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace Asmmvc1670.Controllers
{
    public class CartController : Controller
    {
        private readonly Asmmvc1670Context _context;

        public CartController(Asmmvc1670Context context)
        {
            _context = context;
        }
        public IActionResult Index()
        {           
            List<Cart> carts = HttpContext.Session.GetJson<List<Cart>>("Cart") ?? new List<Cart>();
            decimal grandTotal = HttpContext.Session.GetJson<decimal>("GrandTotal");

            CartItem cartVM = new CartItem
            {
                CartItems = carts,
                GrandTotal = grandTotal
            };

            return View(cartVM); 
        }

        public async Task<IActionResult> Add(int Id)
        {
            Product product = await _context.Product.FindAsync(Id);
            List<Cart> cart = HttpContext.Session.GetJson<List<Cart>>("Cart") ?? new List<Cart>();
            Cart carts = cart.Where(x => x.ProductId == Id).FirstOrDefault();

            if (carts == null)
            {
                Cart newCartItem = new Cart(product);
                cart.Add(newCartItem);

                _context.Cart.Add(newCartItem);
            }
            else
            {
                carts.Quantity += 1;

                _context.Cart.Update(carts);
            }
            await _context.SaveChangesAsync();

            HttpContext.Session.SetJson("Cart", cart);
            return Redirect(Request.Headers["Referer"].ToString());
        }

        [HttpPost]
        public IActionResult UpdateAddress(int productId, string newAddress)
        {
            List<Cart> cart = HttpContext.Session.GetJson<List<Cart>>("Cart");

            var cartItem = cart.FirstOrDefault(item => item.ProductId == productId);
            if (cartItem != null)
            {
                cartItem.Address = newAddress;
                HttpContext.Session.SetJson("Cart", cart);
                
                var cartInDatabase = _context.Cart.FirstOrDefault(c => c.ProductId == productId);
                if (cartInDatabase != null)
                {
                    cartInDatabase.Address = newAddress;
                    _context.Cart.Update(cartInDatabase);
                    _context.SaveChanges();
                }
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Decrease(int Id)
        {
            List<Cart> cart = HttpContext.Session.GetJson<List<Cart>>("Cart");

            Cart carts = cart.FirstOrDefault(c => c.ProductId == Id);

            if (carts != null)
            {
                if (carts.Quantity > 1)
                {
                    --carts.Quantity;
                }
                else
                {
                    cart.RemoveAll(p => p.ProductId == Id);
                }

                var cartInDatabase = await _context.Cart.FirstOrDefaultAsync(c => c.ProductId == Id);
                if (cartInDatabase != null)
                {
                    cartInDatabase.Quantity = carts.Quantity;
                    _context.Cart.Update(cartInDatabase);
                    await _context.SaveChangesAsync();
                }

                if (cart.Count == 0)
                {
                    HttpContext.Session.Remove("Cart");
                }
                else
                {
                    HttpContext.Session.SetJson("Cart", cart);
                }

                decimal grandTotal = CalculateGrandTotal(cart);
                HttpContext.Session.SetJson("GrandTotal", grandTotal);
            }

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Increase(int Id)
        {
            List<Cart> cart = HttpContext.Session.GetJson<List<Cart>>("Cart");

            Cart carts = cart.FirstOrDefault(c => c.ProductId == Id);

            if (carts != null)
            {
                carts.Quantity++;

                var cartInDatabase = await _context.Cart.FirstOrDefaultAsync(c => c.ProductId == Id);
                if (cartInDatabase != null)
                {
                    cartInDatabase.Quantity = carts.Quantity;
                    _context.Cart.Update(cartInDatabase);
                    await _context.SaveChangesAsync();
                }

                HttpContext.Session.SetJson("Cart", cart);
                decimal grandTotal = CalculateGrandTotal(cart);
                HttpContext.Session.SetJson("GrandTotal", grandTotal);
            }

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Remove(int Id)
        {
            List<Cart> cart = HttpContext.Session.GetJson<List<Cart>>("Cart");

            Cart cartToRemove = cart.FirstOrDefault(c => c.ProductId == Id);

            if (cartToRemove != null)
            {
                var cartInDatabase = await _context.Cart.FirstOrDefaultAsync(c => c.ProductId == Id);
                if (cartInDatabase != null)
                {
                    _context.Cart.Remove(cartInDatabase);
                    await _context.SaveChangesAsync();
                }

                cart.Remove(cartToRemove);

                if (cart.Count == 0)
                {
                    HttpContext.Session.Remove("Cart");
                }
                else
                {
                    HttpContext.Session.SetJson("Cart", cart);
                }
            }

            return RedirectToAction("Index");
        }

        private decimal CalculateGrandTotal(List<Cart> cart)
        {
            decimal grandTotal = (decimal)cart.Sum(c => c.Quantity * c.Price);
            return grandTotal;
        }
    }
}
