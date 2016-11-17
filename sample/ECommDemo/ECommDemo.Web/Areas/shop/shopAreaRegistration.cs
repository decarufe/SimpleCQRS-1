using System.Web.Mvc;

namespace ECommDemo.Web.Areas.shop
{
    public class shopAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "shop";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "shop_default",
                "shop/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
