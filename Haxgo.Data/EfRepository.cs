using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using Haxgo.Entities;

namespace Haxgo.Data
{
    public class EfRepository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly DbContext _context;
        private readonly IDbSet<T> _entities;

        public EfRepository(HaxgoContext context)
        {
            _context = context;
            _entities = context.Set<T>();
        }

        #region IRepository<T> 成员

        public T GetById(object id)
        {
            return this._entities.Find(id);
        }

        public void Create(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                _entities.Add(entity);
                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                string msg = string.Empty;
                foreach (DbEntityValidationResult dbevr in dbEx.EntityValidationErrors)
                {
                    foreach (DbValidationError dbve in dbevr.ValidationErrors)
                    {
                        msg = msg + string.Format("Property: {0} - Error: {1}", dbve.PropertyName, dbve.ErrorMessage) + Environment.NewLine;
                    }
                }
                Exception fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        public void Update(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                string msg = string.Empty;
                foreach (DbEntityValidationResult dbevr in dbEx.EntityValidationErrors)
                {
                    foreach (DbValidationError dbve in dbevr.ValidationErrors)
                    {
                        msg = msg + string.Format("Property: {0} - Error: {1}", dbve.PropertyName, dbve.ErrorMessage) + Environment.NewLine;
                    }
                }
                Exception fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        public void Delete(T entity)
        {
            try
            {
                if (entity == null)
                {
                    throw new ArgumentNullException("entity");
                }
                _entities.Remove(entity);
                _context.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                string msg = string.Empty;
                foreach (DbEntityValidationResult dbevr in dbEx.EntityValidationErrors)
                {
                    foreach (DbValidationError dbve in dbevr.ValidationErrors)
                    {
                        msg = msg + string.Format("Property: {0} - Error: {1}", dbve.PropertyName, dbve.ErrorMessage) + Environment.NewLine;
                    }
                }
                Exception fail = new Exception(msg, dbEx);
                throw fail;
            }
        }

        public virtual IQueryable<T> Table
        {
            get { return _entities; }
        }

        #endregion
    }
}
