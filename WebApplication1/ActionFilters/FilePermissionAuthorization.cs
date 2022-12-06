using BusinessLogic.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using System.Linq;

namespace WebApplication1.ActionFilters
{
    public class FilePermissionAuthorization : ActionFilterAttribute
    {
        
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            //to check that the current logged in user has the username ryanattard@gmail.com to allow him in the DeleteItem()

            //HA: you need to check whether the current logged in user has got the rights to access/edit this file

            //this line is needed for the HA !!!!
            //how to get already initialised instances
            ItemsServices myServices = (ItemsServices)context.HttpContext.RequestServices.GetService(typeof(ItemsServices));

            string id = (context.ActionArguments.ElementAt(0).Value).ToString();

            if(context.HttpContext.User.Identity.IsAuthenticated == false)
            {
                
                context.Result = //new UnauthorizedResult();
                    new RedirectToRouteResult(new RouteValueDictionary(new { action = "ErrorMessage", controller = "Home", message = "access denied" }));
            }
            else
            {
                string currentlyLoggedInUsername = context.HttpContext.User.Identity.Name;

                if(currentlyLoggedInUsername != "ryanattard@gmail.com")
                {
                    context.Result = //new UnauthorizedResult();
                        new RedirectToRouteResult(new RouteValueDictionary(new { action = "ErrorMessage", controller = "Home", message = "access denied" }));

                }



            }

        }
    }
}
