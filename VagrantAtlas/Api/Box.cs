using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace VagrantAtlas.Api
{
    public class Box
    {
        private List<BoxVersion> _versions = new List<BoxVersion>();
        private HashSet<string> _tags = new HashSet<string>();

        [Required, StringLength(30), RegularExpression("^[a-zA-Z0-9_]+$")]
        public string User { get; set; }

        [Required, StringLength(60), RegularExpression("^[a-zA-Z0-9_]+$")]
        public string Name { get; set; }

        public string Description { get; set; }

        public List<BoxVersion> Versions
        {
            get { return _versions; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                _versions = value;
            }
        }

        public HashSet<string> Tags
        {
            get { return _tags; }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                _tags = value;
            }
        }
    }
}
