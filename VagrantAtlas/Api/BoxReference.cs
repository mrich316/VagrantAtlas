using System.ComponentModel.DataAnnotations;

namespace VagrantAtlas.Api
{
    public class BoxReference
    {
        [Required, StringLength(30), RegularExpression("^[a-zA-Z0-9_]+$")]
        public string User { get; set; }

        [Required, StringLength(60), RegularExpression("^[a-zA-Z0-9_]+$")]
        public string Name { get; set; }
    }
}
