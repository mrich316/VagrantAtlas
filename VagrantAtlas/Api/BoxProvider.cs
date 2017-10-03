using System.ComponentModel.DataAnnotations;

namespace VagrantAtlas.Api
{
    public class BoxProvider
    {
        [Required, StringLength(50)]
        public string Name { get; set; }

        [Required, StringLength(4096), Url]
        public string Url { get; set; }

        [Required, RegularExpression("^md5|sha1|sha256$")]
        public string ChecksumType { get; set; }

        [Required, StringLength(64), RegularExpression("^[a-fA-F0-9]+$")]
        public string Checksum { get; set; }
    }
}
