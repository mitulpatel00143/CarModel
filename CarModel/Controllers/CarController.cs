using DAL.Abstract;
using DAL.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CarModel.Controllers
{
    public class CarController : Controller
    {

        private readonly ILogger<CarController> _logger;
        private ICar _carServices { get; set; }
        public CarController(ILogger<CarController> logger, ICar car)
        {
            _logger = logger;
            _carServices = car;
        }



        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult GetData([FromBody] AjaxPostModel model)
        {
            try
            {
                var search = model?.Search?.Value;
                var sortColumn = model.Order?[0]?.Column ?? 0;
                var sortDirection = model.Order?[0]?.Dir ?? "ASC";
                var pageNumber = model.Start / model.Length + 1;
                var pageSize = model.Length;

                var cars = _carServices.GetCarDetailsPage(search, sortColumn, sortDirection, pageNumber, pageSize);
                var carTotalCount = _carServices.GetCarsCount();

                return Json(new
                {
                    recordsTotal = carTotalCount,
                    recordsFiltered = carTotalCount,
                    data = cars
                });
            }
            catch (Exception ex)
            {
                return Json(new { error = "An error occurred on your request." });
            }
        }


        public IActionResult SaveCar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult SaveCar(Car car, IFormFile carImage)
        {

            try
            {

                if (carImage != null && carImage.Length > 0)
                {
                    var uniqueFileName = GetUniqueFileName(carImage.FileName);
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Uploads\\", "Car");
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        carImage.CopyTo(stream);
                    }

                    car.CarImage = Path.Combine("Car", uniqueFileName);
                }
                else
                {
                    car.CarImage = Request.Form["CarImage"];
                }

                ModelState.Remove("CarImage");
                if (ModelState.IsValid)
                {
                    _carServices.SaveCarDetails(car);

                    TempData["SuccessMessage"] = "Car details saved successfully.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errorMessage"] = "Model data is invalid";
                    return View(car);
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "An error occurred while saving car details.";
                return View(car);
            }

        }


        [HttpGet]
        public IActionResult EditCar(int id)
        {
            if (id == null)
            {
                return RedirectToAction("Index");
            }

            Car car = _carServices.GetCarDetailsByCarId(id);

            if (car == null)
            {
                TempData["errorMessage"] = $"Car details not found with Employee code : {id}";
                return RedirectToAction("Index");
            }

            ViewBag.CarImage = car.CarImage;

            return View(car);

        }

        [HttpPost]
        public IActionResult EditCar(Car car, IFormFile carImage)
        {
            try
            {

                if (carImage != null && carImage.Length > 0)
                {
                    var uniqueFileName = GetUniqueFileName(carImage.FileName);
                    var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Uploads\\", "Car");
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        carImage.CopyTo(stream);
                    }

                    car.CarImage = Path.Combine("Car", uniqueFileName);
                }
                else
                {
                    car.CarImage = Request.Form["CarImage"];
                }

                ModelState.Remove("CarImage");
                if (ModelState.IsValid)
                {
                    _carServices.UpdateCarDetails(car);

                    TempData["successMessage"] = "Car details updated successfully";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["errorMessage"] = "Model data is invalid";
                    return View(car);
                }
            }
            catch (Exception ex)
            {
                TempData["errorMessage"] = ex.Message;
                return View(car);
            }

        }

        private string GetUniqueFileName(string fileName)
        {
            fileName = Path.GetFileName(fileName);
            return Path.GetFileNameWithoutExtension(fileName)
                   + "_"
                   + Guid.NewGuid().ToString().Substring(0, 4)
                   + Path.GetExtension(fileName);
        }


        [HttpDelete]
        public IActionResult DeleteCar(string id)
        {
            var result = _carServices.DeleteCarDetails(id);

            if (result == 1)
            {
                return Json(new { success = true, message = "Car details deleted successfully." });
            }
            else
            {
                return Json(new { success = false, message = "Failed to delete Car details." });
            }
        }


    }
}
