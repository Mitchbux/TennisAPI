using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisAPI.BusinessLayer
{
    public class PlayerData
    {
        public int id { get; set; }
        public int age { get; set; }
        public int height { get; set; }
        public int weight { get; set; }
        public int rank { get; set; }
        public IEnumerable<int> last { get; set; } = new List<int>();

    }
}
