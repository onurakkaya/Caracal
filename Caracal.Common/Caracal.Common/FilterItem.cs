using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Caracal.Common
{
    [DataContract]
    public class FilterItem<T> : IFilter
    {
        public FilterItem(Expression<Func<T, bool>> expr, FilterBindingType bindingType)
        {
            _bindingType = bindingType;
            List<string> x = new List<string>();
            List<IFilter> xFilters = expr.ExtractFilter().ToList();
            if (xFilters.Count > 0)
            {
                IFilter filterItem = xFilters.First();
                _item = string.Concat("[", filterItem.Item, "]");
                _value = filterItem.Value.Replace('\"', '\'');
                _operator = filterItem.Operator;
                _bindingType = filterItem.BindingType;
            }
        }
        private string _item;
        private string _value;
        private string _operator;
        private FilterBindingType _bindingType;
        [DataMember]
        public string Item
        {
            get
            {
                return _item;
            }
            protected set { }
        }
        [DataMember]
        public string Value
        {
            get
            {
                return _value;
            }
            protected set { }
        }
        [DataMember]
        public string Operator
        {
            get
            {
                return _operator;
            }
            protected set { }
        }
        [DataMember]
        public FilterBindingType BindingType
        {
            get
            {
                return _bindingType;
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
        public override string ToString()
        {
            return GetFilterString();
        }
        [OperationContract]
        public static FilterItem<T> GetFilter(Expression<Func<T, bool>> expr, FilterBindingType bindingType)
        {
            return new FilterItem<T>(expr, bindingType);
        }
        protected internal void ChangeBindingType(FilterBindingType newType)
        {
            _bindingType = newType;
        }
    }
}
