using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SportEvent.Data;
using SportEvent.Data.Interfaces;
using SportEvent.Models;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Reflection;

namespace SportEvent.Controllers
{
    public class OrganizerController : Controller
    {
        private readonly IOrganizerRepository _organizerRepository;
        private readonly ILogger<OrganizerController> _logger;
        private readonly HttpClient _apiClient;
        private readonly AuthorizationService _authorization;

        public OrganizerController(IOrganizerRepository organizerRepository, ILogger<OrganizerController> logger, AuthorizationService authorization)
        {
            _organizerRepository = organizerRepository;
            _logger = logger;
            _authorization = authorization;
            _apiClient = new HttpClient();
        }
             

        [HttpGet]
        public ViewResult Create() => View();


        [HttpPost]
        public async Task<ActionResult> Create(Organizer model)
        {
            var IsAuth = _authorization.IsAuthorization(_apiClient);
            if (IsAuth == false)
            {
                return RedirectToAction("Unauthorized", "User");
            }
            try
            {
                if (ModelState.IsValid)
                {
                    var registrationSuccess = await _organizerRepository.Create(model, _apiClient);

                    if (registrationSuccess)
                    {
                        return RedirectToAction("Index", "Organizer");
                    }
                    else
                    {
                        var errorMessage = "Failed to register user.";
                        ModelState.AddModelError(string.Empty, errorMessage);

                        return View(model);
                    }
                }
                return View();
            }
            catch
            {
                return View();
            }
        }
        [HttpGet]
        public async Task<ActionResult> Index(int page=1, int perPage=10)
        {
            var IsAuth = _authorization.IsAuthorization(_apiClient);
            if (IsAuth == false)
            {
                return RedirectToAction("Unauthorized", "User");
            }

            if (page <= 0 || perPage <= 0)
            {
                return RedirectToAction("InvalidPage", "Error");
            }

            try
            {
                var organizers = await _organizerRepository.GetAllOrganizers(_apiClient, page, perPage);

                if (organizers == null)
                {
                    return RedirectToAction("Login", "User");
                }

                var data = organizers.Data;
                var meta = organizers.Meta;

                int totalItems = meta.pagination.total;
                int itemsPerPage = meta.pagination.per_page;
                int totalPages = meta.pagination.total_pages;
                int currentPage = meta.pagination.current_page;

                ViewBag.CurrentPage = currentPage;
                ViewBag.TotalPages = totalPages;
                ViewBag.ItemsPerPage = itemsPerPage;

                return View(data);
            }
            catch
            {
                return RedirectToAction("Login", "User");
            }
        }


        [HttpGet("Organizer/Details/{id}")]
        public async Task<ActionResult> Detail(int id)
        {
            var IsAuth = _authorization.IsAuthorization(_apiClient);
            if (IsAuth == false)
            {
                return RedirectToAction("Unauthorized", "User");
            }
            try
            {
                var organizer = await _organizerRepository.GetById(id, _apiClient);
                return View(organizer);
            }
            catch
            {
                return View();
            }

        }

        [HttpGet]
        public async Task<ActionResult> Edit(int id)
        {
            var IsAuth = _authorization.IsAuthorization(_apiClient);
            if (IsAuth == false)
            {
                return RedirectToAction("Unauthorized", "User");
            }
            try
            {
                var organizer = await _organizerRepository.GetById(id, _apiClient);
                return View(organizer);
            }
            catch
            {
                return View();
            }

        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, Organizer student)
        {
            var IsAuth = _authorization.IsAuthorization(_apiClient);
            if (IsAuth == false)
            {
                return RedirectToAction("Unauthorized", "User");
            }
            try
            {
                var updatedOrganizer = await _organizerRepository.EditOrganizer(id, student, _apiClient);

                if (updatedOrganizer == false)
                {
                    return View(student);
                }

                return RedirectToAction("Index", "Organizer");
            }
            catch (HttpRequestException ex)
            {
                return View(student);
            }
        }

        [HttpGet]
        public async Task<ActionResult> Delete(int id)
        {
            var IsAuth = _authorization.IsAuthorization(_apiClient);
            if (IsAuth == false)
            {
                return RedirectToAction("Unauthorized", "User");
            }
            try
            {
                var eventToDelete = await _organizerRepository.GetById(id, _apiClient);
                return View(eventToDelete);
            }
            catch
            {
                return View();
            }
        }

        [HttpPost]
        public async Task<ActionResult> Delete(int id, IFormCollection collection)
        {
            var IsAuth = _authorization.IsAuthorization(_apiClient);
            if (IsAuth == false)
            {
                return RedirectToAction("Unauthorized", "User");
            }
            try
            {
                var eventToDelete = await _organizerRepository.Delete(id, _apiClient);
                if (eventToDelete == true)
                {
                    return RedirectToAction("Index", "Organizer");

                }
                else
                {
                    _logger.LogError("Error Delete Organizer");
                    return RedirectToAction("Index", "Organizer");
                }
            }
            catch
            {
                return RedirectToAction("Index", "Organizer");
            }
        }
    }
}
