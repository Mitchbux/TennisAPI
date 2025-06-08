using Npgsql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisAPI.DataLayer
{
    public class DataLayer : IDataLayer
    {
        public string ConnectionString { get; set; } = string.Empty;
        public DataLayer(string _connection)
        {
            ConnectionString = _connection ?? throw new ArgumentNullException(nameof(_connection), "La chaîne de connexion ne peut pas être null.");
        }
        public DataTable Query(string sql, params P[]? commandParams)
        {
            var data = new DataTable();
            using (var cnx = new Npgsql.NpgsqlConnection(ConnectionString))
            {
                cnx.Open();
                var command = new NpgsqlDataAdapter(sql, cnx);
                var parameters = command?.SelectCommand?.Parameters;
                if (parameters != null)
                    foreach (var param in commandParams ?? Array.Empty<P>())
                    {
                        parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                    }
                if (command != null)
                {
                    command.Fill(data);
                }
                cnx.Close();
            }
            return data;
        }

    }
}
