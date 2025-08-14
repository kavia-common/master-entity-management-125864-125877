using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet.Models;

namespace dotnet.Repositories
{
    /// <summary>
    /// Simple in-memory repository for MasterRace entities.
    /// </summary>
    public class InMemoryMasterRaceRepository : IMasterRaceRepository
    {
        private readonly ConcurrentDictionary<Guid, MasterRace> _store = new();

        public Task<IReadOnlyList<MasterRace>> GetAllAsync()
        {
            var list = _store.Values
                .OrderByDescending(x => x.CreatedAt)
                .ToList()
                .AsReadOnly();
            return Task.FromResult((IReadOnlyList<MasterRace>)list);
        }

        public Task<MasterRace?> GetByIdAsync(Guid id)
        {
            _store.TryGetValue(id, out var value);
            return Task.FromResult(value);
        }

        public Task AddAsync(MasterRace entity)
        {
            _store[entity.Id] = entity;
            return Task.CompletedTask;
        }

        public Task UpdateAsync(MasterRace entity)
        {
            _store[entity.Id] = entity;
            return Task.CompletedTask;
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            return Task.FromResult(_store.TryRemove(id, out _));
        }

        public Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null)
        {
            var exists = _store.Values.Any(v =>
                string.Equals(v.Name.Trim(), name.Trim(), StringComparison.OrdinalIgnoreCase)
                && (!excludeId.HasValue || v.Id != excludeId.Value));
            return Task.FromResult(exists);
        }
    }
}
