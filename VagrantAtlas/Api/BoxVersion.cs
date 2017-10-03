using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VagrantAtlas.Api
{
    public class BoxVersion
    {
        private List<BoxProvider> _providers = new List<BoxProvider>();

        [Required, RegularExpression(@"^(0|[1-9]\d*)\.(0|[1-9]\d*)\.(0|[1-9]\d*)$")]
        public string Version { get; set; }

        [Required]
        public List<BoxProvider> Providers
        {
            get { return _providers; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                _providers = value;
            }
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((BoxVersion)obj);
        }

        public override int GetHashCode()
        {
            return (Version != null ? Version.GetHashCode() : 0);
        }

        public static bool operator ==(BoxVersion left, BoxVersion right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(BoxVersion left, BoxVersion right)
        {
            return !Equals(left, right);
        }

        protected bool Equals(BoxVersion other)
        {
            return string.Equals(Version, other.Version);
        }
    }
}
