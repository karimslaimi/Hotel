using Hotel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.ServiceDepence
{
    public interface IserviceDepenses
    {
        void Add(Depenses entity);
        void Delete(Expression<Func<Depenses, bool>> where);
        void Delete(Depenses entity);
        Depenses Get(Expression<Func<Depenses, bool>> where);
        IEnumerable<Depenses> GetAll();
        Depenses GetById(long id);
        Depenses GetById(string id);
        IEnumerable<Depenses> GetMany(Expression<Func<Depenses, bool>> where = null, Expression<Func<Depenses, bool>> orderBy = null);

        void Update(Depenses entity);

        void Commit();
    }
}
