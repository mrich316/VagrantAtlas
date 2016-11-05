using Ploeh.AutoFixture;
using Ploeh.AutoFixture.AutoMoq;
using Ploeh.AutoFixture.Xunit2;

namespace VagrantAtlas.Tests
{
    public class UnitTestConventions : AutoDataAttribute
    {
        public UnitTestConventions() : base(new Fixture()
                .Customize(new UnitTestCustomizations())
                .Customize(new AutoMoqCustomization()))
        {
        }
    }
}
