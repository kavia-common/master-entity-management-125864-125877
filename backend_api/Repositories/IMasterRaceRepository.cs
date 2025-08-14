using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dotnet.Models;

namespace dotnet.Repositories
{
    // PUBLIC_INTERFACE
    public interface IMasterRaceRepository
    {
        /// <summary>
        /// Returns all master races.
        /// </summary>
        Task<IReadOnlyList<MasterRace>> GetAllAsync();

        /// <summary>
        /// Get a master race by id.
        /// </summary>
        Task<MasterRace?> GetByIdAsync(Guid id);

        /// <summary>
        /// Add a new master race.
        /// </summary>
        Task AddAsync(MasterRace entity);

        /// <summary>
        /// Update an existing master race.
        /// </summary>
        Task UpdateAsync(MasterRace entity);

        /// <summary>
        /// Delete a master race by id. Returns true if deleted.
        /// </summary>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Returns true if any entity exists with the given name (case-insensitive),
        /// excluding the optional entity with excludeId (useful during updates).
        /// </summary>
        Task<bool> ExistsByNameAsync(string name, Guid? excludeId = null);
    }
}
