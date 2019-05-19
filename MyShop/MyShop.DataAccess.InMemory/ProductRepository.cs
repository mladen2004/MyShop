using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;//dodajemo reference na navedena dva elementa
using MyShop.Core.Models;

namespace MyShop.DataAccess.InMemory
{
    public class ProductRepository
    {
        ObjectCache cache = MemoryCache.Default;

        List<Product> products=new List<Product>();

        //kreiranje konstruktora
        public ProductRepository()
        {
            products = cache["products"] as List<Product>;
            //ako je cache prazan
            if (products == null)
            {
                //kreiramo novu praznu listu
                products = new List<Product>();
            }
        }

            //metoda za pohranu liste u cache
       public void Commit()
       {
             cache["products"] = products;
       }
        //metoda za dodavanje novog proizvoda u listu
        public void Insert(Product p)
        {
            products.Add(p);
        }
        //update metoda
        public void Update(Product product)
        {
            Product ProductToUpdate = products.Find(b => b.Id == product.Id);
            if (ProductToUpdate != null)
            {
                //ako postoji traženi proizvod isti mijenjamo s novim
                ProductToUpdate = product;
            }
            //ako proizvod ne postoji bacamo novu iznimku
            else
            {
                throw new Exception("Product not found");
            }
        }

        //pretraživanje proizvoda prema id-u
        public Product Find(string Id)
        {
            Product product = products.Find(b => b.Id == Id);
            if (product != null)
            {
                //ako postoji traženi proizvod isti vračamo
                return product;
            }
            //ako proizvod ne postoji bacamo novu iznimku
            else
            {
                throw new Exception("Product not found");
            }
        }
        //metoda koja vraća listu proizvoda

        public IQueryable<Product> Collection()
        {
            return products.AsQueryable();
        }

        public void Delete(string Id)
        {
            Product product = products.Find(b => b.Id == Id);
            if (product != null)
            {
                //ako postoji traženi proizvod isti brišemo
                products.Remove(product);
            }
            //ako proizvod ne postoji bacamo novu iznimku
            else
            {
                throw new Exception("Product not found");
            }

        }
        
    }
}
