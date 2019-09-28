using Hotel.Data;
using Hotel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.ServiceDepence
{
    public class ServiceDepenses:IserviceDepenses
    {
        public void Add(Depenses entity)
        {
            using (var ctx = new DatabContext())
            {
                ctx.Depenses.Add(entity);
                ctx.SaveChanges();
            }
        }

        public void Commit()
        {
            using (var ctx = new DatabContext())
            {
                ctx.SaveChanges();
            }
        }

        public void Delete(Expression<Func<Depenses, bool>> where)
        {
            using (var ctx = new DatabContext())
            {
                IEnumerable<Depenses> objects = ctx.Depenses.Where(where).AsEnumerable();
                foreach (Depenses obj in objects)
                    ctx.Depenses.Remove(obj);
                ctx.SaveChanges();

            }
        }

        public void Delete(Depenses entity)
        {
            using (var ctx = new DatabContext())
            {

                ctx.Depenses.Remove(entity);
                ctx.SaveChanges();

            }
        }

        public Depenses Get(Expression<Func<Depenses, bool>> where)
        {
            using (var ctx = new DatabContext())
            {

                return ctx.Depenses.Where(where).FirstOrDefault();


            }
        }

        public IEnumerable<Depenses> GetAll()
        {
            using (var ctx = new DatabContext())
            {

                return ctx.Depenses;

            }
        }

        public Depenses GetById(long id)
        {
            using (var ctx = new DatabContext())
            {

                return ctx.Depenses.Find(id);

            }
        }

        public Depenses GetById(string id)
        {
            using (var ctx = new DatabContext())
            {

                return ctx.Depenses.Find(id);

            }
        }

        public IEnumerable<Depenses> GetMany(Expression<Func<Depenses, bool>> where = null, Expression<Func<Depenses, bool>> orderBy = null)
        {
            IQueryable<Depenses> Query;
            using (var ctx = new DatabContext())
            {

                Query = ctx.Depenses;
                if (where != null)
                {
                    Query = Query.Where(where);
                }
                if (orderBy != null)
                {
                    Query = Query.OrderBy(orderBy);
                }
                return Query.Reverse().ToList();

            }
        }


        public void Update(Depenses entity)
        {
            using (var ctx = new DatabContext())
            {

                ctx.Depenses.Attach(entity);
                ctx.Entry(entity).State = EntityState.Modified;
                ctx.SaveChanges();

            }
        }
    }
}
