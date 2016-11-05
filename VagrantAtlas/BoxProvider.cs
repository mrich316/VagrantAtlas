using System.ComponentModel.DataAnnotations;

namespace VagrantAtlas
{
    public class BoxProvider
    {
        [Required, StringLength(50)]
        public string Name { get; set; }

        [Required, Url]
        public string Url { get; set; }

        [Required, RegularExpression("^md5|sha1|sha256$")]
        public string ChecksumType { get; set; }

        [Required, RegularExpression("^[a-fA-F0-9]+$")]
        public string Checksum { get; set; }
    }
}
