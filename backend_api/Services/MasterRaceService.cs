using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using dotnet.Dtos;
using dotnet.Models;
using dotnet.Repositories;
using dotnet.Validation;

namespace dotnet.Services
{
    /// <summary>
    /// Service implementing business logic for MasterRace operations.
    /// </summary>
    public class MasterRaceService : IMasterRaceService
    {
        private readonly IMasterRaceRepository _repository;

        public MasterRaceService(IMasterRaceRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<MasterRaceResponse>> GetAllAsync()
        {
            var items = await _repository.GetAllAsync();
            var list = new List<MasterRaceResponse>(items.Count);
            foreach (var it in items)
            {
                list.Add(MasterRaceResponse.From(it));
            }
            return list.AsReadOnly();
        }

        public async Task<MasterRaceResponse?> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity is null ? null : MasterRaceResponse.From(entity);
        }

        public async Task<(MasterRaceResponse? created, ValidationProblem? problem)> CreateAsync(MasterRaceCreateRequest request)
        {
            var problem = MasterRaceValidator.ValidateCreate(request);

            // Check uniqueness of Name
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                var exists = await _repository.ExistsByNameAsync(request.Name);
                if (exists)
                {
                    problem.Add(nameof(request.Name), "A master race with the same name already exists.");
                }
            }

            if (problem.HasErrors)
            {
                return (null, problem);
            }

            var now = DateTimeOffset.UtcNow;
            var entity = new MasterRace
            {
                Id = Guid.NewGuid(),
                Name = request.Name.Trim(),
                Description = request.Description?.Trim(),
                Origin = request.Origin?.Trim(),
                PowerLevel = request.PowerLevel,
                IsExtinct = request.IsExtinct,
                CreatedAt = now,
                UpdatedAt = now
            };

            await _repository.AddAsync(entity);
            return (MasterRaceResponse.From(entity), null);
        }

        public async Task<(MasterRaceResponse? updated, bool notFound, ValidationProblem? problem)> UpdateAsync(Guid id, MasterRaceUpdateRequest request)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing is null)
            {
                return (null, true, null);
            }

            var problem = MasterRaceValidator.ValidateUpdate(request);

            // Check uniqueness of Name (excluding current entity)
            if (!string.IsNullOrWhiteSpace(request.Name))
            {
                var exists = await _repository.ExistsByNameAsync(request.Name, excludeId: id);
                if (exists)
                {
                    problem.Add(nameof(request.Name), "A master race with the same name already exists.");
                }
            }

            if (problem.HasErrors)
            {
                return (null, false, problem);
            }

            existing.Name = request.Name.Trim();
            existing.Description = request.Description?.Trim();
            existing.Origin = request.Origin?.Trim();
            existing.PowerLevel = request.PowerLevel;
            existing.IsExtinct = request.IsExtinct;
            existing.UpdatedAt = DateTimeOffset.UtcNow;

            await _repository.UpdateAsync(existing);
            return (MasterRaceResponse.From(existing), false, null);
        }

        public Task<bool> DeleteAsync(Guid id)
        {
            return _repository.DeleteAsync(id);
        }
    }
}
