using System;
using System.ComponentModel.DataAnnotations;

namespace VagrantAtlas
{
    public class BoxProvider
    {
        [Required, StringLength(50)]
        public string Name { get; set; }

        [Required, Url]
        public string Url { get; set; }

        [Required]
        public string ChecksumType { get; set; }

        [Required, RegularExpression("^[a-fA-F0-9]+$")]
        public string Checksum { get; set; }
    }
}
