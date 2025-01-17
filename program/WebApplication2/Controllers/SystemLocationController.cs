﻿using GatePass.DataAccess.SystemLocation;
using GatePass_Project.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using System.Diagnostics;

namespace GatePass.Controllers
{
    public class SystemLocationController : Controller
    {
        private readonly SqlConnection _connection;
        private readonly SystemLocationRepository _locationRepository;



        public SystemLocationController(SqlConnection connection, SystemLocationRepository locationRepository)
        {
            _connection = connection;
            _locationRepository = locationRepository;
        }

        public IActionResult SystemLocation()
        {
            try
            {
                var locations = _locationRepository.GetSystemLocations();
                ViewBag.Locations = locations;
                return View();
            }
            catch (Exception)
            {
                return RedirectToAction("Error");
            }
        }

        public IActionResult CreateSystemLocation()
        {
            return View();
        }

        [HttpPost]
        public IActionResult UploadLocationCSV(IFormFile csvFile)
        {
            var result = _locationRepository.UploadLocationCSV(csvFile);
            TempData["LocationMessage"] = result;
            return RedirectToAction("SystemLocation");
        }

        [HttpPost]
        public IActionResult NewSystemLocation(LocationsModel model)
        {
            var result = _locationRepository.NewSystemLocation(model);
            TempData["LocationMessage"] = result;
            return RedirectToAction("SystemLocation");
        }

        // Helper method to check if the location already exists in the database

        [HttpPost]
        public IActionResult DeleteLocation(int locationId)
        {
            var result = _locationRepository.DeleteLocation(locationId);
            TempData["LocationMessage"] = result;
            return RedirectToAction("SystemLocation");
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}