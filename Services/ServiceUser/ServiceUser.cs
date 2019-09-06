using Hotel.Data;
using Hotel.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class ServiceUser:IserviceUser
    {
        public void Add(User entity)
        {
            using (var ctx = new DatabContext())
            {
                ctx.Users.Add(entity);
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

        public void Delete(Expression<Func<User, bool>> where)
        {
            using (var ctx = new DatabContext())
            {
                IEnumerable<User> objects = ctx.Users.Where(where).AsEnumerable();
                foreach (User obj in objects)
                    ctx.Users.Remove(obj);
                ctx.SaveChanges();

            }
        }

        public void Delete(User entity)
        {
            using (var ctx = new DatabContext())
            {

                ctx.Users.Remove(entity);
                ctx.SaveChanges();

            }
        }

        public User Get(Expression<Func<User, bool>> where)
        {
            using (var ctx = new DatabContext())
            {

                return ctx.Users.Where(where).FirstOrDefault();


            }
        }

        public IEnumerable<User> GetAll()
        {
            using (var ctx = new DatabContext())
            {

                return ctx.Users;

            }
        }

        public User GetById(long id)
        {
            using (var ctx = new DatabContext())
            {

                return ctx.Users.Find(id);

            }
        }

        public User GetById(string id)
        {
            using (var ctx = new DatabContext())
            {

                return ctx.Users.Find(id);

            }
        }

        public IEnumerable<User> GetMany(Expression<Func<User, bool>> where = null, Expression<Func<User, bool>> orderBy = null)
        {
            IQueryable<User> Query;
            using (var ctx = new DatabContext())
            {

                Query = ctx.Users;
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


        public void Update(User entity)
        {
            using (var ctx = new DatabContext())
            {

                ctx.Users.Attach(entity);
                ctx.Entry(entity).State = EntityState.Modified;
                ctx.SaveChanges();

            }
        }

    }
}
