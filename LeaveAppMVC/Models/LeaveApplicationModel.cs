using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LeaveAppMVC.Models
{
    public class LeaveApplicationModel : IValidatableObject
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Employee name is required.")]
        public string EmployeeName { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddTHH:mm}")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-ddTHH:mm}")]
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Justification is required.")]
        [MinLength(20, ErrorMessage = "Justification must be at least 20 characters.")]
        public string Justification { get; set; }

        [Required(ErrorMessage = "Manager name is required.")]
        public string ManagerName { get; set; }

        public string? Status { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndDate <= StartDate)
            {
                yield return new ValidationResult(
                    "End date must be later than start date.",
                    new[] { nameof(LeaveApplicationModel.EndDate) }
                );
            }
        }
    }
}

