using System.ComponentModel.DataAnnotations;

namespace PartyInvitationManager.Models.ViewModels
{
    public class InvitationCreateViewModel
    {
        public int PartyId { get; set; }

        [Required]
        [Display(Name = "Guest Name")]
        public string GuestName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Guest Email")]
        public string GuestEmail { get; set; }
    }
}