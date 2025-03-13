using System.ComponentModel.DataAnnotations;

namespace PartyInvitationManager.Models.Entities
{
    public class Invitation
{
    public int Id { get; set; }

    [Required]
    [Display(Name = "Guest Name")]
    public string GuestName { get; set; }

    [Required]
    [EmailAddress]
    [Display(Name = "Guest Email")]
    public string GuestEmail { get; set; }

    [Required]
    public InvitationStatus Status { get; set; } = InvitationStatus.InviteNotSent;

    // Foreign key
    public int PartyId { get; set; }

    // Navigation property
    public Party Party { get; set; }
}
}
