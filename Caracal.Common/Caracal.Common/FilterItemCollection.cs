using System;
using System.Collections;
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
    public class FilterItemCollection<T> : ICollection<IFilter>
    {
        [DataMember]
        private ICollection<IFilter> _items;

        public delegate void FilterItemChangedEventHandler(IFilter item);
        public delegate void FilterItemsChangedEventHandler(IEnumerable<IFilter> items);
        public event FilterItemChangedEventHandler FilterItemAdded;
        public event FilterItemChangedEventHandler FilterItemRemoved;
        public event FilterItemsChangedEventHandler FilterCollectionChanged;
        public FilterItemCollection()
        {
            _items = new List<IFilter>();
        }
        public FilterItemCollection(Expression<Func<T, bool>> expression)
        {
            _items = _items ?? new List<IFilter>();
            List<IFilter> xFilters = expression.ExtractFilter().ToList();
            foreach (FilterBase filter in xFilters)
            {
                filter._value = filter._value.Replace('\"', '\'');
                if (!filter._value.IsNumber() && filter._value[0] != '\'')
                    filter._value = string.Concat("'", filter._value, "'");
            }
            ((List<IFilter>)_items).AddRange(xFilters);
        }
        [DataMember]
        public int Count
        {
            get
            {
                return _items.Count;
            }
            protected set { }
        }
        [DataMember]
        public bool IsReadOnly
        {
            get
            {
                return _items.IsReadOnly;
            }
            protected set { }
        }
        [OperationContract]
        public void Add(IFilter item)
        {
            _items.Add(item);
            FilterItemAdded?.Invoke(item);
            FilterCollectionChanged?.Invoke(new List<IFilter> { item });
        }
        [OperationContract]
        public void AddRange(IEnumerable<IFilter> items)
        {
            ((List<IFilter>)_items).AddRange(items);
            FilterCollectionChanged?.Invoke(items);
            FilterItemAdded?.Invoke(items.Last());
        }
        [OperationContract]
        public void Clear()
        {
            _items.Clear();
            FilterCollectionChanged?.Invoke(null);
        }
        [OperationContract]
        public bool Contains(IFilter item)
        {
            return _items.Contains(item);
        }
        [OperationContract]
        public void CopyTo(IFilter[] array, int arrayIndex)
        {
            _items.CopyTo(array, arrayIndex);
        }
        [OperationContract]
        public IEnumerator<IFilter> GetEnumerator()
        {
            return _items.GetEnumerator();
        }
        [OperationContract]
        public bool Remove(IFilter item)
        {
            bool returnValue = _items.Remove(item);
            FilterItemRemoved?.Invoke(item);
            FilterCollectionChanged?.Invoke(new List<IFilter> { item });
            return returnValue;
        }
        IEnumerator IEnumerable.GetEnumerator()
        {
            return _items.GetEnumerator();
        }
        [OperationContract]
        public string GetFilterString()
        {
            string filterString = string.Empty;
            FilterItem<FilterBase> xItem = null;
            foreach (var item in _items)
            {
                //if (item != _items.Last() && item.BindingType == FilterBindingType.None)
                //{
                //    if (xItem != null)
                //        item.(FilterBindingType.Optional);
                //    else
                //    {
                //        xItem = (FilterBase)item;
                //        continue;
                //    }
                //}
                filterString = string.Concat(filterString, " ", item.GetFilterString());
            }
            if (xItem != null) filterString = string.Concat(filterString, " ", xItem.GetFilterString());
            return filterString;
        }
    }
}
