using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Caracal.Data
{
    public class xParameter
    {
        public string ParameterName { get; set; }
        public object Value { get; set; }
        public DbType DbType { get; set; }

    }
    public enum DbType
    {
        BigInt = 0,
        Binary = 1,
        Bit = 2,
        Char = 3,
        Date = 31,
        DateTime = 4,
        DateTime2 = 33,
        DateTimeOffset = 34,
        Decimal = 5,
        Float = 6,
        Image = 7,
        Int = 8,
        Money = 9,
        NChar = 10,
        NText = 11,
        NVarChar = 12,
        Real = 13,
        SmallDateTime = 15,
        SmallInt = 16,
        SmallMoney = 17,
        Structured = 30,
        Text = 18,
        Time = 32,
        Timestamp = 19,
        TinyInt = 20,
        Udt = 29,
        UniqueIdentifier = 14,
        VarBinary = 21,
        VarChar = 22,
        Variant = 23,
        Xml = 25
    }
}
