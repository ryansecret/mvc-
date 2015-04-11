using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;

namespace MVC.Controls
{
    public static class ExpressionExtensions
    {
        public static string MemberWithoutInstance<T>(this Expression<Func<T, object>> expression) where T : class
        {
            MemberExpression memberExpression = expression.ToMemberExpression();

            if (memberExpression == null)
            {
                return null;
            }

            if (memberExpression.Expression.NodeType == ExpressionType.MemberAccess)
            {
                MemberExpression innerMemberExpression = (MemberExpression)memberExpression.Expression;

                return memberExpression.ToString().Substring(innerMemberExpression.Expression.ToString().Length + 1);
            }

            return memberExpression.Member.Name;
        }

        public static MemberExpression ToMemberExpression<T>(this Expression<Func<T, object>> expression) where T : class
        {
            MemberExpression memberExpression = expression.Body as MemberExpression;

            if (memberExpression == null)
            {
                UnaryExpression unaryExpression = expression.Body as UnaryExpression;

                if (unaryExpression != null)
                {
                    memberExpression = unaryExpression.Operand as MemberExpression;
                }
            }

            return memberExpression;
        }
    }
}
