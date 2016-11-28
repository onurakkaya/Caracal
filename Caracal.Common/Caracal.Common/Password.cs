using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caracal.Common
{
    public class Password
    {

        private string _value = string.Empty;

        public string GetDecryptedValue()
        {
            return string.Empty;
        }

        public void SetValue(string value)
        {
            _value = value;
        }
        public override string ToString()
        {
            return _value;
        }
    }
}
