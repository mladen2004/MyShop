using MyShop.Core.Contracts;
using MyShop.Core.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyShop.DataAccess.SQL
{
    public class SQLRepository<T> : IRepository<T> where T : BaseEntity
    {
        //entity framework kešira podatke i ne zapisuje ih odmah u tablicu zbog čega imamo commit naredbu


        //implementacija datacontext klase kojom smo uspostavili vezu s bazom podataka
        internal DataContext context;
        //implementacija underlaying table seta kojem ćemo htjeti pristupiti
        internal DbSet<T> dbSet;//using system data entity
        //db set ima niz svojih find metoda kao npr find

        //navedenim naredbama (DataContext naredbama) mapiramo proizvod s tablicom baze
        //kreiranje konstruktora koji nam mora omogućiti proslijeđivanje data konteksta context
        public SQLRepository(DataContext context){
            this.context = context;
            //postavljanje underlying tablice referenciranjem konteksta i pozivom set naredbe u koju proslijeđujemo
            //model na kojem radimo
            this.dbSet = context.Set<T>();

            }


        public IQueryable<T> Collection()
        {
            return dbSet;

        }

     

        public void Commit()
        {
            context.SaveChanges();
        }

     

        public void Delete(string Id)
        {
            //dbSet.Remove(Find(Id));
            //prvo pronalazimo objekt
            var t = Find(Id);
            //provjeravamo entry state objekta
            if (context.Entry(t).State == EntityState.Detached)
            {//ako je objekt u bilo kojem drugom stanju osim detached tada
             //ef zna za njega i vodi računa o njemu, što je nešto kao invalid
             //state ako nije attached
                dbSet.Attach(t);//nakon što je objekt povezan s našom ef tablicom, možemo ga ukloniti
                
            }
            dbSet.Remove(t);


        }

     

        public T Find(string Id)
        {
            return dbSet.Find(Id);

        }

     

        public void Insert(T t)
        {
            dbSet.Add(t);
        }

    
        //zbog toga jer entity framework ne pohranjuje promjene odmah u bazu podataka koristimo naredbu commit
        //u sljedećem primjeru pripremamo model za pohranu u bazu podataka preko prve naredbe
        //drugom naredbom se model nakon što upotrijebimo naredbu commit pohranjuje u samu bezu
        public void Update(T t)
        {
            dbSet.Attach(t);//kačimo objekt na entity framework tablicu
            context.Entry(t).State =EntityState.Modified;//ovime govorimo ef-u da pozivom save changes naredbe potraži model t i 
                                                         //proslijedi ga u bazu podataka
               
        }

      
    }
}
