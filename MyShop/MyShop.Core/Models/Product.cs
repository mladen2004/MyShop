using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.Core.Models
{
    public class Product:BaseEntity
        //deklariramo je kao public jer mora biti dostupna iz drugih projekata, sadrži informacije o proizvodima
    {
        //public string Id { get; set; }
        [StringLength(20)]
        [DisplayName("Product Name")]
        public string Name { get; set; }
        public string Description { get; set; }
        [Range(0,1000)]
        public decimal Price { get; set; }
        public string Categoriy { get; set; }
        public string Image { get; set; }//sadržavat će url na product image

        //public Product()//product konstruktor
        //{
        //    this.Id = Guid.NewGuid().ToString();//kod svakog kereiranja novog proizvoda generirati će se novi guid

        //}
        //Obzirom da baseEntity klasa sadrži konstruktor za id isti nije potrebno instancirati u ovoj klasi koja je nasljeđuje
    }
}
