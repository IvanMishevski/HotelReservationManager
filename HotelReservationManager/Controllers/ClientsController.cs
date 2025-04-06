﻿using HotelReservationManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HotelReservationManager.Controllers
{
    public class ClientsController : Controller
    {
        private readonly HotelReservationContext _context;
        private readonly UserManager<User> _userManager;
        public ClientsController(HotelReservationContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public async Task<IActionResult> Index(string searchString, int pageSize = 10, int pageNumber = 1)
        {
            var clients = _context.Clients.AsQueryable();

            if (!string.IsNullOrEmpty(searchString))
            {
                clients = clients.Where(u => u.FirstName.Contains(searchString) ||
                                        u.LastName.Contains(searchString));
            }

            int totalClients = await clients.CountAsync();
            var clientsList = await clients.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            ViewBag.TotalPages = (int)Math.Ceiling((double)totalClients / pageSize);
            ViewBag.CurrentPage = pageNumber;

            return View(clientsList);
        }
        public async Task<IActionResult> Reservations(string id, int pageSize = 10, int pageNumber = 1)
        {
            var client = await _context.Clients
                .Include(c => c.ReservationClients)
                    .ThenInclude(rc => rc.Reservation)
                        .ThenInclude(r => r.Room)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (client == null)
            {
                return NotFound();
            }

            var reservations = client.ReservationClients
                .Select(rc => rc.Reservation)
                .OrderByDescending(r => r.CheckInDate)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            int totalReservations = client.ReservationClients.Count;

            ViewBag.ClientName = $"{client.FirstName} {client.LastName}";
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalReservations / pageSize);
            ViewBag.CurrentPage = pageNumber;
            ViewBag.ClientId = id;

            return View(reservations);
        }
        private bool ClientExists(string id)
        {
            return _context.Clients.Any(e => e.Id == id);
        }

    }
}
