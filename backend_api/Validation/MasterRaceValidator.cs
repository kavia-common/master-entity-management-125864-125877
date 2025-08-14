using System;
using System.Text.RegularExpressions;
using dotnet.Dtos;

namespace dotnet.Validation
{
    /// <summary>
    /// Validator for MasterRace DTOs implementing business rules.
    /// </summary>
    public static class MasterRaceValidator
    {
        private static readonly Regex NamePattern = new Regex(@"^[A-Za-z0-9][A-Za-z0-9 \-]{1,48}[A-Za-z0-9]$"); // 3-50, alnum/space/hyphen, no leading/trailing space/hyphen

        /// <summary>
        /// Validate create request. 
        /// Rules:
        /// - Name: required, 3-50 chars, alphanumeric with spaces/hyphens, must be unique (checked externally)
        /// - Description: optional, <= 500 chars
        /// - Origin: optional, <= 100 chars
        /// - PowerLevel: 
        ///     If IsExtinct = true -> must be exactly 0
        ///     If IsExtinct = false -> 1..9000
        /// </summary>
        public static ValidationProblem ValidateCreate(MasterRaceCreateRequest req)
        {
            var problem = new ValidationProblem();

            ValidateName(req.Name, problem);
            ValidateDescription(req.Description, problem);
            ValidateOrigin(req.Origin, problem);
            ValidatePower(req.PowerLevel, req.IsExtinct, problem);

            return problem;
        }

        /// <summary>
        /// Validate update request. Same rules as create.
        /// </summary>
        public static ValidationProblem ValidateUpdate(MasterRaceUpdateRequest req)
        {
            var problem = new ValidationProblem();

            ValidateName(req.Name, problem);
            ValidateDescription(req.Description, problem);
            ValidateOrigin(req.Origin, problem);
            ValidatePower(req.PowerLevel, req.IsExtinct, problem);

            return problem;
        }

        private static void ValidateName(string? name, ValidationProblem problem)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                problem.Add(nameof(name), "Name is required.");
                return;
            }
            if (name.Length < 3 || name.Length > 50)
            {
                problem.Add(nameof(name), "Name must be between 3 and 50 characters.");
            }
            // Trim to validate pattern without leading/trailing whitespace
            var candidate = name.Trim();
            if (!NamePattern.IsMatch(candidate))
            {
                problem.Add(nameof(name), "Name can only include letters, numbers, spaces and hyphens, and cannot start/end with a space or hyphen.");
            }
        }

        private static void ValidateDescription(string? description, ValidationProblem problem)
        {
            if (!string.IsNullOrEmpty(description) && description.Length > 500)
            {
                problem.Add(nameof(description), "Description must be 500 characters or fewer.");
            }
        }

        private static void ValidateOrigin(string? origin, ValidationProblem problem)
        {
            if (!string.IsNullOrEmpty(origin) && origin.Length > 100)
            {
                problem.Add(nameof(origin), "Origin must be 100 characters or fewer.");
            }
        }

        private static void ValidatePower(int powerLevel, bool isExtinct, ValidationProblem problem)
        {
            if (isExtinct)
            {
                if (powerLevel != 0)
                {
                    problem.Add(nameof(powerLevel), "Extinct master races must have a power level of 0.");
                }
                return;
            }

            if (powerLevel < 1 || powerLevel > 9000)
            {
                problem.Add(nameof(powerLevel), "PowerLevel must be between 1 and 9000 when the race is not extinct.");
            }
        }
    }
}
