using DAL.Abstract;
using DAL.Entities;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.ConstrainedExecution;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Services
{
    public class CarServices : ICar
    {
        private readonly ILogger _logger;
        private readonly IUnitOfWork _UnitOfWork;
        public CarServices(ILogger<CarServices> logger, IUnitOfWork unitofwork)
        {
            _logger = logger;
            _UnitOfWork = unitofwork;
        }


        public int GetCarsCount()
        {

            var result = _UnitOfWork.GetDataSet("stp_car_GetCarCount");
            return Convert.ToInt32(result.Tables[0].Rows[0]["CarCount"]);
        }

        public List<CarViewModel> GetCarDetails()
        {

            DataSet dataSet = _UnitOfWork.GetDataSet("stp_car_GetAllCars");
            List<CarViewModel> cars = new List<CarViewModel>();

            foreach (DataRow dr in dataSet.Tables[0].Rows)
            {

                CarViewModel carDetails = new CarViewModel()
                {
                    Brand = Convert.ToString(dr["Brand"]),
                    Class = Convert.ToString(dr["Class"]),
                    ModelName = Convert.ToString(dr["ModelName"]),
                    ModelCode = Convert.ToString(dr["ModelCode"]),
                    Description = Convert.ToString(dr["Description"]),
                    Feature = Convert.ToString(dr["Feature"]),
                    Price = Convert.ToString(dr["Price"]),
                    DateOfManufacturing = Convert.ToDateTime(dr["DateOfManufacturing"]),
                    IsActive = Convert.ToString(dr["IsActive"]),
                    CarImage = Convert.ToString(dr["CarImage"]),
                };
                cars.Add(carDetails);
            }

            return cars;
        }

        public List<CarViewModel> GetCarDetailsPage(string search, int sortColumn, string sortDirection, int pageNumber, int pageSize)
        {
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("@SearchTerm", (object)search ?? DBNull.Value),
                new SqlParameter("@SortColumn", GetSortColumnName(sortColumn)),
                new SqlParameter("@SortDirection", sortDirection),
                new SqlParameter("@PageNumber", pageNumber),
                new SqlParameter("@PageSize", pageSize)
            };

            List<CarViewModel> cars = new List<CarViewModel>();
            try
            {
                DataSet dataSet = _UnitOfWork.GetDataSet("stp_car_GetAllCars", parameters);

                foreach (DataRow dr in dataSet.Tables[0].Rows)
                {

                    CarViewModel carDetails = new CarViewModel()
                    {
                        Id = Convert.ToInt32(dr["Id"]),
                        Brand = Convert.ToString(dr["Brand"]),
                        Class = Convert.ToString(dr["Class"]),
                        ModelName = Convert.ToString(dr["ModelName"]),
                        ModelCode = Convert.ToString(dr["ModelCode"]),
                        Description = Convert.ToString(dr["Description"]),
                        Feature = Convert.ToString(dr["Feature"]),
                        Price = Convert.ToString(dr["Price"]),
                        DateOfManufacturing = Convert.ToDateTime(dr["DateOfManufacturing"]),
                        IsActive = Convert.ToString(dr["IsActive"]),
                        CarImage = Convert.ToString(dr["CarImage"]),
                    };
                    cars.Add(carDetails);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GetCarsDetailsPage");
            }

            return cars;
        }
        private string GetSortColumnName(int sortColumn)
        {
            switch (sortColumn)
            {
                case 0: return "DateOfManufacturing";
                case 2: return "ModelName";
                case 7: return "DateOfManufacturing";

                default: return "DateOfManufacturing";
            }
        }


        public Car GetCarDetailsByCarId(int id)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("@Id", id)
            };

            DataSet dataSet = _UnitOfWork.GetDataSet("stp_car_GetCarById", sqlParameters);
            Car carDetails = new Car();

            if (dataSet.Tables[0].Rows.Count > 0)
            {
                DataRow dr = dataSet.Tables[0].Rows[0];

                carDetails.Id = Convert.ToInt32(dr["Id"]);
                carDetails.Brand = Convert.ToString(dr["Brand"]);
                carDetails.Class = Convert.ToString(dr["Class"]);
                carDetails.ModelName = Convert.ToString(dr["ModelName"]);
                carDetails.ModelCode = Convert.ToString(dr["ModelCode"]);
                carDetails.Description = Convert.ToString(dr["Description"]);
                carDetails.Feature = Convert.ToString(dr["Feature"]);
                carDetails.Price = Convert.ToDecimal(dr["Price"]);
                carDetails.DateOfManufacturing = Convert.ToDateTime(dr["DateOfManufacturing"]);
                carDetails.IsActive = Convert.ToBoolean(dr["IsActive"]);
                carDetails.CarImage = Convert.ToString(dr["CarImage"]);
            }

            return carDetails;
        }

        public void SaveCarDetails(Car car)
        {
            var parameters = new List<SqlParameter>
            {
                new SqlParameter("@Brand", car.Brand),
                new SqlParameter("@Class", car.Class),
                new SqlParameter("@ModelName", car.ModelName),
                new SqlParameter("@ModelCode", car.ModelCode),
                new SqlParameter("@Description", car.Description),
                new SqlParameter("@Feature", car.Feature),
                new SqlParameter("@Price", car.Price),
                new SqlParameter("@DateOfManufacturing", car.DateOfManufacturing),
                new SqlParameter("@IsActive", car.IsActive),
                new SqlParameter("@CarImage", car.CarImage)
            };
            _UnitOfWork.ExecuteNonQueryStoredProcedure("stp_car_InsertCar", parameters, false);
        }

        public void UpdateCarDetails(Car car)
        {
            var parameters = new List<SqlParameter>
            {
               new SqlParameter("@Id", car.Id),
               new SqlParameter("@Brand", car.Brand),
                new SqlParameter("@Class", car.Class),
                new SqlParameter("@ModelName", car.ModelName),
                new SqlParameter("@ModelCode", car.ModelCode),
                new SqlParameter("@Description", car.Description),
                new SqlParameter("@Feature", car.Feature),
                new SqlParameter("@Price", car.Price),
                new SqlParameter("@DateOfManufacturing", car.DateOfManufacturing),
                new SqlParameter("@IsActive", car.IsActive),
                new SqlParameter("@CarImage", car.CarImage)
            };
            _UnitOfWork.ExecuteNonQueryStoredProcedure("stp_car_UpdateCar", parameters, false);
        }

        public int DeleteCarDetails(string id)
        {
            int flag = 0;

            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>
                {
                    new SqlParameter("@Id", id)
                };

                _UnitOfWork.ExecuteNonQueryStoredProcedure("stp_car_DeleteCar", sqlParameters, true);
                flag = 1;
            }
            catch (Exception ex)
            {
                flag = 0;
            }

            return flag;
        }


    }
}
