using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace VagrantAtlas
{
    public class JsonFileBackedAtlas : IBoxRepository
    {
        private readonly string _filePath;
        private readonly JsonSerializerSettings _jsonSettings;
        private readonly IBoxRepository _innerAtlas;

        public JsonFileBackedAtlas(string filePath, JsonSerializerSettings jsonSettings)
            : this(filePath, jsonSettings, new MemoryAtlas())
        {
            // Empty.
        }

        public JsonFileBackedAtlas(string filePath, JsonSerializerSettings jsonSettings, IBoxRepository innerAtlas)
        {
            if (filePath == null) throw new ArgumentNullException(nameof(filePath));
            if (jsonSettings == null) throw new ArgumentNullException(nameof(jsonSettings));
            if (innerAtlas == null) throw new ArgumentNullException(nameof(innerAtlas));

            _filePath = filePath;
            _jsonSettings = jsonSettings;
            _innerAtlas = innerAtlas;

            Open(filePath);
        }

        public IEnumerable<Box> GetAll()
        {
            return _innerAtlas.GetAll();
        }

        public Box Get(string user, string boxName)
        {
            return _innerAtlas.Get(user, boxName);
        }

        public Box AddOrUpdate(Box box)
        {
            var value = _innerAtlas.AddOrUpdate(box);

            Save(_filePath);

            return value;
        }

        private void Open(string storagePath)
        {
            if (!File.Exists(storagePath)) return;

            var fileStream = new FileStream(
                storagePath, FileMode.Open, FileAccess.Read, FileShare.None);

            using (var reader = new StreamReader(fileStream))
            {
                var json = reader.ReadToEnd();
                var boxes = JsonConvert.DeserializeObject<List<Box>>(json, _jsonSettings);

                foreach (var box in boxes)
                {
                    _innerAtlas.AddOrUpdate(box);
                }
            }
        }

        private void Save(string storagePath)
        {
            var fileStream = new FileStream(
                storagePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);

            using (var writer = new StreamWriter(fileStream))
            {
                var json = JsonConvert.SerializeObject(GetAll().ToList(), _jsonSettings);
                writer.Write(json);
            }
        }
    }
}