using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caracal.Common;
namespace Caracal.Data
{
    public class QueryBulider
    {
        private DProcType _processType;

        public DProcType ProcessType
        {
            get { return _processType; }
            set { _processType = value; }
        }

        //private DdlType _ddlType;

        //public DdlType DdlType
        //{
        //    get { return _ddlType; }
        //    set
        //    {
        //        switch (value)
        //        {
        //            case DdlType.SELECT:
        //            case DdlType.INSERT:
        //            case DdlType.UPDATE: _processType = DProcType.NONE; break;
        //        }
        //        _ddlType = value;
        //    }
        //}

        //private string _fields;

        //public string Fields
        //{
        //    get { return _fields; }
        //    set
        //    {
        //        _fields = value;
        //        if (_ddlType != DdlType.SELECT) _values = string.Empty;
        //    }
        //}

        private string _values;

        public string Values
        {
            get { return _values; }
            set { _values = value; }
        }



        public string GenerateQuery()
        {
            return string.Empty;
        }

    }

    public enum DProcType
    {
        CREATE, ALTER, DROP, NONE
    }
    //public enum DdlType
    //{
    //    SELECT, INSERT, UPDATE, VIEW,
    //    TABLE, TRIGGER, PROCEDURE, FUNCTION
    //}

    public enum DKeyword
    {
        WHERE, ORDER_BY, GROUP_BY, HAVING
    }
}
