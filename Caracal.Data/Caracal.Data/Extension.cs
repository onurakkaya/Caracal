using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Caracal.Common;
using System.Reflection;
using System.Windows.Forms;
namespace Caracal.Data
{
    public static class Extension
    {

        public static string ToSql(this ITable tableClass, DdlType type, IList<string> fields = null, string condition = default(string))
        {
            string pJoinString = string.Empty;
            if (tableClass.IsJoinRequired)
                pJoinString = tableClass.JoinCondition;
            List<PropertyInfo> tableProperties = tableClass.GetType().GetProperties().ToList();
            List<PropertyInfo> xp = new List<PropertyInfo>();
            if (fields != null)
                tableProperties.ForEach(x => { if (fields.Where(y => y == x.Name).Count() > 0) xp.Add(x); });
            else
                xp = tableProperties;

            xp.Remove(xp.Any(x => x.Name == "TableName") ? xp.First(x => x.Name == "TableName") : null);
            xp.Remove(xp.Any(x => x.Name == "Id" && type != DdlType.Select) ? xp.First(x => x.Name == "Id") : null);
            List<string> xFields = new List<string>();
            xp.ForEach(x => xFields.Add(string.Format("[{0}]", x.Name)));
            string xSelector = "SELECT ";
            List<string> xConnector = new List<string>();
            foreach (var item in xp)
            {
                var itemValue = item.GetValue(tableClass);
                itemValue = itemValue ?? "NULL";
                Type itemType = itemValue.GetType();

                if ((itemType == typeof(DateTime) && ((DateTime)itemValue) == default(DateTime).CheckForSql()) ||
                    (itemType.IsValueType && itemValue == Activator.CreateInstance(itemType))) itemValue = "NULL";
                else if (itemType == typeof(bool)) itemValue = (bool)itemValue == false ? 0 : 1;
                else if (itemType == typeof(DateTime)) itemValue = ((DateTime)itemValue).ToSqlString();

                bool isNumber = itemValue.ToString().IsNumber() && (itemType == typeof(int) && itemType == typeof(bool));
                string valueText = isNumber || itemValue.ToString().ToUpper() == "NULL" ? itemValue.ToString() : string.Concat("'", itemValue, "'");

                if (type == DdlType.Insert)
                    xConnector.Add(string.Format("{0} AS [{1}]", valueText, item.Name));
                else if (type == DdlType.Update)
                    xConnector.Add(string.Format("[{0}] = {1}", item.Name, valueText));
            }

            string iSelectSql = type == DdlType.Insert ? xSelector + string.Join(",", xConnector) : string.Join(",", xConnector);

            string cFields = string.Join(",", xFields);

            string baseSql = string.Empty;

            switch (type)
            {
                case DdlType.Select:
                    baseSql = string.Format("SELECT {0} FROM [{1}] {2} {2}",
                    cFields,
                    tableClass.TableName,
                    pJoinString,
                    string.IsNullOrEmpty(condition) ? string.Empty : "WHERE" + condition);
                    break;
                case DdlType.Insert:
                    baseSql = string.Format("INSERT INTO [{0}] ({1}) \n{2}",
                        tableClass.TableName,
                        cFields,
                        iSelectSql);
                    break;
                case DdlType.Update:
                    baseSql = string.Format("UPDATE [{0}] SET {1} {2}", tableClass.TableName, iSelectSql, string.IsNullOrEmpty(condition) ? string.Empty : "WHERE" + condition);
                    break;
                case DdlType.Delete:
                    baseSql = string.Format("DELETE FROM [{0}] {1}", tableClass.TableName, string.IsNullOrEmpty(condition) ? string.Empty : "WHERE" + condition);
                    break;
                default:
                    break;
            }
            return baseSql;
        }
    }

    public enum DdlType
    {
        Select = 0,
        Insert = 1,
        Update = 2,
        Delete = 3
    }
}
