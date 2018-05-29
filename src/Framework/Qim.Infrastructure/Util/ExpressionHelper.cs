using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Qim.Util
{
    public static class ExpressionHelper
    {
        public static PropertyInfo GetPropertyInfo(LambdaExpression lambdaExpression)
        {
            var expression = lambdaExpression.Body;
            if (expression.NodeType == ExpressionType.Convert ||
                expression.NodeType == ExpressionType.ConvertChecked)
            {
                // For Boxed Value Types
                expression = ((UnaryExpression)expression).Operand;
            }

            if (expression.NodeType != ExpressionType.MemberAccess)
            {
                throw new InvalidOperationException(
                    string.Format(Resource.Invalid_IncludePropertyExpression, expression.NodeType));
            }

            var memberExpression = (MemberExpression)expression;
            var memberInfo = memberExpression.Member as PropertyInfo;
            if (memberInfo != null)
            {
                if (memberExpression.Expression.NodeType != ExpressionType.Parameter)
                {
                    // Chained expressions and non parameter based expressions are not supported.
                    throw new InvalidOperationException(
                     string.Format(Resource.Invalid_IncludePropertyExpression, expression.NodeType));
                }

                return memberInfo;
            }
            else
            {
                // Fields are also not supported.
                throw new InvalidOperationException(
                     string.Format(Resource.Invalid_IncludePropertyExpression, expression.NodeType));
            }
        }


    }
}