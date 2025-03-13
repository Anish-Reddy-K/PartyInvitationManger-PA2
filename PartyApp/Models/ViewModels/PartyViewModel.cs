using System;
using System.ComponentModel.DataAnnotations;

namespace PartyInvitationManager.Models.ViewModels
{
    public class PartyViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter a description for the party")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Please enter the event date")]
        [Display(Name = "Event Date")]
        [DataType(DataType.Date)]
        public DateTime EventDate { get; set; }

        [Required(ErrorMessage = "Please enter the location")]
        public string Location { get; set; }
    }
}