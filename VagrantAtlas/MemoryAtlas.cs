﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace VagrantAtlas
{
    public class MemoryAtlas : IBoxRepository
    {
        private readonly ConcurrentDictionary<string, Box> _atlas = new ConcurrentDictionary<string, Box>();

        public MemoryAtlas(IEnumerable<Box> boxes)
        {
            foreach (var box in boxes.Where(box => !_atlas.TryAdd(GetId(box), box)))
            {
                throw new ArgumentException(
                    string.Format("Multiple boxes with same id found: {0} already exist.", GetId(box)),
                    "boxes");
            }
        }

        public MemoryAtlas()
        {
        }

        protected string GetId(Box box)
        {
            return string.Format("{0}/{1}", box.User, box.Name);
        }

        public IEnumerable<Box> GetAll()
        {
            return _atlas.Values;
        }

        public Box Get(string id)
        {
            Box box;
            _atlas.TryGetValue(id, out box);

            return box;
        }

        public Box AddOrUpdate(Box box)
        {
            var id = GetId(box);

            return _atlas.AddOrUpdate(id,
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
        }
    }
}