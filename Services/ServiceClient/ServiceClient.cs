using Hotel.Data;
using Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.ServiceClient
{
    public class ServiceClient:IserviceClient
    {
        public void Add(Client entity)
        {
            using (var ctx = new DatabContext())
            {
                ctx.Clients.Add(entity);
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

        public void Delete(Expression<Func<Client, bool>> where)
        {
            using (var ctx = new DatabContext())
            {
                IEnumerable<Client> objects = ctx.Clients.Where(where).AsEnumerable();
                foreach (Client obj in objects)
                    ctx.Clients.Remove(obj);
                ctx.SaveChanges();

            }
        }

        public void Delete(Client entity)
        {
            using (var ctx = new DatabContext())
            {

                ctx.Clients.Remove(entity);
                ctx.SaveChanges();

            }
        }

        public Client Get(Expression<Func<Client, bool>> where)
        {
            using (var ctx = new DatabContext())
            {

                return ctx.Clients.Where(where).FirstOrDefault();


            }
        }

        public IEnumerable<Client> GetAll()
        {
            using (var ctx = new DatabContext())
            {

                return ctx.Clients.AsEnumerable();

            }
        }

        public Client GetById(long id)
        {
            using (var ctx = new DatabContext())
            {

                return ctx.Clients.Find(id);

            }
        }

        public Client GetById(string id)
        {
            using (var ctx = new DatabContext())
            {

                return ctx.Clients.Find(id);

            }
        }

        public IEnumerable<Client> GetMany(Expression<Func<Client, bool>> where = null, Expression<Func<Client, bool>> orderBy = null)
        {
            IQueryable<Client> Query;
            using (var ctx = new DatabContext())
            {

                Query = ctx.Clients;
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


        public void Update(Client entity)
        {
            using (var ctx = new DatabContext())
            {

                ctx.Clients.Attach(entity);
                ctx.Entry(entity).State = EntityState.Modified;
                ctx.SaveChanges();

            }
        }
    }
}
