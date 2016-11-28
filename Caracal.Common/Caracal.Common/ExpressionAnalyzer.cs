using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Caracal.Common
{
    public static class ExpressionAnalyzer
    {
        public static IEnumerable<MemberExpression> GetMemberExpressions(this Expression body)
        {
            var candidates = new Queue<Expression>(new[] { body });
            while (candidates.Count > 0)
            {
                var expr = candidates.Dequeue();
                if (expr is MemberExpression) yield return ((MemberExpression)expr);
                else if (expr is UnaryExpression) candidates.Enqueue(((UnaryExpression)expr).Operand);
                else if (expr is BinaryExpression)
                {
                    var binary = expr as BinaryExpression;
                    candidates.Enqueue(binary.Left);
                    candidates.Enqueue(binary.Right);
                }
                else if (expr is MethodCallExpression)
                {
                    var method = expr as MethodCallExpression;
                    foreach (var arg in method.Arguments)
                        candidates.Enqueue(arg);
                }
                else if (expr is LambdaExpression)
                    candidates.Enqueue(((LambdaExpression)expr).Body);
            }
        }
        public static IEnumerable<IFilter> ExtractFilter(this Expression body)
        {
            List<FilterBase> filters = new List<FilterBase>();
            FilterBase filterBuffer = new FilterBase();

            Queue<string> operators = new Queue<string>();
            var candidates = new Queue<Expression>(new[] { body });
            while (candidates.Count > 0)
            {
                var expression = candidates.Dequeue();
                if (expression is LambdaExpression)
                    candidates.Enqueue(((LambdaExpression)expression).Body);
                else if (expression is BinaryExpression)
                {
                    BinaryExpression bExpr = expression as BinaryExpression;

                    string operatorString = bExpr.NodeType.GetOperatorString();

                    if (operatorString.Contains("AND"))
                        operators.Enqueue(FilterBindingType.Must.ToString());
                    else if (operatorString.Contains("OR"))
                        operators.Enqueue(FilterBindingType.Optional.ToString());
                    else
                        operators.Enqueue(FilterBindingType.None.ToString());

                    if (!(bExpr.Left is BinaryExpression) && (!(bExpr.Right is BinaryExpression)))
                    {

                        string itemName = bExpr.Left is MemberExpression ? ((MemberExpression)bExpr.Left).Member.Name : bExpr.Left.ToString();
                        string itemValue = Expression.Lambda(bExpr.Right).Compile().DynamicInvoke().ToString().Replace('\'', '´');

                        if (string.IsNullOrEmpty(itemValue.ToString()))
                        {
                            Type leftHandObjType = bExpr.Left.Type;
                            if (leftHandObjType == typeof(string) || leftHandObjType == typeof(DateTime)) itemValue = "''";
                        }
                        FilterBindingType bindingType = (FilterBindingType)Enum.Parse(typeof(FilterBindingType), operators.Dequeue());
                        filterBuffer.SetFilter(itemName, bExpr.NodeType.GetOperatorString(), itemValue, bindingType);
                        filters.Add(filterBuffer);
                        filterBuffer = new FilterBase();
                    }
                    else
                    {
                        candidates.Enqueue(bExpr.Left);
                        candidates.Enqueue(bExpr.Right);
                    }
                }
            }
            return filters;
        }
        public static string GetOperatorString(this ExpressionType type)
        {
            string xOperator = string.Empty;
            switch (type)
            {
                case ExpressionType.Add:
                    break;
                case ExpressionType.AddAssign:
                    break;
                case ExpressionType.AddAssignChecked:
                    break;
                case ExpressionType.AddChecked:
                    break;
                case ExpressionType.And:
                    xOperator = "AND";
                    break;
                case ExpressionType.AndAlso:
                    xOperator = "AND";
                    break;
                case ExpressionType.AndAssign:
                    break;
                case ExpressionType.ArrayIndex:
                    break;
                case ExpressionType.ArrayLength:
                    break;
                case ExpressionType.Assign:
                    break;
                case ExpressionType.Block:
                    break;
                case ExpressionType.Call:
                    break;
                case ExpressionType.Coalesce:
                    break;
                case ExpressionType.Conditional:
                    break;
                case ExpressionType.Constant:
                    break;
                case ExpressionType.Convert:
                    break;
                case ExpressionType.ConvertChecked:
                    break;
                case ExpressionType.DebugInfo:
                    break;
                case ExpressionType.Decrement:
                    break;
                case ExpressionType.Default:
                    break;
                case ExpressionType.Divide:
                    break;
                case ExpressionType.DivideAssign:
                    break;
                case ExpressionType.Dynamic:
                    break;
                case ExpressionType.Equal:
                    xOperator = "=";
                    break;
                case ExpressionType.ExclusiveOr:
                    break;
                case ExpressionType.ExclusiveOrAssign:
                    break;
                case ExpressionType.Extension:
                    break;
                case ExpressionType.Goto:
                    break;
                case ExpressionType.GreaterThan:
                    xOperator = ">";
                    break;
                case ExpressionType.GreaterThanOrEqual:
                    xOperator = ">=";
                    break;
                case ExpressionType.Increment:
                    break;
                case ExpressionType.Index:
                    break;
                case ExpressionType.Invoke:
                    break;
                case ExpressionType.IsFalse:
                    break;
                case ExpressionType.IsTrue:
                    break;
                case ExpressionType.Label:
                    break;
                case ExpressionType.Lambda:
                    break;
                case ExpressionType.LeftShift:
                    break;
                case ExpressionType.LeftShiftAssign:
                    break;
                case ExpressionType.LessThan:
                    xOperator = "<";
                    break;
                case ExpressionType.LessThanOrEqual:
                    xOperator = "<=";
                    break;
                case ExpressionType.ListInit:
                    break;
                case ExpressionType.Loop:
                    break;
                case ExpressionType.MemberAccess:
                    break;
                case ExpressionType.MemberInit:
                    break;
                case ExpressionType.Modulo:
                    break;
                case ExpressionType.ModuloAssign:
                    break;
                case ExpressionType.Multiply:
                    break;
                case ExpressionType.MultiplyAssign:
                    break;
                case ExpressionType.MultiplyAssignChecked:
                    break;
                case ExpressionType.MultiplyChecked:
                    break;
                case ExpressionType.Negate:
                    break;
                case ExpressionType.NegateChecked:
                    break;
                case ExpressionType.New:
                    break;
                case ExpressionType.NewArrayBounds:
                    break;
                case ExpressionType.NewArrayInit:
                    break;
                case ExpressionType.Not:
                    xOperator = "NOT";
                    break;
                case ExpressionType.NotEqual:
                    xOperator = "<>";
                    break;
                case ExpressionType.OnesComplement:
                    break;
                case ExpressionType.Or:
                    xOperator = "OR";
                    break;
                case ExpressionType.OrAssign:
                    break;
                case ExpressionType.OrElse:
                    break;
                case ExpressionType.Parameter:
                    break;
                case ExpressionType.PostDecrementAssign:
                    break;
                case ExpressionType.PostIncrementAssign:
                    break;
                case ExpressionType.Power:
                    break;
                case ExpressionType.PowerAssign:
                    break;
                case ExpressionType.PreDecrementAssign:
                    break;
                case ExpressionType.PreIncrementAssign:
                    break;
                case ExpressionType.Quote:
                    break;
                case ExpressionType.RightShift:
                    break;
                case ExpressionType.RightShiftAssign:
                    break;
                case ExpressionType.RuntimeVariables:
                    break;
                case ExpressionType.Subtract:
                    break;
                case ExpressionType.SubtractAssign:
                    break;
                case ExpressionType.SubtractAssignChecked:
                    break;
                case ExpressionType.SubtractChecked:
                    break;
                case ExpressionType.Switch:
                    break;
                case ExpressionType.Throw:
                    break;
                case ExpressionType.Try:
                    break;
                case ExpressionType.TypeAs:
                    break;
                case ExpressionType.TypeEqual:
                    break;
                case ExpressionType.TypeIs:
                    xOperator = "IS";
                    break;
                case ExpressionType.UnaryPlus:
                    break;
                case ExpressionType.Unbox:
                    break;
                default:
                    break;
            }
            return xOperator;
        }
    }
}
