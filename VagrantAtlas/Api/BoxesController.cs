using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace VagrantAtlas.Api
{
    public class BoxesController : Controller
    {
        private readonly IBoxRepository _boxRepository;

        public BoxesController(IBoxRepository boxRepository)
        {
            if (boxRepository == null) throw new ArgumentNullException(nameof(boxRepository));

            _boxRepository = boxRepository;
        }

        [Route("boxes/{user}/{name}", Name = Constants.RouteNames.Boxes)]
        [HttpHead, HttpGet]
        public async Task<IActionResult> Get(BoxReference boxReference, CancellationToken cancellationToken)
        {
            var box = await _boxRepository.Get(boxReference, cancellationToken)
                .ConfigureAwait(false);

            if (box == null)
            {
                return NotFound();
            }

            return Ok(box);
        }

        [Authorize]
        [HttpPut("boxes/{user}/{name}/{version}")]
        public async Task<IActionResult> Put(BoxReferenceVersion boxReferenceVersion, [FromBody] BoxProvider provider)
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

            var newBox = await _boxRepository.AddOrUpdate(box)
                .ConfigureAwait(false);

            return CreatedAtRoute(Constants.RouteNames.Boxes, boxReferenceVersion, newBox);
        }
    }
}