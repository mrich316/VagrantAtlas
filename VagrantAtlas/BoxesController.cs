using System;
using System.Collections.Generic;
using System.Web.Http;

namespace VagrantAtlas
{
    public class BoxesController : ApiController
    {
        private readonly IBoxRepository _boxRepository;

        public BoxesController(IBoxRepository boxRepository)
        {
            if (boxRepository == null) throw new ArgumentNullException(nameof(boxRepository));

            _boxRepository = boxRepository;
        }

        [HttpHead, HttpGet]
        public IHttpActionResult Get([FromUri] BoxReference boxReference)
        {
            var atlasBox = _boxRepository.Get(boxReference.User, boxReference.Name);

            return (atlasBox == null)
                ? (IHttpActionResult)NotFound()
                : Ok(atlasBox);
        }

        [Authorize]
        [HttpPut]
        public IHttpActionResult Put([FromUri] BoxReferenceVersion boxReferenceVersion, [FromBody] BoxProvider provider)
        {
            var box = new Box
            {
                Name = boxReferenceVersion.Name,
                User = boxReferenceVersion.User,
                Versions = new List<BoxVersion>
                {
                    new BoxVersion
                    {
                        Version = boxReferenceVersion.Version,
                        Providers = new List<BoxProvider>
                        {
                            provider
                        }
                    }
                }
            };

            var newBox = _boxRepository.AddOrUpdate(box);
            return CreatedAtRoute(Constants.RouteNames.Boxes, boxReferenceVersion, newBox);
        }
    }
}