namespace Task.Schedu.Data
{
    using System;
    using System.Collections.Generic;
    using MySql.Data.MySqlClient;
    using Dapper;
    using DapperExtensions;
    using DapperExtensions.Mapper;
    using DapperExtensions.Sql;
    using System.Data;
    using System.Data.SqlClient;
    using System.Reflection;
    /// <summary>
    /// 数据库类型
    /// </summary>
    public enum DbType
    {
        /// <summary>
        /// MySqlClient
        /// </summary>
        SqlServer,
        /// <summary>
        /// SqlClient
        /// </summary>
        MySql
    }
    public class DbAccess<T>
    {

        #region Dapper.Ext
        /// <summary>
        /// Void
        /// </summary>
        /// <param name="action"></param>
        /// <param name="dbConnect"></param>
        /// <param name="dbType"></param>
        protected void VoidExt(Action<IDatabase> action, string dbConnect, DbType dbType = DbType.MySql)
        {
            using (var client = DbFactory.CreateDb(dbType, dbConnect))
            {
                action(client);
            }
        }

        /// <summary>
        /// FindBy
        /// </summary>
        /// <param name="func"></param>
        /// <param name="dbConnect"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        protected IEnumerable<T> FindByExt(Func<IDatabase, IEnumerable<T>> func, string dbConnect, DbType dbType = DbType.MySql)
        {
            using (var client = DbFactory.CreateDb(dbType, dbConnect))
            {
                return func(client);
            }
        }
        /// <summary>
        /// FirstOrDefault
        /// </summary>
        /// <param name="func"></param>
        /// <param name="dbConnect"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        protected T FirstOrDefaultExt(Func<IDatabase, T> func, string dbConnect, DbType dbType = DbType.MySql)
        {
            using (var client = DbFactory.CreateDb(dbType, dbConnect))
            {
                return func(client);
            }
        }
        /// <summary>
        /// Insert、Update、Delete
        /// </summary>
        /// <param name="func"></param>
        /// <param name="dbConnect"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        protected bool CommitExt(Func<IDatabase, bool> func, string dbConnect, DbType dbType = DbType.MySql)
        {
            using (var client = DbFactory.CreateDb(dbType, dbConnect))
            {
                return func(client);
            }
        }
        #endregion

        #region Dapper
        /// <summary>
        /// Void
        /// </summary>
        /// <param name="action"></param>
        /// <param name="dbConnect"></param>
        /// <param name="dbType"></param>
        protected void Void(Action<IDbConnection> action, string dbConnect, DbType dbType= DbType.MySql)
        {
            using (var client = DbFactory.MakeDb(dbType, dbConnect))
            {
                action(client);
            }
        }

        /// <summary>
        /// FindBy
        /// </summary>
        /// <param name="func"></param>
        /// <param name="dbConnect"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        protected IEnumerable<T> FindBy(Func<IDbConnection, IEnumerable<T>> func, string dbConnect, DbType dbType = DbType.MySql)
        {
            using (var client = DbFactory.MakeDb(dbType, dbConnect))
            {
                return func(client);
            }
        }
        /// <summary>
        /// FirstOrDefault
        /// </summary>
        /// <param name="func"></param>
        /// <param name="dbConnect"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        protected T FirstOrDefault(Func<IDbConnection, T> func, string dbConnect, DbType dbType = DbType.MySql)
        {
            using (var client = DbFactory.MakeDb(dbType, dbConnect))
            {
                return func(client);
            }
        }
        /// <summary>
        /// Insert、Update、Delete
        /// </summary>
        /// <param name="func"></param>
        /// <param name="dbConnect"></param>
        /// <param name="dbType"></param>
        /// <returns></returns>
        protected bool Commit(Func<IDbConnection, bool> func, string dbConnect, DbType dbType = DbType.MySql)
        {
            using (var client = DbFactory.MakeDb(dbType, dbConnect))
            {
                return func(client);
            }
        }
        #endregion
    }

    /// <summary>
    /// 
    /// </summary>
    public static class DbFactory
    {
        /// <summary>
        /// Dapper.Ext
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="dbConnect"></param>
        /// <returns></returns>
        public static IDatabase CreateDb(DbType dbType, string dbConnect)
        {
            IDbConnection connection = null;
            IDatabase dbDatabase = null;
            var config = new DapperExtensionsConfiguration();
            switch (dbType)
            {
                case DbType.SqlServer:
                    connection = new SqlConnection(dbConnect);
                    config = new DapperExtensionsConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(),
                        new SqlServerDialect());

                    break;
                case DbType.MySql:
                    connection = new MySqlConnection(dbConnect);
                    config = new DapperExtensionsConfiguration(typeof(AutoClassMapper<>), new List<Assembly>(),
                        new MySqlDialect());
                    break;
            }
            var sqlGenerator = new SqlGeneratorImpl(config);
            dbDatabase = new Database(connection, sqlGenerator);
            return dbDatabase;
        }
        /// <summary>
        /// Dapper
        /// </summary>
        /// <param name="dbType"></param>
        /// <param name="dbConnect"></param>
        /// <returns></returns>
        public static IDbConnection MakeDb(DbType dbType,string dbConnect)
        {
            IDbConnection connection = null;
            switch (dbType)
            {
                case DbType.SqlServer:
                    connection = new SqlConnection(dbConnect);
                    break;
                case DbType.MySql:
                    connection = new MySqlConnection(dbConnect);
                    break;
            }
            return connection;
        }
    }
}
