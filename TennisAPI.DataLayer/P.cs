using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TennisAPI.DataLayer
{
    public class P
    {
        public string Key { get; set; }
        public object? Value { get; set; }   
        public P(string k, object? v) 
        {
            Key = k;
            Value = v;
        }
    }
}
