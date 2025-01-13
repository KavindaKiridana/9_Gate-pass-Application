using GatePass.DataAccess.ItemTracker;
using GatePass.DataAccess.MyReceipt;
using GatePass_Project.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NuGet.Protocol.Core.Types;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace GatePass_Project.Controllers
{
    public class ItemTrackerController : Controller
    {
        private readonly ILogger<ItemTrackerController> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly ItemTrackerRepository _trackerRepository;

        public ItemTrackerController(ILogger<ItemTrackerController> logger, IConfiguration configuration, ItemTrackerRepository trackerRepository)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = _configuration.GetConnectionString("DefaultConnection");
            _trackerRepository = trackerRepository;
        }

        public IActionResult ItemTracker()
        {
            try
            {
                var requests = _trackerRepository.ItemTracker();
                return View(requests);
            }
            catch (Exception)
            {
                return RedirectToAction("Error");
            }
        }

        [HttpPost]
        public IActionResult MarkAsReturned(int requestRefNo)
        {
            try
            {
                _trackerRepository.MarkAsReturned(requestRefNo);
                return RedirectToAction("ItemTracker");
            }
            catch (Exception)
            {
                return RedirectToAction("Error");
            }
        }

        public IActionResult PendingReturnableDetails(int id)
        {
            List<ItemTrackerModel> items = _trackerRepository.GetDetailsById(id);

            if (items.Count == 0)
            {
                return RedirectToAction("Error");
            }

            return View(items);
        }
    }
}