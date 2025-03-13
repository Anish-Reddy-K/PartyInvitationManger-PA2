using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PartyInvitationManager.Data;
using PartyInvitationManager.Models.Entities;
using PartyInvitationManager.Models.ViewModels;

namespace PartyInvitationManager.Controllers
{
    public class PartyController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PartyController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Party
        public async Task<IActionResult> Index()
        {
            return View(await _context.Parties.ToListAsync());
        }

        // GET: Party/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var party = await _context.Parties
                .Include(p => p.Invitations)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (party == null)
            {
                return NotFound();
            }

            var viewModel = new PartyDetailsViewModel
            {
                Party = party,
                Invitations = party.Invitations,
                TotalInvites = party.Invitations.Count,
                TotalSent = party.Invitations.Count(i => i.Status == InvitationStatus.InviteSent ||
                                                     i.Status == InvitationStatus.RespondedYes ||
                                                     i.Status == InvitationStatus.RespondedNo),
                TotalRespondedYes = party.Invitations.Count(i => i.Status == InvitationStatus.RespondedYes),
                TotalRespondedNo = party.Invitations.Count(i => i.Status == InvitationStatus.RespondedNo),
                TotalPending = party.Invitations.Count(i => i.Status == InvitationStatus.InviteNotSent)
            };

            return View(viewModel);
        }

        // GET: Party/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Party/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Description,EventDate,Location")] Party party)
        {
            if (ModelState.IsValid)
            {
                _context.Add(party);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(party);
        }

        // GET: Party/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var party = await _context.Parties.FindAsync(id);
            if (party == null)
            {
                return NotFound();
            }
            return View(party);
        }

        // POST: Party/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Description,EventDate,Location")] Party party)
        {
            if (id != party.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(party);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PartyExists(party.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(party);
        }

        // GET: Party/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var party = await _context.Parties
                .FirstOrDefaultAsync(m => m.Id == id);
            if (party == null)
            {
                return NotFound();
            }

            return View(party);
        }

        // POST: Party/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var party = await _context.Parties.FindAsync(id);
            if (party != null)
            {
                _context.Parties.Remove(party);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        private bool PartyExists(int id)
        {
            return _context.Parties.Any(e => e.Id == id);
        }
    }
}