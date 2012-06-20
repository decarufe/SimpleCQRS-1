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
        private const int ItemsPerPage = 6;
        public IInventoryReader InventoryReader { get; set; }

        public ActionResult Index(int page = 1)
        {
            page = Math.Max(page-1, 0);

            ViewBag.Items       = InventoryReader.Items.Take(ItemsPerPage).Skip(page*ItemsPerPage).OrderBy(x => x.ItemId).ToList();
            ViewBag.ItemsCount  = InventoryReader.Items.Count();

            return View();
        }
    }
}
