using System;
using System.Linq.Expressions;
using System.Reflection;

namespace reflection
{
    public class Differentiator
    {
        public static Expression DifferentiateMultiplication(BinaryExpression exp)
        {
            return Expression.Add(
                Expression.Multiply(Differentiate(exp.Left), exp.Right),
                Expression.Multiply(exp.Left, Differentiate(exp.Right))
            );
        }

        private static Expression DifferentiateAddition(BinaryExpression exp)
        {
            return Expression.Add(
                Differentiate(exp.Left),
                Differentiate(exp.Right)
            );
        }

        private static Expression DifferentiateSinus(MethodCallExpression exp)
        {
            return Expression.Multiply(
                Differentiate(exp.Arguments[0]), 
                Expression.Call(typeof(Math).GetMethod("Cos", BindingFlags.Public | BindingFlags.Static), exp.Arguments)
            );
        }

        private static Expression Differentiate(Expression exp)
        {
            if (exp is ConstantExpression)
                return Expression.Constant(0.0);
            if (exp is ParameterExpression)
                return Expression.Constant(1.0);
            if (exp is BinaryExpression && exp.NodeType == ExpressionType.Multiply)
                return DifferentiateMultiplication((BinaryExpression)exp);
            if (exp is BinaryExpression && exp.NodeType == ExpressionType.Add)
                return DifferentiateAddition((BinaryExpression)exp);
            if (exp is MethodCallExpression && ((MethodCallExpression)exp).Method.Name == "Sin")
                return DifferentiateSinus((MethodCallExpression)exp);
            throw new ArgumentException();
        }

        public static Func<double, double> Differentiate(Expression<Func<double, double>> function)
        {
            var lambda = Expression.Lambda<Func<double, double>>(Differentiate(function.Body), function.Parameters);
            return lambda.Compile();
        }
    }
}