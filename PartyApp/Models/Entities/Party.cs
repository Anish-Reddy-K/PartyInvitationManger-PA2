using PartyInvitationManager.Models.Entities;
using System.ComponentModel.DataAnnotations;

namespace PartyInvitationManager.Models.Entities
{
    public class Party
    {
        public int Id { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Event Date")]
        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; }
        public string Location { get; set; }

        // Navigation property
        public List<Invitation> Invitations { get; set; } = new List<Invitation>();
    }
}
