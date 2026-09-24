using Microsoft.AspNetCore.Mvc;
using OTPSettings.Data;
using OTPSettings.Models;
using System.Diagnostics;

namespace OTPSettings.Controllers
{
    public class OTPSettingController : Controller
    {
        private readonly IOTPSettingRepository _repository;
        private readonly ILogger<OTPSettingController> _logger;

        public OTPSettingController(IOTPSettingRepository repository, ILogger<OTPSettingController> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var model = await _repository.GetAsync();
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to load OTP setting.");
                TempData["ErrorMessage"] = "Failed to load OTP setting.";
                return View(new OTPSettingViewModel());
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(OTPSettingViewModel model)
        {
            try
            {
                await _repository.UpdateAsync(model);
                TempData["SuccessMessage"] = "OTP setting updated successfully.";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update OTP setting.");
                TempData["ErrorMessage"] = "Failed to update OTP setting.";
            }

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
