using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
namespace Caracal.Common
{
    public interface IFilter
    {
        string Item { get; }
        string Value { get; }
        string Operator { get; }
        string ToString();
        FilterBindingType BindingType { get; }
        string GetFilterString();
    }
    public enum FilterBindingType
    {
        Must = 2, Optional = 1, None = 0
    }
}
