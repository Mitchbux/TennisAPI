using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisAPI.BusinessLayer
{
    public interface IPlayerDepot
    {
        public IEnumerable<Player> GetAll();    
        public Player? GetById(int id);
        public Statistics GetStatistics();
    }
}
