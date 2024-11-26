using LugxGaming.BusinessLogic.Extensions;
using LugxGaming.BusinessLogic.Models.Cart;
using LugxGaming.BusinessLogic.Models.Payment;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LugxGaming.BusinessLogic.Components
{
    public class SmallCartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.GetJson<List<CartItem>>("Cart");
            SmallCartModel smallCart;

            if (cart == null || cart.Count == 0)
            {
                smallCart = null;
            }
            else
            {
                smallCart = new SmallCartModel
                {
                    NumberOfItems = cart.Sum(c => c.Quantity),
                    TotalAmount = cart.Sum(c => c.Quantity * c.USDPrice)
                };
            }

            return View(smallCart);
        }
    }
}