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
    public class TableJoin<Master, Detail>
    {
        [DataMember]
        private ICollection<IFilter> _items;
        [DataMember]
        protected Expression<Func<Master, Detail, bool>> _expression;
        [DataMember]
        protected JoinType _joinType;


        public TableJoin(Expression<Func<Master, Detail, bool>> expression, JoinType jType = JoinType.InnerJoin)
        {
            this._items = this._items ?? new List<IFilter>();
            this._expression = expression;
            this._joinType = jType;
            ((List<IFilter>)this._items).AddRange(expression.ExtractFilter().ToList());
        }

        public enum JoinType
        {
            [EnumMember]
            InnerJoin,
            [EnumMember]
            LeftOuterJoin,
            [EnumMember]
            RightOuterJoin,
            [EnumMember]
            FullOuterJoin
        }
        [OperationContract]
        public string GetJoinString()
        {
            string joinString = string.Empty;
            FilterItem<FilterBase> xItem = null;
            foreach (var item in this._items)
            {
                joinString = string.Concat(joinString, " ", item.GetFilterString());
            }
            ITable DetailType = (ITable)Activator.CreateInstance<Detail>();
            if (xItem != null) joinString = string.Join(" ", this._joinType, DetailType.TableName, "ON", joinString, xItem.GetFilterString());
            return joinString;
        }
    }
}
