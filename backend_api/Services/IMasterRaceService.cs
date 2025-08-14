using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dotnet.Dtos;

namespace dotnet.Services
{
    // PUBLIC_INTERFACE
    public interface IMasterRaceService
    {
        /// <summary>
        /// Returns all master races.
        /// </summary>
        Task<IReadOnlyList<MasterRaceResponse>> GetAllAsync();

        /// <summary>
        /// Get a master race by id.
        /// </summary>
        Task<MasterRaceResponse?> GetByIdAsync(Guid id);

        /// <summary>
        /// Create a new master race. Returns ValidationProblem on failure.
        /// </summary>
        Task<(MasterRaceResponse? created, ValidationProblem? problem)> CreateAsync(MasterRaceCreateRequest request);

        /// <summary>
        /// Update a master race. Returns notFound=false on success, along with any validation problem.
        /// If notFound is true, the target entity does not exist.
        /// </summary>
        Task<(MasterRaceResponse? updated, bool notFound, ValidationProblem? problem)> UpdateAsync(Guid id, MasterRaceUpdateRequest request);

        /// <summary>
        /// Delete a master race. Returns true if deleted, false if not found.
        /// </summary>
        Task<bool> DeleteAsync(Guid id);
    }
}
