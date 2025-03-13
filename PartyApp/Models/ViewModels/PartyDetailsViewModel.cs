using PartyInvitationManager.Models.Entities;
using System.Collections.Generic;

namespace PartyInvitationManager.Models.ViewModels
{
    public class PartyDetailsViewModel
    {
        public Party Party { get; set; }
        public List<Invitation> Invitations { get; set; }
        public int TotalInvites { get; set; }
        public int TotalSent { get; set; }
        public int TotalRespondedYes { get; set; }
        public int TotalRespondedNo { get; set; }
        public int TotalPending { get; set; }
    }
}