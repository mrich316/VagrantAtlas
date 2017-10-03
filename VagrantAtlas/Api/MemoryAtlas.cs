using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace VagrantAtlas.Api
{
    public class MemoryAtlas : IBoxRepository
    {
        private readonly ConcurrentDictionary<string, Box> _atlas = new ConcurrentDictionary<string, Box>();

        public MemoryAtlas(IEnumerable<Box> boxes)
        {
            foreach (var box in boxes.Where(box => !_atlas.TryAdd(GetId(box), box)))
            {
                throw new ArgumentException(
                    $"Multiple boxes with same id found: {GetId(box)} already exist.",
                    nameof(boxes));
            }
        }

        public MemoryAtlas()
        {
        }

        protected string GetId(Box box)
        {
            return GetId(box.User, box.Name);
        }

        protected string GetId(string user, string boxName)
        {
            return $"{user}/{boxName}";
        }

        public Task<IEnumerable<Box>> GetAll(CancellationToken cancellationToken)
        {
            return Task.FromResult(_atlas.Values.AsEnumerable());
        }

        public Task<Box> Get(BoxReference boxReference, CancellationToken cancellationToken)
        {
            if (boxReference == null) throw new ArgumentNullException(nameof(boxReference));

            var id = GetId(boxReference.User, boxReference.Name);

            _atlas.TryGetValue(id, out Box box);

            return Task.FromResult(box);
        }

        public Task<Box> AddOrUpdate(Box box)
        {
            var id = GetId(box);

            var newBox = _atlas.AddOrUpdate(id,
                box,
                (key, existingBox) =>
                {
                    // overwrite existing versions.
                    var versions = box.Versions.Concat(existingBox.Versions).Distinct().ToList();
                    existingBox.Versions = versions;

                    if (box.Description != null)
                    {
                        existingBox.Description = box.Description;
                    }

                    if (box.Tags.Any())
                    {
                        existingBox.Tags = box.Tags;
                    }

                    return existingBox;
                });

            return Task.FromResult(newBox);
        }
    }
}