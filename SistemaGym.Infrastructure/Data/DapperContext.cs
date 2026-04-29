using Dapper;
using SistemaGym.Core.Enum;
using SistemaGym.Core.Interfaces;
using System.Data;
using System.Data.Common;

namespace SistemaGym.Infrastructure.Data
{
    public class DapperContext : IDapperContext
    {
        private readonly IDbConnectionFactory _connFactory;

        private static readonly AsyncLocal<(
            IDbConnection? Conn, IDbTransaction? Tx)>
            _ambient = new();

        public DataBaseProvider Provider => _connFactory.Provider;

        public DapperContext(IDbConnectionFactory connFactory)
        {
            _connFactory = connFactory;
        }

        /// <summary>
        /// UnitOfWork llamará a este método al iniciar la transacción.
        /// </summary>
        public void SetAmbientConnection(
            IDbConnection conn,
            IDbTransaction? tx)
        {
            _ambient.Value = (conn, tx);
        }

        /// <summary>
        /// UnitOfWork llamará a este método cuando finalice o haga rollback.
        /// </summary>
        public void ClearAmbientConnection()
        {
            _ambient.Value = (null, null);
        }

        /// <summary>
        /// Si el UnitOfWork está activo, usa la misma conexión y transacción.
        /// Si no hay UnitOfWork, crea una conexión nueva desde IDbConnectionFactory.
        /// ownsConnection indica si la conexión debe cerrarse aquí.
        /// </summary>
        private (IDbConnection conn, IDbTransaction? tx, bool ownsConnection) GetConnAndTx()
        {
            var ambient = _ambient.Value;

            if (ambient.Conn != null)
            {
                return (ambient.Conn, ambient.Tx, false);
            }

            var conn = _connFactory.CreateConnection();

            return (conn, null, true);
        }

        /// <summary>
        /// Abre la conexión si está cerrada.
        /// </summary>
        private async Task OpenIfNeededAsync(IDbConnection conn)
        {
            if (conn is DbConnection dbConn &&
                dbConn.State == ConnectionState.Closed)
            {
                await dbConn.OpenAsync();
            }
        }

        public async Task<T?> QueryFirstOrDefaultAsync<T>(
            string sql,
            object? param = null,
            CommandType commandType = CommandType.Text)
        {
            var (conn, tx, owns) = GetConnAndTx();

            try
            {
                await OpenIfNeededAsync(conn);

                return await conn.QueryFirstOrDefaultAsync<T>(
                    new CommandDefinition(
                        sql,
                        param,
                        tx,
                        commandType: commandType));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (owns)
                {
                    if (conn is DbConnection dbConn &&
                        dbConn.State != ConnectionState.Closed)
                    {
                        await dbConn.CloseAsync();
                    }

                    conn.Dispose();
                }
            }
        }

        /// <summary>
        /// Ejecuta un SELECT que devuelve múltiples filas.
        /// </summary>
        public async Task<IEnumerable<T>> QueryAsync<T>(
            string sql,
            object? param = null,
            CommandType commandType = CommandType.Text)
        {
            var (conn, tx, owns) = GetConnAndTx();

            try
            {
                await OpenIfNeededAsync(conn);

                return await conn.QueryAsync<T>(
                    new CommandDefinition(
                        sql,
                        param,
                        tx,
                        commandType: commandType));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (owns)
                {
                    if (conn is DbConnection dbConn &&
                        dbConn.State != ConnectionState.Closed)
                    {
                        await dbConn.CloseAsync();
                    }

                    conn.Dispose();
                }
            }
        }

        /// <summary>
        /// Ejecuta comandos INSERT, UPDATE o DELETE.
        /// Devuelve el número de filas afectadas.
        /// </summary>
        public async Task<int> ExecuteAsync(
            string sql,
            object? param = null,
            CommandType commandType = CommandType.Text)
        {
            var (conn, tx, owns) = GetConnAndTx();

            try
            {
                await OpenIfNeededAsync(conn);

                return await conn.ExecuteAsync(
                    new CommandDefinition(
                        sql,
                        param,
                        tx,
                        commandType: commandType));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (owns)
                {
                    if (conn is DbConnection dbConn &&
                        dbConn.State != ConnectionState.Closed)
                    {
                        await dbConn.CloseAsync();
                    }

                    conn.Dispose();
                }
            }
        }

        /// <summary>
        /// Ejecuta un query y devuelve un valor escalar.
        /// Ejemplo: último ID insertado.
        /// </summary>
        public async Task<T> ExecuteScalarAsync<T>(
            string sql,
            object? param = null,
            CommandType commandType = CommandType.Text)
        {
            var (conn, tx, owns) = GetConnAndTx();

            try
            {
                await OpenIfNeededAsync(conn);

                var res = await conn.ExecuteScalarAsync(
                    new CommandDefinition(
                        sql,
                        param,
                        tx,
                        commandType: commandType));

                if (res == null || res == DBNull.Value)
                {
                    return default!;
                }

                return (T)Convert.ChangeType(res, typeof(T));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                if (owns)
                {
                    if (conn is DbConnection dbConn &&
                        dbConn.State != ConnectionState.Closed)
                    {
                        await dbConn.CloseAsync();
                    }

                    conn.Dispose();
                }
            }
        }
    }
}