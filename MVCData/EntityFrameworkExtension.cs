using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;


namespace MVCData
{
    public static class EntityFrameworkExtension
    {
        public static IQueryable<TEntity> IncludeEntity<TEntity>(this IQueryable<TEntity> query, params Expression<Func<TEntity, object>>[] includes) where TEntity : class
        {
            if (includes != null)
            {
                query = includes.Aggregate(query,
                          (current, include) => current.Include(include));
            }

            return query;
        }

        public static IQueryable<TEntity> IncludeEntity<TEntity>(this IDbSet<TEntity> dbSet, params Expression<Func<TEntity, object>>[] includes) where TEntity : class
        {
            IQueryable<TEntity> query = null;
            foreach (var include in includes)
            {
                query = dbSet.IncludeEntity(include);
            }

            return query == null ? dbSet : query;
        }


        /// <summary>
        /// Check stored procedure exist
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="storedProcedureName"></param>
        /// <returns></returns>
        public static bool StoredProcedureExists(this DbContext dbContext, string storedProcedureName)
        {
            var query = dbContext.Database.SqlQuery(typeof(int), string.Format("SELECT COUNT(*) FROM [sys].[objects] WHERE [type_desc] = 'SQL_STORED_PROCEDURE' AND [name] = '{0}';", storedProcedureName), new object[] { });

            int exists = query.Cast<int>().Single();

            return (exists > 0);
        }

        /// <summary>
        /// Check user defined function exist
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="userDefinedFunctionName"></param>
        /// <returns></returns>
        public static bool UserDefinedFunctionExists(this DbContext dbContext, string userDefinedFunctionName)
        {
            var query = dbContext.Database.SqlQuery(typeof(int), string.Format("SELECT COUNT(*) FROM [sys].[objects] WHERE TYPE in ('FN', 'IF', 'TF') AND [name] = '{0}';", userDefinedFunctionName), new object[] { });

            int exists = query.Cast<int>().Single();

            return (exists > 0);
        }


        /// <summary>
        /// Create a new stored procedure
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="queryString"></param>
        public static void CreateStoredProcedure(this DbContext dbContext, string storedProcedureName, string queryString)
        {
            if (dbContext.StoredProcedureExists(storedProcedureName)) dbContext.Database.ExecuteSqlCommand(@"DROP PROCEDURE " + storedProcedureName);

            dbContext.Database.ExecuteSqlCommand(@"CREATE PROC " + storedProcedureName + "\r\n" + queryString);
        }

        /// <summary>
        /// Create a new user defined function
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="userDefinedFunctionName"></param>
        /// <param name="queryString"></param>
        public static void CreateUserDefinedFunction(this DbContext dbContext, string userDefinedFunctionName, string queryString)
        {
            if (dbContext.UserDefinedFunctionExists(userDefinedFunctionName)) dbContext.Database.ExecuteSqlCommand(@"DROP FUNCTION " + userDefinedFunctionName);

            dbContext.Database.ExecuteSqlCommand(@"CREATE FUNCTION " + userDefinedFunctionName + "\r\n" + queryString);
        }



        /// <summary>
        /// Create new stored procedure to check a pecific existing, for example: Editable, Revisable, ...
        /// </summary>
        /// <param name="storedProcedureName"></param>
        /// <param name="parameterString"></param>
        /// <param name="queryArray"></param>
        public static void CreateProcedureToCheckExisting(this DbContext dbContext, string storedProcedureName, string[] queryArray)
        {
            string queryString = "";

            queryString = " @EntityID int " + "\r\n";
            queryString = queryString + " WITH ENCRYPTION " + "\r\n";
            queryString = queryString + " AS " + "\r\n";
            queryString = queryString + "   BEGIN " + "\r\n";

            queryString = queryString + "       DECLARE @ExistIdentityID Int " + "\r\n";
            queryString = queryString + "       SET @ExistIdentityID = -1 " + "\r\n";

            if (queryArray != null)
            {
                foreach (string subQuery in queryArray)
                {
                    queryString = queryString + "       DECLARE CursorLocal CURSOR LOCAL FOR " + subQuery + "\r\n";
                    queryString = queryString + "       OPEN CursorLocal " + "\r\n";
                    queryString = queryString + "       FETCH NEXT FROM CursorLocal INTO @ExistIdentityID " + "\r\n";
                    queryString = queryString + "       CLOSE CursorLocal " + "\r\n";
                    queryString = queryString + "       DEALLOCATE CursorLocal " + "\r\n";
                    queryString = queryString + "       IF @ExistIdentityID != -1  RETURN @ExistIdentityID " + "\r\n";
                }
            }

            queryString = queryString + "       RETURN @ExistIdentityID " + "\r\n";
            queryString = queryString + "   END " + "\r\n";

            dbContext.CreateStoredProcedure(storedProcedureName, queryString);
        }

        public static void CreateProcedureToCheckExisting(this DbContext dbContext, string storedProcedureName)
        {
            dbContext.CreateProcedureToCheckExisting(storedProcedureName, null);
        }



    }
}
