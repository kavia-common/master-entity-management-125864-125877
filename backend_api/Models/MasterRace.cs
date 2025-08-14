using System;

namespace dotnet.Models
{
    /// <summary>
    /// Domain model representing a MasterRace entity.
    /// </summary>
    public class MasterRace
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; } = string.Empty;

        public string? Description { get; set; }

        public string? Origin { get; set; }

        /// <summary>
        /// Power level of the master race. If IsExtinct is true, must be 0.
        /// Otherwise must be in range [1, 9000].
        /// </summary>
        public int PowerLevel { get; set; }

        public bool IsExtinct { get; set; }

        public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

        public DateTimeOffset UpdatedAt { get; set; } = DateTimeOffset.UtcNow;
    }
}
