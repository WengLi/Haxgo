using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Haxgo.Entities;

namespace Haxgo.Data
{
    public interface IRepository<T> where T : BaseEntity
    {
        T GetById(object id);
        void Create(T entity);
        void Update(T entity);
        void Delete(T entity);
        IQueryable<T> Table { get; }
    }
}
