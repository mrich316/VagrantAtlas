using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace VagrantAtlas.Api
{
    public class JsonFileBackedAtlas : IBoxRepository
    {
        private readonly string _filePath;
        private readonly JsonSerializerSettings _jsonSettings;
        private readonly IBoxRepository _innerAtlas;

        private readonly object _fileLock = new object();

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

        public Task<IEnumerable<Box>> GetAll(CancellationToken cancellationToken)
        {
            return _innerAtlas.GetAll(cancellationToken);
        }

        public Task<Box> Get(BoxReference boxReference, CancellationToken cancellationToken)
        {
            return _innerAtlas.Get(boxReference, cancellationToken);
        }

        public async Task<Box> AddOrUpdate(Box box)
        {
            var value = await _innerAtlas.AddOrUpdate(box)
                .ConfigureAwait(false);

            await Save(_filePath)
                .ConfigureAwait(false);

            return value;
        }

        private void Open(string storagePath)
        {
            lock (_fileLock)
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
        }

        private async Task Save(string storagePath)
        {
            var boxes = await GetAll(CancellationToken.None)
                .ConfigureAwait(false);

            lock (_fileLock)
            {
                var fileStream = new FileStream(
                    storagePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None);

                using (var writer = new StreamWriter(fileStream))
                {
                    var json = JsonConvert.SerializeObject(boxes.ToList(), _jsonSettings);
                    writer.Write(json);
                }
            }
        }
    }
}