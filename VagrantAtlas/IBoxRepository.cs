using System.Collections.Generic;

namespace VagrantAtlas
{
    public interface IBoxRepository
    {
        IEnumerable<Box> GetAll();

        Box Get(string user, string boxName);

        Box AddOrUpdate(Box box);
    }
}
