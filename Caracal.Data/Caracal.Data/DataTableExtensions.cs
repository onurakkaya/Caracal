using System.Collections.Generic;
using System.Linq;
using System.Data;
using System;
using System.Reflection;

namespace Caracal.Data
{
    public static class DataTableExtensions
    {
        public static List<DataRow> ToRowList(this DataTable table)
        {
            return table.Rows.Cast<DataRow>().ToList();
        }

        public static List<DataColumn> ToColumnList(this DataTable table)
        {
            return table.Columns.Cast<DataColumn>().ToList();
        }

        public static List<T> ToList<T>(this DataTable table)
        {
            List<T> listT = new List<T>();
            Type typeT = typeof(T);
            List<PropertyInfo> properties = typeT.GetProperties().ToList();
            table.ToRowList().ForEach(x =>
            {
                T variableT = Activator.CreateInstance<T>();
                properties.ForEach(y =>
                {
                    if (table.ToColumnList().Any(z => z.ColumnName == y.Name))
                        y.SetValue(variableT, y.PropertyType == typeof(Enum) ? Enum.Parse(y.PropertyType, x[y.Name].ToString()) : CheckData(x[y.Name], y.PropertyType));
                });
                listT.Add(variableT);
            });
            return listT;
        }
        private static object CheckData(object value, Type type)
        {
            if (value == null) return Activator.CreateInstance(type);

            if (value.Equals(DBNull.Value)) try { return Activator.CreateInstance(type); } catch (MissingMethodException) { return string.Empty; }

            try { return Convert.ChangeType(value, type); }
            catch (InvalidCastException) { throw new Exception(string.Format("Patladı Gitti CheckForNull : {0}", value)); }
        }
    }
}
