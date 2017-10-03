using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace VagrantAtlas.Api
{
    public interface IBoxRepository
    {
        Task<IEnumerable<Box>> GetAll(CancellationToken cancellationToken);

        Task<Box> Get(BoxReference reference, CancellationToken cancellationToken);

        Task<Box> AddOrUpdate(Box box);
    }
}
