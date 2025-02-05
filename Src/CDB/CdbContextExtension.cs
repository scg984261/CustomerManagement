using Microsoft.EntityFrameworkCore;

namespace CDB
{
    public partial class CdbContext
    {
        public virtual IQueryable<TEntity> RunSql<TEntity>(string sql, params object[] parameters) where TEntity : class
        {
            return this.Set<TEntity>().FromSqlRaw(sql, parameters);
        }
    }
}
