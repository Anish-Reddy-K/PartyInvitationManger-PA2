using Microsoft.EntityFrameworkCore;
using PartyInvitationManager.Data;
using PartyInvitationManager.Models.Entities;
using PartyInvitationManager.Models.ViewModels;

namespace PartyInvitationManager.Services
{
    public class PartyService
    {
        private readonly ApplicationDbContext _context;

        public PartyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Party>> GetAllPartiesAsync()
        {
            return await _context.Parties.ToListAsync();
        }

        public async Task<PartyDetailsViewModel> GetPartyDetailsAsync(int id)
        {
            var party = await _context.Parties
                .Include(p => p.Invitations)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (party == null)
            {
                return null;
            }

            var viewModel = new PartyDetailsViewModel
            {
                Party = party,
                Invitations = party.Invitations.ToList(),
                TotalInvites = party.Invitations.Count,
                TotalSent = party.Invitations.Count(i => i.Status != InvitationStatus.InviteNotSent),
                TotalRespondedYes = party.Invitations.Count(i => i.Status == InvitationStatus.RespondedYes),
                TotalRespondedNo = party.Invitations.Count(i => i.Status == InvitationStatus.RespondedNo),
                TotalPending = party.Invitations.Count(i => i.Status == InvitationStatus.InviteSent)
            };

            return viewModel;
        }

        public async Task<bool> CreatePartyAsync(Party party)
        {
            _context.Parties.Add(party);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdatePartyAsync(Party party)
        {
            _context.Entry(party).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PartyExists(party.Id))
                {
                    return false;
                }
                else
                {
                    throw;
                }
            }
        }

        public async Task<bool> DeletePartyAsync(int id)
        {
            var party = await _context.Parties.FindAsync(id);

            if (party == null)
            {
                return false;
            }

            _context.Parties.Remove(party);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> CreateInvitationAsync(InvitationCreateViewModel model)
        {
            var invitation = new Invitation
            {
                GuestName = model.GuestName,
                GuestEmail = model.GuestEmail,
                Status = InvitationStatus.InviteNotSent,
                PartyId = model.PartyId
            };

            _context.Invitations.Add(invitation);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SendInvitationAsync(int id)
        {
            var invitation = await _context.Invitations.FindAsync(id);

            if (invitation == null)
            {
                return false;
            }

            invitation.Status = InvitationStatus.InviteSent;
            _context.Update(invitation);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<InvitationResponseViewModel> GetInvitationForResponseAsync(int id)
        {
            var invitation = await _context.Invitations
                .Include(i => i.Party)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invitation == null)
            {
                return null;
            }

            var viewModel = new InvitationResponseViewModel
            {
                InvitationId = invitation.Id,
                GuestName = invitation.GuestName,
                PartyDescription = invitation.Party.Description,
                PartyDate = invitation.Party.EventDate,
                PartyLocation = invitation.Party.Location
            };

            return viewModel;
        }

        public async Task<bool> RespondToInvitationAsync(InvitationResponseViewModel model)
        {
            var invitation = await _context.Invitations.FindAsync(model.InvitationId);

            if (invitation == null)
            {
                return false;
            }

            invitation.Status = model.WillAttend ? InvitationStatus.RespondedYes : InvitationStatus.RespondedNo;
            _context.Update(invitation);
            await _context.SaveChangesAsync();
            return true;
        }

        private bool PartyExists(int id)
        {
            return _context.Parties.Any(e => e.Id == id);
        }
    }
}