using Hotel.Data;
using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.ServiceRevenu
{
    public class ServiceRevenu:IserviceRevenu
    {

        public void Add(Revenu entity)
        {
            using (var ctx = new DatabContext())
            {
                ctx.Revenus.Add(entity);
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

        public void Delete(Expression<Func<Revenu, bool>> where)
        {
            using (var ctx = new DatabContext())
            {
                IEnumerable<Revenu> objects = ctx.Revenus.Where(where).AsEnumerable();
                foreach (Revenu obj in objects)
                    ctx.Revenus.Remove(obj);
                ctx.SaveChanges();

            }
        }

        public void Delete(Revenu entity)
        {
            using (var ctx = new DatabContext())
            {

                ctx.Revenus.Remove(entity);
                ctx.SaveChanges();

            }
        }

        public Revenu Get(Expression<Func<Revenu, bool>> where)
        {
            using (var ctx = new DatabContext())
            {

                return ctx.Revenus.Where(where).Include(h=>h.reservation).FirstOrDefault();


            }
        }


        public IEnumerable<Revenu> GetAll()
        {
            using (var ctx = new DatabContext())
            {

                return ctx.Revenus;

            }
        }

        public Revenu GetById(long id)
        {
            using (var ctx = new DatabContext())
            {

                return ctx.Revenus.Find(id);

            }
        }

        public Revenu GetById(string id)
        {
            using (var ctx = new DatabContext())
            {

                return ctx.Revenus.Find(id);

            }
        }

        public IEnumerable<Revenu> GetMany(Expression<Func<Revenu, bool>> where = null, Expression<Func<Revenu, bool>> orderBy = null)
        {
            IQueryable<Revenu> Query;
            using (var ctx = new DatabContext())
            {

                Query = ctx.Revenus.Include(x => x.reservation);
                if (where != null)
                {
                    Query = Query.Where(where);
                }
                if (orderBy != null)
                {
                    Query = Query.OrderBy(orderBy);
                }
                return Query.ToList();

            }
        }


        public void Update(Revenu entity)
        {
            using (var ctx = new DatabContext())
            {

                ctx.Revenus.Attach(entity);
                ctx.Entry(entity).State = EntityState.Modified;
                ctx.SaveChanges();

            }
        }

    }
}
