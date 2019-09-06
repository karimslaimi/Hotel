using Hotel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IserviceUser
    {
        void Add(User entity);
        void Delete(Expression<Func<User, bool>> where);
        void Delete(User entity);
        User Get(Expression<Func<User, bool>> where);
        IEnumerable<User> GetAll();
        User GetById(long id);
        User GetById(string id);
        IEnumerable<User> GetMany(Expression<Func<User, bool>> where = null, Expression<Func<User, bool>> orderBy = null);

        void Update(User entity);

        void Commit();
    }
}
