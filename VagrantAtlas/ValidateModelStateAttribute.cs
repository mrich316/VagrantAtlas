using System.Linq;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace VagrantAtlas
{
    public class ValidateModelStateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            OnValidate(actionContext);
        }

        protected virtual void OnValidate(ActionExecutingContext actionContext)
        {
            if (!actionContext.ModelState.IsValid
                && !SkipValidation(actionContext)
                && actionContext.Result == null)
                HandleInvalidModelState(actionContext);
        }

        protected virtual void HandleInvalidModelState(ActionExecutingContext actionContext)
        {
            actionContext.Result = new BadRequestObjectResult(actionContext.ModelState);
        }

        // adapted from System.Web.Http/AuthorizeAttribute.cs
        private static bool SkipValidation(ActionExecutingContext actionContext)
        {
            return actionContext.ActionDescriptor.FilterDescriptors.Any(f =>
                f.GetType() == typeof(AllowInvalidModelStateAttribute));
        }
    }
}