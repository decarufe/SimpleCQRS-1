using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECommDemo.ViewModel.Shop;

namespace ECommDemo.Web.Areas.shop.Controllers
{
    public class CatalogController : Controller
    {
        public ICatalogReader Catalog { get; set; }
        //
        // GET: /shop/CatalogService/

        public ActionResult Index()
        {
            var items = Catalog.Items.ToList();

            return Content(items.Count.ToString());
        }
    }
}
