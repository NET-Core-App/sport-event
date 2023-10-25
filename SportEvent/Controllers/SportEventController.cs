using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SportEvent.Data;
using SportEvent.Data.Implementations;
using SportEvent.Data.Interfaces;
using SportEvent.Models;
using System.Net;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SportEvent.Controllers
{
    public class SportEventController : Controller
    {
        private readonly ISportEventRepository _sportEventRepository;
        private readonly IOrganizerRepository _organizerRepository;
        private readonly ILogger<SportEventController> _logger;
        private readonly AuthorizationService _authorization;
        private readonly HttpClient _apiClient;

        public SportEventController(ISportEventRepository sportEventRepository, IOrganizerRepository organizerRepository, ILogger<SportEventController> logger, AuthorizationService authorization)
        {
            _sportEventRepository = sportEventRepository;
            _organizerRepository = organizerRepository;
            _logger = logger;
            _authorization = authorization;
            _apiClient = new HttpClient();
        }
        [HttpGet]
        public async Task<ActionResult> Index(int page = 1, int perPage = 10)
        {
            var IsAuth = _authorization?.IsAuthorization(_apiClient);
            if (IsAuth == false)
            {
                return RedirectToAction("Unauthorized", "User");
            }
            try
            {
                var sportEvents = await _sportEventRepository.GetAll(_apiClient, page, perPage);
                var data = sportEvents.Data;
                var meta = sportEvents.Meta;

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
                return View();
            }
          
        }

        [HttpGet("SportEvent/Details/{id}")]
        public async Task<ActionResult> Detail(int id)
        {
            var IsAuth = _authorization.IsAuthorization(_apiClient);
            if (IsAuth == false)
            {
                return RedirectToAction("Unauthorized", "User");
            }
            try
            {
                var data = await _sportEventRepository.Detail(id, _apiClient);
           /*     if(data == null)
                {
                    return RedirectToAction("Error", "Shared");
                }*/
                return View(data);
            }
            catch
            {
                return View();
            }
        
        }

        [HttpGet]
        public ViewResult Create() => View();


        [HttpPost]
        public async Task<ActionResult> Create(SportEventCreateModel model)
        {
            var IsAuth = _authorization.IsAuthorization(_apiClient);
            if (IsAuth == false)
            {
                return RedirectToAction("Unauthorized", "User");
            }
            try
            {
                var organizers = await _organizerRepository.GetAllOrganizers(_apiClient, 1, 10);
                ViewBag.Organizers = new SelectList((System.Collections.IEnumerable)organizers, "id", "organizerName");

                if (ModelState.IsValid)
                {
                    var registrationSuccess = await _sportEventRepository.Create(model, _apiClient);
                    if (registrationSuccess)
                    {
                        return RedirectToAction("Index", "SportEvent");
                    }
                    else
                    {
                        var errorMessage = "Failed to register Sport Event.";
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
        public async Task<ActionResult> Edit(int id)
        {
            var IsAuth = _authorization.IsAuthorization(_apiClient);
            if (IsAuth == false)
            {
                return RedirectToAction("Unauthorized", "User");
            }
            try
            {
                var data = await _sportEventRepository.GetById(id, _apiClient);
                return View(data);
            }
            catch
            {
                return View();
            }

        }

        [HttpPost]
        public async Task<ActionResult> Edit(int id, SportEventCreateModel data)
        {
            var IsAuth = _authorization.IsAuthorization(_apiClient);
            if (IsAuth == false)
            {
                return RedirectToAction("Unauthorized", "User");
            }
            try
            {
                var updatedOrganizer = await _sportEventRepository.Edit(id, data, _apiClient);

                if (updatedOrganizer == false)
                {
                    return View(data);
                }

                return RedirectToAction("Index", "SportEvent");
            }
            catch (HttpRequestException ex)
            {
                return View(data);
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
                var eventToDelete = await _sportEventRepository.Detail(id, _apiClient);
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
                var eventToDelete = await _sportEventRepository.Delete(id, _apiClient);
                if (eventToDelete == true)
                {
                    return RedirectToAction("Index", "SportEvent");

                }
                else
                {
                    _logger.LogError("Error Delete SportEvent");
                    return RedirectToAction("Index", "SportEvent");
                }
            }
            catch
            {
                return RedirectToAction("Index", "SportEvent");
            }
        }
    }
}
