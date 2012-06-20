using System.Web.Mvc;

namespace ECommDemo.Web.Areas.logistics
{
    public class logisticsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "logistics";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "logistics_default",
                "logistics/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
