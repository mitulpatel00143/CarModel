using DAL.Entities;
using DAL.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Abstract
{
    public interface ICar
    {
        int GetCarsCount();
        List<CarViewModel> GetCarDetails();
        List<CarViewModel> GetCarDetailsPage(string search, int sortColumn, string sortDirection, int pageNumber, int pageSize);

        Car GetCarDetailsByCarId(int id);


        void SaveCarDetails(Car car);
        void UpdateCarDetails(Car car);
        int DeleteCarDetails(string Id);

    }
}
