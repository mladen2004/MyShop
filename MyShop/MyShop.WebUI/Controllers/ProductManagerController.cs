using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.DataAccess.InMemory;

namespace MyShop.WebUI.Controllers
{
    public class ProductManagerController : Controller

        //Unutar product i productCategory klase moramo naslijediti BaseEntity klasu
    {
        //kreiramo instancu product repositorija
        // GET: ProductManager
        IRepository<Product> context;//deklaracija
        //konstruktor  koji inicijalizira taj repozitorij
        IRepository<ProductCategory> productCategories;//koristimo kako bi mogli povući kategorije proizvoda iz baze podataka
        public ProductManagerController(IRepository<Product> productContext, IRepository<ProductCategory> productCategoriesContext)//injection of classes through constructor
        {
            //context = new InMemoryRepository<Product>();//instanciranje
            //productCategories = new InMemoryRepository<ProductCategory>();
            context = productContext;
            productCategories = productCategoriesContext;
        }
        public ActionResult Index()// vraća listu proizvoda
        {
            List<Product> products = context.Collection().ToList();
            return View(products);
        }

        public ActionResult Create()//dodajemo mogućnost kreiranaj novog objekta korištenjem view modela kako bi 
                                    //dodali mogućnost odabira kategorije iz liste ------------------
        {
            //Product product = new Product();
            //return View(product);

            ProductManagerViewModel viewModel = new ProductManagerViewModel();//sadrži kao svojstvo objekt product i INumerable kategorije
            viewModel.Product = new Product();//kreiramo novi prazni objekt proizvod
            viewModel.ProductCategories = productCategories.Collection();//instanciramo kolekciju kategorija proizvoda
            //u view vračamo view model koji smo upravo kreirali
            return View(viewModel);
        }
        [HttpPost]
        public ActionResult Create(Product product, HttpPostedFileBase file)
        {
            if (!ModelState.IsValid)
            {
                return View(product);//ako proslijeđeni model nije ispravan vraća se view za unos modela
            }
            else
            {//dodajemo model kolekciji objekata
                if (file != null) { //ako slika proizvoda postoji
                    product.Image = product.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + product.Image);
                }
                context.Insert(product);
                context.Commit();//dodajemo objekt model u cache memoriju
                return RedirectToAction("Index");

            }

        }

        public ActionResult Edit(string id)
        {
            Product product = context.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            else {
                //obzirom da moramo vratiti view model isti moramo instancirati
                //prvo instanciramo model koji ćemo povezati s edit modelom, a zatim moramo instancirati i listu pošto je i ona dio view modela

                ProductManagerViewModel viewModel = new ProductManagerViewModel();
                viewModel.Product = product;
                viewModel.ProductCategories = productCategories.Collection();
                //na ovaj način smo i kod create-a i kod edita dobili unutar samog modela listu iz koje možemo povući dropdown listu

                return View(viewModel);
            }
        }
        [HttpPost]
        public ActionResult Edit(Product product,string Id, HttpPostedFile file)
        {
            Product productToEdit = context.Find(Id);
            if (productToEdit == null)
            {
                return HttpNotFound();
            }
            else
            {
                if (!ModelState.IsValid)
                {
                    return View(product);
                }
                if (file != null)
                { //ako slika proizvoda postoji
                    productToEdit.Image = productToEdit.Id + Path.GetExtension(file.FileName);
                    file.SaveAs(Server.MapPath("//Content//ProductImages//") + productToEdit.Image);
                }

                
                productToEdit.Name = product.Name;
                productToEdit.Price = product.Price;
                productToEdit.Categoriy = product.Categoriy;
                productToEdit.Description = product.Description;

                context.Commit();
                return RedirectToAction("Index");
               
                
            }
        }

        public ActionResult Delete(string Id)
        {
            Product productToDelete = context.Find(Id);
            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                return View(productToDelete);
            }
        }
        [HttpPost]
        [ActionName("Delete")]
        public ActionResult ConfirmDelete(string Id)
        {
            Product productToDelete = context.Find(Id);
            if (productToDelete == null)
            {
                return HttpNotFound();
            }
            else
            {
                context.Delete(Id);
                context.Commit();
                return RedirectToAction("Index");
            }

        }
    }
}