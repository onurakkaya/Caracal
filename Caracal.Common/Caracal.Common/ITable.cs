using Caracal.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caracal.Common
{
    public interface ITable
    {
        string TableName { get; set; }
        bool IsJoinRequired { get; set; }
        string JoinCondition { get; set; }
    }
}
