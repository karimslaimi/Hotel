using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.ServiceRevenu
{
    public interface IserviceRevenu
    {

        void Add(Revenu entity);
        void Delete(Expression<Func<Revenu, bool>> where);
        void Delete(Revenu entity);
        Revenu Get(Expression<Func<Revenu, bool>> where);
        IEnumerable<Revenu> GetAll();
        Revenu GetById(long id);
        Revenu GetById(string id);
        IEnumerable<Revenu> GetMany(Expression<Func<Revenu, bool>> where = null, Expression<Func<Revenu, bool>> orderBy = null);

        void Update(Revenu entity);

        void Commit();


    }
}
