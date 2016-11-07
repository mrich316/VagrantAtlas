using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using System.Web.Http.Controllers;
using Moq;
using Xunit;

namespace VagrantAtlas.Tests
{
    public class ValidateAttributeTests
    {
        [Theory, UnitTestConventions]
        public void OnActionExecuting_Returns_OnValidModelState(ValidateAttribute sut, HttpActionContext actionContext)
        {
            // Test precondition.
            Assert.True(actionContext.ModelState.IsValid);

            var expected = actionContext.Response;
            sut.OnActionExecuting(actionContext);

            // Response should not change.
            Assert.Equal(expected, actionContext.Response);
        }

        [Theory, UnitTestConventions]
        public void OnActionExecuting_ReturnsBadRequest_OnInvalidModelState(ValidateAttribute sut, HttpActionContext actionContext)
        {
            // Test precondition.
            actionContext.ModelState.AddModelError("value", "invalid");
            Assert.False(actionContext.ModelState.IsValid);

            sut.OnActionExecuting(actionContext);

            Assert.NotNull(actionContext.Response);
            Assert.Equal(HttpStatusCode.BadRequest, actionContext.Response.StatusCode);
        }

        [Theory, UnitTestConventions]
        public void OnActionExecuting_SkipBadRequest_OnAllowInvalidStateAttribute(ValidateAttribute sut, HttpActionContext actionContext)
        {
            var mock = Mock.Get(actionContext.ActionDescriptor);
            var customAttributes = new Collection<AllowInvalidModelStateAttribute>(
                new List<AllowInvalidModelStateAttribute>()
                {
                    new AllowInvalidModelStateAttribute()
                });

            mock.Setup(x => x.GetCustomAttributes<AllowInvalidModelStateAttribute>())
                .Returns(customAttributes);

            // Test precondition.
            actionContext.ModelState.AddModelError("value", "invalid");
            Assert.False(actionContext.ModelState.IsValid);

            var expected = actionContext.Response;
            sut.OnActionExecuting(actionContext);

            // Response should not change.
            Assert.Equal(expected, actionContext.Response);
        }
    }
}