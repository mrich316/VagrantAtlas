using System;
using System.Web.Http.Results;
using Ploeh.AutoFixture.Xunit2;
using Xunit;

namespace VagrantAtlas.Tests
{
    public class BoxesControllerTests
    {
        [Fact]
        public void Ctor_Throws_OnNullBoxRepository()
        {
            Assert.Throws<ArgumentNullException>("boxRepository", () => new BoxesController(null));
        }

        [Theory, UnitTestConventions]
        public void Get_ReturnsOk_OnExistingBox([Frozen] IBoxRepository repository, BoxesController sut, Box box)
        {
            var expected = repository.AddOrUpdate(box);
            var result = sut.Get(box.User, box.Name);

            Assert.IsType<OkNegotiatedContentResult<Box>>(result);
            Assert.Equal(expected, ((OkNegotiatedContentResult<Box>) result).Content);
        }

        [Theory, UnitTestConventions]
        public void Get_ReturnsNotFound_OnMissingBox(BoxesController sut, Box box)
        {
            Assert.IsType<NotFoundResult>(sut.Get(box.User, box.Name));
        }
    }
}
