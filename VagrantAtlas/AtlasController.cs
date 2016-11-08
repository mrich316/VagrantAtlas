using System;
using System.Linq;
using System.Web.Http;

namespace VagrantAtlas
{
    public class AtlasController : ApiController
    {
        private readonly IBoxRepository _boxRepository;

        public AtlasController(IBoxRepository boxRepository)
        {
            if (boxRepository == null) throw new ArgumentNullException(nameof(boxRepository));
            _boxRepository = boxRepository;
        }

        [HttpGet, HttpHead]
        public IHttpActionResult Get()
        {
            var boxes = _boxRepository
                .GetAll()
                .Select(box => new
                {
                    box.User,
                    box.Name,
                    box.Tags,
                    box.Description,
                    box.Versions,
                    Href = Url.Link(
                        Constants.RouteNames.Boxes,
                        new {box.User, box.Name})
                });

            return Ok(boxes);
        }
    }
}