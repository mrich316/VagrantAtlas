using System.ComponentModel.DataAnnotations;

namespace VagrantAtlas
{
    public class BoxReferenceVersion : BoxReference
    {
        [Required, RegularExpression(@"^(0|[1-9]\d*)\.(0|[1-9]\d*)\.(0|[1-9]\d*)$")]
        public string Version { get; set; }
    }
}
