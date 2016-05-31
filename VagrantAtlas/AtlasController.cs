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
            if (boxRepository == null) throw new ArgumentNullException("boxRepository");
            _boxRepository = boxRepository;
        }

        [HttpGet, HttpHead]
        public IHttpActionResult Get()
        {
            var boxes = _boxRepository
                .GetAll()
                .Select(box => new { box.Name, box.Tags, box.Description });

            return Ok(boxes);
        }
    }
}