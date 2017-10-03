using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using VagrantAtlas.Api;
using Xunit;

namespace VagrantAtlas.UnitTests
{
    public class BoxProviderTests
    {
        [Theory]
        [InlineData("md5")]
        [InlineData("sha1")]
        [InlineData("sha256")]
        public void ChecksumType_IsValid_Algorithms(string checksumType)
        {
            var sut = new BoxProvider();
            sut.ChecksumType = checksumType;

            var results = new List<ValidationResult>();
            var context = new ValidationContext(sut, null, null)
            {
                MemberName = nameof(sut.ChecksumType)
            };

            Assert.True(Validator.TryValidateProperty(sut.ChecksumType, context, results),
                results.FirstOrDefault()?.ErrorMessage);
        }

        [Theory]
        [InlineData("SHA256")]
        [InlineData("random")]
        public void ChecksumType_IsInvalid_Algorithms(string checksumType)
        {
            var sut = new BoxProvider();
            sut.ChecksumType = checksumType;

            var results = new List<ValidationResult>();
            var context = new ValidationContext(sut, null, null)
            {
                MemberName = nameof(sut.ChecksumType)
            };

            Assert.False(Validator.TryValidateProperty(sut.ChecksumType, context, results));
        }

        [Theory]
        [InlineData("garbage")]
        public void Checksum_IsInvalid(string checksum)
        {
            var sut = new BoxProvider();
            sut.Checksum = checksum;

            var results = new List<ValidationResult>();
            var context = new ValidationContext(sut, null, null)
            {
                MemberName = nameof(sut.Checksum)
            };

            Assert.False(Validator.TryValidateProperty(sut.Checksum, context, results));
        }

        [Theory]
        [InlineData("Name")]
        [InlineData("Url")]
        [InlineData("ChecksumType")]
        [InlineData("Checksum")]
        public void MemberName_IsRequired(string memberName)
        {
            foreach (var value in new List<string> { string.Empty, null })
            {
                var results = new List<ValidationResult>();
                var context = new ValidationContext(new BoxProvider(), null, null)
                {
                    MemberName = memberName
                };

                Assert.False(Validator.TryValidateProperty(value, context, results));
            }
        }

        [Theory]
        [InlineData("garbage/")]
        public void Url_IsInvalid_OnLengthGreaterThan4096(string url)
        {
            while (url.Length < 4096)
            {
                url += url;
            }

            var sut = new BoxProvider();
            sut.Url = "http://localhost.localtest.me/" + url;

            var results = new List<ValidationResult>();
            var context = new ValidationContext(sut, null, null)
            {
                MemberName = nameof(sut.Url)
            };

            Assert.False(Validator.TryValidateProperty(sut.Url, context, results));
        }

        [Theory]
        [InlineData("garbage/")]
        public void Url_IsValid_OnUrlFQDN(string url)
        {
            var sut = new BoxProvider();
            sut.Url = "http://localhost.localtest.me/" + url;

            var results = new List<ValidationResult>();
            var context = new ValidationContext(sut, null, null)
            {
                MemberName = nameof(sut.Url)
            };

            Assert.True(Validator.TryValidateProperty(sut.Url, context, results));
        }
    }
}
