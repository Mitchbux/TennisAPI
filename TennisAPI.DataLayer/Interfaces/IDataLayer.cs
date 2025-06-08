using System.Data;

namespace TennisAPI.DataLayer
{
    public interface IDataLayer
    {
        public string ConnectionString { get; set; }
        public DataTable Query(string sql, params P[]? commandParams);

    }
}
