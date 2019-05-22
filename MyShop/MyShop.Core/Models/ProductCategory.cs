using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models
{
    public class ProductCategory:BaseEntity
    {
        //public string Id { get; set; }
        public string Category { get; set; }
        //kreiranje konstruktora koji će automatski izgenerirati Id
        //public ProductCategory()
        //{
        //    this.Id = Guid.NewGuid().ToString();
        //}
        //Obzirom da baseEntity klasa sadrži konstruktor za id isti nije potrebno instancirati u ovoj klasi koja je nasljeđuje

    }
}
