using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace VagrantAtlas
{
    public class ValidateAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            OnValidate(actionContext);
        }

        protected virtual void OnValidate(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid
                && !SkipValidation(actionContext)
                && actionContext.Response == null)
            {
                HandleInvalidModelState(actionContext);
            }
        }

        protected virtual void HandleInvalidModelState(HttpActionContext actionContext)
        {
            actionContext.Response = actionContext.ControllerContext.Request
                .CreateErrorResponse(HttpStatusCode.BadRequest, actionContext.ModelState);
        }

        // adapted from System.Web.Http/AuthorizeAttribute.cs
        private static bool SkipValidation(HttpActionContext actionContext)
        {
            return actionContext.ActionDescriptor.GetCustomAttributes<AllowInvalidModelStateAttribute>().Any()
                   || actionContext.ControllerContext.ControllerDescriptor.GetCustomAttributes<AllowInvalidModelStateAttribute>().Any();
        }
    }
}
