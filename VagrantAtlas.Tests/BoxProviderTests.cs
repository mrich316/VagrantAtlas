using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Xunit;

namespace VagrantAtlas.Tests
{
    public class BoxProviderTests
    {
        [Theory]
        [InlineTestConventions("md5")]
        [InlineTestConventions("sha1")]
        [InlineTestConventions("sha256")]
        public void ChecksumType_IsValid_Algorithms(string checksumType, BoxProvider sut)
        {
            sut.ChecksumType = checksumType;

            var results = new List<ValidationResult>();
            var context = new ValidationContext(sut, null, null)
            {
                MemberName = "ChecksumType"
            };

            Assert.True(Validator.TryValidateProperty(sut.ChecksumType, context, results),
                results.FirstOrDefault()?.ErrorMessage);
        }

        [Theory, UnitTestConventions]
        public void ChecksumType_IsInvalid_Algorithms(string checksumType, BoxProvider sut)
        {
            sut.ChecksumType = checksumType;

            var results = new List<ValidationResult>();
            var context = new ValidationContext(sut, null, null)
            {
                MemberName = "ChecksumType"
            };

            Assert.False(Validator.TryValidateProperty(sut.ChecksumType, context, results));
        }

        [Theory]
        [InlineTestConventions("Name")]
        [InlineTestConventions("Url")]
        [InlineTestConventions("ChecksumType")]
        [InlineTestConventions("Checksum")]
        public void MemberName_IsRequired(string memberName, BoxProvider sut)
        {
            foreach (var value in new List<string> {string.Empty, null})
            {
                var results = new List<ValidationResult>();
                var context = new ValidationContext(sut, null, null)
                {
                    MemberName = memberName
                };

                Assert.False(Validator.TryValidateProperty(value, context, results));
            }
        }
    }
}
