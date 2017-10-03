using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace VagrantAtlas.Api
{
    public class AtlasController : Controller
    {
        private readonly IBoxRepository _boxRepository;

        public AtlasController(IBoxRepository boxRepository)
        {
            if (boxRepository == null) throw new ArgumentNullException(nameof(boxRepository));
            _boxRepository = boxRepository;
        }

        [Route("atlas", Name = Constants.RouteNames.Default)]
        [HttpGet, HttpHead]
        public async Task<IActionResult> Get(CancellationToken cancellationToken)
        {
            var boxes = await _boxRepository.GetAll(cancellationToken)
                .ConfigureAwait(false);

            var viewModels = boxes.Select(box => new
            {
                box.User,
                box.Name,
                box.Tags,
                box.Description,
                box.Versions,
                Href = Url.Link(Constants.RouteNames.Boxes, new {box.User, box.Name})
            }).ToList();

            return Ok(viewModels);
        }
    }
}