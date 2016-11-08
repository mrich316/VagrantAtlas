using System;
using System.Linq;
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
            var result = sut.Get(new BoxReference
            {
                User = box.User,
                Name = box.Name
            });

            Assert.IsType<OkNegotiatedContentResult<Box>>(result);
            Assert.Equal(expected, ((OkNegotiatedContentResult<Box>) result).Content);
        }

        [Theory, UnitTestConventions]
        public void Get_ReturnsNotFound_OnMissingBox(BoxesController sut, Box box)
        {
            Assert.IsType<NotFoundResult>(sut.Get(new BoxReference
            {
                User = box.User,
                Name = box.Name
            }));
        }

        [Theory, UnitTestConventions]
        public void Put_ReturnsCreatedAt_OnBoxCreated([Frozen] IBoxRepository repository, BoxesController sut, BoxReferenceVersion reference, BoxProvider provider)
        {
            Assert.Null(repository.Get(reference.User, reference.Name));

            Assert.IsType<CreatedAtRouteNegotiatedContentResult<Box>>(sut.Put(reference, provider));

            var actual = repository.Get(reference.User, reference.Name);
            Assert.NotNull(actual);
            Assert.Equal(reference.User, actual.User);
            Assert.Equal(reference.Name, actual.Name);
            Assert.True(actual.Versions.Any(v => v.Version == reference.Version));
        }

        [Theory, UnitTestConventions]
        public void Put_ReturnsCreatedAt_OnBoxUpdated([Frozen] IBoxRepository repository, BoxesController sut, BoxReferenceVersion reference, BoxProvider provider)
        {
            repository.AddOrUpdate(new Box
            {
                Name = reference.Name,
                User = reference.User
            });

            Assert.IsType<CreatedAtRouteNegotiatedContentResult<Box>>(sut.Put(reference, provider));

            var actual = repository.Get(reference.User, reference.Name);
            Assert.NotNull(actual);
            Assert.Equal(reference.User, actual.User);
            Assert.Equal(reference.Name, actual.Name);
            Assert.True(actual.Versions.Any(v => v.Version == reference.Version));
        }
    }
}
