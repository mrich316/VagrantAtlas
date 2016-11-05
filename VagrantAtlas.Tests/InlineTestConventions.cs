using Ploeh.AutoFixture.Xunit2;

namespace VagrantAtlas.Tests
{
    public class InlineTestConventions : InlineAutoDataAttribute
    {
        public InlineTestConventions(params object[] values) 
            : base(new UnitTestConventions(), values)
        {
        }
    }
}
