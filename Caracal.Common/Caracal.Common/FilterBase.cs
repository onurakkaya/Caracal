using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Caracal.Common
{
    [DataContract]
    public class FilterBase : IFilter
    {
        [DataMember]
        protected internal string _item;
        [DataMember]
        protected internal string _value;
        [DataMember]
        protected internal string _operator;
        [DataMember]
        protected internal FilterBindingType _bindingType = FilterBindingType.None;
        public FilterBindingType BindingType
        {
            get
            {
                return _bindingType;
            }
            protected set { }
        }
        public string Item
        {
            get
            {
                return _item;
            }
            protected set { }
        }
        public string Operator
        {
            get
            {
                return _operator;
            }
            protected set { }
        }
        public string Value
        {
            get
            {
                return _value;
            }
            protected set { }
        }
        [OperationContract]
        public string GetFilterString()
        {
            string joinOperand = string.Empty;
            switch (_bindingType)
            {
                case FilterBindingType.Must:
                    joinOperand = "AND";
                    break;
                case FilterBindingType.Optional:
                    joinOperand = "OR";
                    break;
                case FilterBindingType.None:
                default:
                    joinOperand = string.Empty;
                    break;
            }
            return string.Join(" ", new[] { _item, _operator, _value, joinOperand });
        }
        [OperationContract]
        public void SetFilter(string item, string operand, string value, FilterBindingType bindingType)
        {
            _item = item;
            _operator = operand;
            _value = value;
            _bindingType = bindingType;
        }
    }
}
