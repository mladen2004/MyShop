using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
    public class ProductCategoryRepository { 

    ObjectCache cache = MemoryCache.Default;

    List<ProductCategory> productCategories = new List<ProductCategory>();

    //kreiranje konstruktora
    public ProductCategoryRepository()
    {
        productCategories = cache["productCategories"] as List<ProductCategory>;
        //ako je cache prazan
        if (productCategories == null)
        {
            //kreiramo novu praznu listu
            productCategories = new List<ProductCategory>();
        }
    }

    //metoda za pohranu liste u cache
    public void Commit()
    {
        cache["productCategories"] = productCategories;
    }
    //metoda za dodavanje novog proizvoda u listu
    public void Insert(ProductCategory p)
    {
        productCategories.Add(p);
    }
    //update metoda
    public void Update(ProductCategory productCategory)
    {
            ProductCategory ProductCategoryToUpdate = productCategories.Find(b => b.Id == productCategory.Id);
        if (ProductCategoryToUpdate != null)
        {
            //ako postoji traženi proizvod isti mijenjamo s novim
            ProductCategoryToUpdate = productCategory;
        }
        //ako proizvod ne postoji bacamo novu iznimku
        else
        {
            throw new Exception("Product Category not found");
        }
    }

    //pretraživanje proizvoda prema id-u
    public ProductCategory Find(string Id)
    {
            ProductCategory product = productCategories.Find(b => b.Id == Id);
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

    public IQueryable<ProductCategory> Collection()
    {
        return productCategories.AsQueryable();
    }

    public void Delete(string Id)
    {
            ProductCategory product = productCategories.Find(b => b.Id == Id);
        if (product != null)
        {
            //ako postoji traženi proizvod isti brišemo
            productCategories.Remove(product);
        }
        //ako proizvod ne postoji bacamo novu iznimku
        else
        {
            throw new Exception("Product not found");
        }

    }

}
}
