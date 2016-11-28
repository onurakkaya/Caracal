using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caracal.Common
{
    public static class IntExtensions
    {
        public static bool Between(this int baseVal, int min, int max)
        {
            return (baseVal >= min && baseVal <= max);
        }
    }
}
