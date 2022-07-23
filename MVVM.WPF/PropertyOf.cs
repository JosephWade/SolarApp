using System.Linq.Expressions;

namespace MVVM.WPF
{
    public class PropertyOf<T>
    {
        public static string Resolve(Expression<Func<T, object>> expression)
        {
            switch(expression.Body.NodeType)
            {
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    UnaryExpression? unaryExpression = expression.Body as UnaryExpression;
                    if (unaryExpression != null && unaryExpression.Operand != null)
                    {
                        #pragma warning disable CS8602 // Dereference of a possibly null reference.
                        return (unaryExpression.Operand as MemberExpression).Member.Name;
                        #pragma warning restore CS8602 // Dereference of a possibly null reference.
                    }
                    else
                    {
                        return string.Empty;
                    }
                default:
                    MemberExpression? memberExpression = expression.Body as MemberExpression;
                    if (memberExpression != null)
                    {
                        return memberExpression.Member.Name;
                    }
                    else
                    {
                        return string.Empty;
                    }

            }
        }
    }

}