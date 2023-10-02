using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HammingCodeSimulator
{
    public static class MyExtensions
    {
        public static string MakeString(this bool val)
        {
            if (val)
                return "1";
            else return "0";
        }
    }
}
