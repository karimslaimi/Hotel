using Hotel.Data;
using Hotel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.ServiceReservation
{
    public class ServiceReservation:IserviceReservation
    {
        public void Add(Reservation entity)
        {
            using (var ctx = new DatabContext())
            {
                ctx.Reservations.Add(entity);
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

        public void Delete(Expression<Func<Reservation, bool>> where)
        {
            using (var ctx = new DatabContext())
            {
                IEnumerable<Reservation> objects = ctx.Reservations.Where(where).AsEnumerable();
                foreach (Reservation obj in objects)
                    ctx.Reservations.Remove(obj);
                ctx.SaveChanges();

            }
        }

        public void Delete(Reservation entity)
        {
            using (var ctx = new DatabContext())
            {

                ctx.Reservations.Remove(entity);
                ctx.SaveChanges();

            }
        }

        public Reservation Get(Expression<Func<Reservation, bool>> where)
        {
            using (var ctx = new DatabContext())
            {

                return ctx.Reservations.Where(where).Include(h=>h.Clients).FirstOrDefault();


            }
        }


        public IEnumerable<Reservation> GetAll()
        {
            using (var ctx = new DatabContext())
            {

                return ctx.Reservations;

            }
        }

        public Reservation GetById(long id)
        {
            using (var ctx = new DatabContext())
            {

                return ctx.Reservations.Find(id);

            }
        }

        public Reservation GetById(string id)
        {
            using (var ctx = new DatabContext())
            {

                return ctx.Reservations.Find(id);

            }
        }

        public IEnumerable<Reservation> GetMany(Expression<Func<Reservation, bool>> where = null, Expression<Func<Reservation, bool>> orderBy = null)
        {
            IQueryable<Reservation> Query;
            using (var ctx = new DatabContext())
            {

                Query = ctx.Reservations.Include(x=>x.Clients);
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


        public void Update(Reservation entity)
        {
            using (var ctx = new DatabContext())
            {

                ctx.Reservations.Attach(entity);
                ctx.Entry(entity).State = EntityState.Modified;
                ctx.SaveChanges();

            }
        }

    }
}

