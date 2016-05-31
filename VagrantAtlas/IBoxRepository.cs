using System.Collections.Generic;

namespace VagrantAtlas
{
    public interface IBoxRepository
    {
        IEnumerable<Box> GetAll();

        Box Get(string id);

        Box AddOrUpdate(Box box);
    }
}
