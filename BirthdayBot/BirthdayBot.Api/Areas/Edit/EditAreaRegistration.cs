using System.Web.Mvc;

namespace BirthdayBot.Api.Areas.Edit
{
    public class EditAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "Edit";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "Edit_default",
                "Edit/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}