using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ECommDemo.ViewModel.Inventory;

namespace ECommDemo.Web.Areas.logistics.Controllers
{
    public class InventoryController : Controller
    {
        public IInventoryReader InventoryReader { get; set; }

        public ActionResult Index()
        {
            var items = InventoryReader.Items.Count();
            return Content("Inventory has " + items + " items");
        }
    }
}
