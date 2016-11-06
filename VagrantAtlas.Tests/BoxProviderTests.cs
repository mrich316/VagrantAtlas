using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.Security.Cryptography;
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
                MemberName = nameof(sut.ChecksumType)
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
                MemberName = nameof(sut.ChecksumType)
            };

            Assert.False(Validator.TryValidateProperty(sut.ChecksumType, context, results));
        }

        [Theory, UnitTestConventions]
        public void Checksum_IsValid(byte[] hashSource, BoxProvider sut)
        {
            var hash = SHA256.Create().ComputeHash(hashSource);
            sut.Checksum = new SoapHexBinary(hash).ToString();

            var results = new List<ValidationResult>();
            var context = new ValidationContext(sut, null, null)
            {
                MemberName = nameof(sut.Checksum)
            };

            Assert.True(Validator.TryValidateProperty(sut.Checksum, context, results));
        }

        [Theory, UnitTestConventions]
        public void Checksum_IsInvalid(string checksum, BoxProvider sut)
        {
            sut.Checksum = checksum;

            var results = new List<ValidationResult>();
            var context = new ValidationContext(sut, null, null)
            {
                MemberName = nameof(sut.Checksum)
            };

            Assert.False(Validator.TryValidateProperty(sut.Checksum, context, results));
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

        [Theory, UnitTestConventions]
        public void Url_IsInvalid_OnLengthGreaterThan4096(string url, BoxProvider sut)
        {
            while (url.Length < 4096)
            {
                url += url;
            }

            sut.Url = "http://localhost.localtest.me/" + url;

            var results = new List<ValidationResult>();
            var context = new ValidationContext(sut, null, null)
            {
                MemberName = nameof(sut.Url)
            };

            Assert.False(Validator.TryValidateProperty(sut.Url, context, results));
        }

        [Theory, UnitTestConventions]
        public void Url_IsInvalid_OnUrlNotFQDN(string url, BoxProvider sut)
        {
            sut.Url = "http://localhost/" + url;

            var results = new List<ValidationResult>();
            var context = new ValidationContext(sut, null, null)
            {
                MemberName = nameof(sut.Url)
            };

            Assert.False(Validator.TryValidateProperty(sut.Url, context, results));
        }

        [Theory, UnitTestConventions]
        public void Url_IsValid_OnUrlFQDN(string url, BoxProvider sut)
        {
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
