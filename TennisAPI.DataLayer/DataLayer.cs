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
        public DataTable Query(string sql)
        {
            // Implementation of the Query method would go here
            throw new NotImplementedException("This method needs to be implemented.");
        }

    }
}
