using Hotel.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Services.ServiceReservation
{
    public interface IserviceReservation
    {
        void Add(Reservation entity);
        void Delete(Expression<Func<Reservation, bool>> where);
        void Delete(Reservation entity);
        Reservation Get(Expression<Func<Reservation, bool>> where);
        IEnumerable<Reservation> GetAll();
        Reservation GetById(long id);
        Reservation GetById(string id);
        IEnumerable<Reservation> GetMany(Expression<Func<Reservation, bool>> where = null, Expression<Func<Reservation, bool>> orderBy = null);

        void Update(Reservation entity);

        void Commit();
    }
}
