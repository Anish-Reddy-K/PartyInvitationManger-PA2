using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartyInvitationManager.Data;
using PartyInvitationManager.Models.Entities;
using PartyInvitationManager.Models.ViewModels;
using PartyInvitationManager.Services;

namespace PartyInvitationManager.Controllers
{
    public class InvitationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IEmailService _emailService;

        public InvitationController(ApplicationDbContext context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }

        // GET: Invitation/Create/5 (PartyId)
        [Route("Invitation/Create/{partyId}")]
        public async Task<IActionResult> Create(int partyId)
        {
            var party = await _context.Parties.FindAsync(partyId);
            if (party == null)
            {
                return NotFound();
            }

            ViewBag.PartyDescription = party.Description;

            var viewModel = new InvitationCreateViewModel
            {
                PartyId = partyId
            };

            return View(viewModel);
        }

        // POST: Invitation/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(InvitationCreateViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var invitation = new Invitation
                {
                    GuestName = viewModel.GuestName,
                    GuestEmail = viewModel.GuestEmail,
                    Status = InvitationStatus.InviteNotSent,
                    PartyId = viewModel.PartyId
                };

                _context.Add(invitation);
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", "Party", new { id = viewModel.PartyId });
            }

            var party = await _context.Parties.FindAsync(viewModel.PartyId);
            if (party == null)
            {
                return NotFound();
            }

            ViewBag.PartyDescription = party.Description;

            return View(viewModel);
        }

        // POST: Invitation/SendInvite/5 (InvitationId)
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Invitation/SendInvite/{id}")]
        public async Task<IActionResult> SendInvite(int id)
        {
            var invitation = await _context.Invitations
                .Include(i => i.Party)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invitation == null)
            {
                return NotFound();
            }

            // Send the email
            await _emailService.SendInvitationEmailAsync(
                invitation.GuestEmail,
                invitation.GuestName,
                invitation.Party.Description,
                invitation.Party.EventDate,
                invitation.Party.Location,
                invitation.Id);

            // Update the status
            invitation.Status = InvitationStatus.InviteSent;
            _context.Update(invitation);
            await _context.SaveChangesAsync();

            return RedirectToAction("Details", "Party", new { id = invitation.PartyId });
        }

        // GET: Invitation/Respond/5 (InvitationId)
        [Route("Invitation/Respond/{id}")]
        public async Task<IActionResult> Respond(int id)
        {
            var invitation = await _context.Invitations
                .Include(i => i.Party)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invitation == null)
            {
                return NotFound();
            }

            var viewModel = new InvitationResponseViewModel
            {
                InvitationId = invitation.Id,
                GuestName = invitation.GuestName,
                PartyDescription = invitation.Party.Description,
                PartyDate = invitation.Party.EventDate,
                PartyLocation = invitation.Party.Location
            };

            return View(viewModel);
        }

        // POST: Invitation/Respond
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Respond(InvitationResponseViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var invitation = await _context.Invitations.FindAsync(viewModel.InvitationId);
                if (invitation == null)
                {
                    return NotFound();
                }

                invitation.Status = viewModel.WillAttend ? InvitationStatus.RespondedYes : InvitationStatus.RespondedNo;
                _context.Update(invitation);
                await _context.SaveChangesAsync();

                return View("Thanks", viewModel);
            }

            return View(viewModel);
        }
    }
}