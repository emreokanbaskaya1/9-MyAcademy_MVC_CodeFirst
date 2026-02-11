using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace _9_MyAcademy_MVC_CodeFirst.Data.Entities
{
    public class PolicySale
    {
        public int Id { get; set; }

        // Customer Information
        [Required(ErrorMessage = "First name is required")]
        [MaxLength(100)]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [MaxLength(100)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [MaxLength(200)]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [MaxLength(20)]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [MaxLength(11)]
        [Display(Name = "TC Identity Number")]
        public string TCIdentityNumber { get; set; }

        [MaxLength(500)]
        [Display(Name = "Address")]
        public string Address { get; set; }

        // Policy Information
        [Required(ErrorMessage = "Policy/Product is required")]
        [Display(Name = "Policy")]
        public int ProductId { get; set; }

        public virtual Product Product { get; set; }

        [Required]
        [Display(Name = "Sale Amount")]
        public decimal SaleAmount { get; set; }

        [Display(Name = "Policy Start Date")]
        public DateTime PolicyStartDate { get; set; } = DateTime.Now;

        [Display(Name = "Policy End Date")]
        public DateTime PolicyEndDate { get; set; } = DateTime.Now.AddYears(1);

        // Sale Status
        [Display(Name = "Payment Status")]
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        // Manual Policy Status (only for Cancelled and Suspended)
        [Display(Name = "Manual Policy Status")]
        public PolicyStatus? ManualPolicyStatus { get; set; }

        [MaxLength(1000)]
        [Display(Name = "Notes")]
        public string Notes { get; set; }

        // Audit Fields
        [Display(Name = "Created Date")]
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Display(Name = "Updated Date")]
        public DateTime? UpdatedDate { get; set; }

        [MaxLength(100)]
        [Display(Name = "Created By")]
        public string CreatedBy { get; set; }

        public bool IsActive { get; set; } = true;

        // Helper property for full name
        [NotMapped]
        public string FullName => $"{FirstName} {LastName}";

        // Helper property for policy number (auto-generated format)
        [NotMapped]
        public string PolicyNumber => $"POL-{CreatedDate:yyyyMMdd}-{Id:D5}";

        // Computed Policy Status based on dates and manual override
        [NotMapped]
        [Display(Name = "Policy Status")]
        public PolicyStatus PolicyStatus
        {
            get
            {
                // If manually set to Cancelled or Suspended, return that
                if (ManualPolicyStatus == PolicyStatus.Cancelled || ManualPolicyStatus == PolicyStatus.Suspended)
                {
                    return ManualPolicyStatus.Value;
                }

                // Auto-calculate based on dates
                var now = DateTime.Now.Date;
                var startDate = PolicyStartDate.Date;
                var endDate = PolicyEndDate.Date;

                if (now < startDate)
                {
                    // Policy hasn't started yet
                    return PolicyStatus.Active; // Or you could add a "Pending" status
                }
                else if (now >= startDate && now <= endDate)
                {
                    // Policy is currently active
                    return PolicyStatus.Active;
                }
                else
                {
                    // Policy has expired
                    return PolicyStatus.Expired;
                }
            }
        }

        // Method to manually set status
        public void SetManualStatus(PolicyStatus status)
        {
            if (status == PolicyStatus.Cancelled || status == PolicyStatus.Suspended)
            {
                ManualPolicyStatus = status;
            }
            else
            {
                // Clear manual status for Active/Expired (auto-calculated)
                ManualPolicyStatus = null;
            }
        }
    }

    public enum PaymentStatus
    {
        Pending = 0,
        Paid = 1,
        PartiallyPaid = 2,
        Refunded = 3,
        Cancelled = 4
    }

    public enum PolicyStatus
    {
        Active = 0,
        Expired = 1,
        Cancelled = 2,
        Suspended = 3
    }
}
