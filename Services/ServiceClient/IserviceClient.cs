using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.ServiceClient
{
    public interface IserviceClient
    {

        void Add(Client entity);
        void Delete(Expression<Func<Client, bool>> where);
        void Delete(Client entity);
        Client Get(Expression<Func<Client, bool>> where);
        IEnumerable<Client> GetAll();
        Client GetById(long id);
        Client GetById(string id);
        IEnumerable<Client> GetMany(Expression<Func<Client, bool>> where = null, Expression<Func<Client, bool>> orderBy = null);

        void Update(Client entity);

        void Commit();
    }
}
