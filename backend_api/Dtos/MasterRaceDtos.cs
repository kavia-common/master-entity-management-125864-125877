using System;
using System.Collections.Generic;
using dotnet.Models;

namespace dotnet.Dtos
{
    /// <summary>
    /// Request body for creating a new MasterRace.
    /// </summary>
    public class MasterRaceCreateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Origin { get; set; }
        public int PowerLevel { get; set; } = 1;
        public bool IsExtinct { get; set; } = false;
    }

    /// <summary>
    /// Request body for updating an existing MasterRace.
    /// </summary>
    public class MasterRaceUpdateRequest
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Origin { get; set; }
        public int PowerLevel { get; set; }
        public bool IsExtinct { get; set; }
    }

    /// <summary>
    /// API response representation of MasterRace.
    /// </summary>
    public class MasterRaceResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? Origin { get; set; }
        public int PowerLevel { get; set; }
        public bool IsExtinct { get; set; }
        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset UpdatedAt { get; set; }

        public static MasterRaceResponse From(MasterRace entity)
        {
            return new MasterRaceResponse
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Origin = entity.Origin,
                PowerLevel = entity.PowerLevel,
                IsExtinct = entity.IsExtinct,
                CreatedAt = entity.CreatedAt,
                UpdatedAt = entity.UpdatedAt
            };
        }
    }

    /// <summary>
    /// Helper representing validation problem errors (field -> messages).
    /// </summary>
    public class ValidationProblem
    {
        public Dictionary<string, string[]> Errors { get; set; } = new Dictionary<string, string[]>();
        public bool HasErrors => Errors.Count > 0;

        public void Add(string field, string message)
        {
            if (!Errors.TryGetValue(field, out var list))
            {
                list = Array.Empty<string>();
            }
            var newList = new List<string>(list) { message };
            Errors[field] = newList.ToArray();
        }
    }
}
