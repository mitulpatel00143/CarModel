using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Entities
{
    public class Car
    {
        public int Id { get; set; }
        public string Brand { get; set; }
        public string Class { get; set; }
        [DisplayName("Model Name")]
        public string ModelName { get; set; }
        [DisplayName("Model Code")]
        [RegularExpression(@"^[0-9]{10}$", ErrorMessage = "Invalid Mobile Number.")]
        public string ModelCode { get; set; }
        public string Description { get; set; }
        public string Feature { get; set; }
        public Decimal Price { get; set; }
        [DisplayName("Date Of Manufacturing")]
        public DateTime DateOfManufacturing { get; set; }
        [DisplayName("Is Active")]
        public bool IsActive { get; set; }
        [DisplayName("Car Image")]
        public string CarImage { get; set; }

    }

    public class AjaxPostModel
    {
        public int Draw { get; set; }
        public AjaxRequestColumns[] Columns { get; set; }
        public AjaxRequestOrder[] Order { get; set; }
        public int Start { get; set; }
        public int Length { get; set; }
        public DataSearch Search { get; set; }
    }

    public class AjaxRequestColumns
    {
        public string Data { get; set; }
        public string Name { get; set; }
        public bool Searchable { get; set; }
        public bool Orderable { get; set; }
        public DataSearch Search { get; set; }
    }

    public class AjaxRequestOrder
    {
        public int Column { get; set; }
        public string Dir { get; set; }
    }

    public class DataSearch
    {
        public string Value { get; set; }
        public bool Regex { get; set; }
    }

}
