using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class CarViewModel
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Class { get; set; }
        public string ModelName { get; set; }
        public string ModelCode { get; set; }
        public string Description { get; set; }
        public string Feature { get; set; }
        public string Price { get; set; }
        public DateTime DateOfManufacturing { get; set; }
        public string IsActive { get; set; }
        public string CarImage { get; set; }
    }
}
