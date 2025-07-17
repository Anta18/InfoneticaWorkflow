using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

namespace Infrastructure.Storage
{
    /// A simple in-memory store that can snapshot to a JSON file.

    public class InMemoryStore<T>
    {
        private readonly ConcurrentDictionary<Guid, T> _dict = new();
        private readonly string? _persistPath;

        public InMemoryStore(string? persistPath = null)
        {
            _persistPath = persistPath;
            if (_persistPath != null && File.Exists(_persistPath))
            {
                var json = File.ReadAllText(_persistPath);
                var items = JsonSerializer.Deserialize<List<T>>(json,
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (items != null)
                    foreach (var item in items)
                    {
                        var idProp = item.GetType().GetProperty("Id");
                        if (idProp?.GetValue(item) is Guid id)
                            _dict[id] = item;
                    }
            }
        }

        public Task AddAsync(Guid id, T item)
        {
            _dict[id] = item;
            return PersistAsync();
        }

        public Task UpdateAsync(Guid id, T item)
        {
            _dict[id] = item;
            return PersistAsync();
        }

        public Task<T?> GetAsync(Guid id)
            => Task.FromResult(_dict.TryGetValue(id, out var item) ? item : default);

        public Task<IEnumerable<T>> ListAsync()
            => Task.FromResult<IEnumerable<T>>(_dict.Values);

        private Task PersistAsync()
        {
            if (_persistPath == null) return Task.CompletedTask;
            var json = JsonSerializer.Serialize(_dict.Values,
                new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(_persistPath, json);
            return Task.CompletedTask;
        }
    }
}
