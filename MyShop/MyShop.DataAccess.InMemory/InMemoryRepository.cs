using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.InMemory
{
    public class InMemoryRepository<T> where T:BaseEntity //kad god prenosimo objekt T on mora biti vrste BaseEntity ili barem
                                                          //naslijediti BaseEntity
    {
        //object cache
        ObjectCache cache = MemoryCache.Default;
        List<T> items;
        string className;

        public InMemoryRepository()
        {
            className = typeof(T).Name;//reflection metoda kojom putem typeof metode dohvaćamo ime klase 
            //inicijalizacija unutarnjeg items storage-a
            items = cache[className] as List<T>;
            //provjera je li kolekcija prazna
            if (items == null)
            {
                //instanciramo novu praznu listu objekata tipa T
                items = new List<T>();
            }

        }

        public void Commit()
        {
            cache[className] = items;
        }

        public void Insert(T t)
        {
            items.Add(t);
        }

        public void Update(T t)
        {
            T tToUpdate = items.Find(b => b.Id == t.Id);
            if (tToUpdate != null)
            {
                tToUpdate = t;
            }
            else
            {
                throw new Exception(className + "not found!");
            }
        }

        public T Find(string Id)
        {
            T tToFind = items.Find(b => b.Id == Id);//kod bilo koje find procedure id javlja grešku jer program ne zna 
                                                    //što je to Id, T može biti bilo što i ne mora nužno sadržavati id
                                                    //pa stoga uvodimo novu base klasu koja kao property sadrži id
                                                     //i koju mora naslijediti klasa InMemoryRepository
            if (tToFind != null)
            {
                return (tToFind);
            }
            else
            {
                throw new Exception(className + "not found!");
            }
        }
        //kreiranje metode koja vraća kolekciju objekata
        public IQueryable<T> Collection()
        {
            return items.AsQueryable();
        }
        //kreiranje delete metode
        public void Delete(string Id)
        {
            T tToDelete= items.Find(b => b.Id == Id);
            if (tToDelete!= null)
            {
                items.Remove(tToDelete);
            }
            else
            {
                throw new Exception(className + "not found!");
            }
        }

    }
}
