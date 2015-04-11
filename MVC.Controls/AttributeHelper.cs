using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Linq.Expressions;
using System.Reflection;
using System.ComponentModel.DataAnnotations;

namespace MVC.Controls
{
    public static class AttributeHelper
    {
        private static Dictionary<object, List<Attribute>> _attributeCache = new Dictionary<object, List<Attribute>>();

        public static Dictionary<object, List<Attribute>> AttributeCache { get { return _attributeCache; } }

        // Types
        public static List<Attribute> GetTypeAttributes<TType>()
        {
            return GetTypeAttributes(typeof(TType));
        }

        public static List<Attribute> GetTypeAttributes(Type type)
        {
            return LockAndGetAttributes(type, tp => ((Type)tp).GetCustomAttributes(true));
        }

        public static List<TAttributeType> GetTypeAttributes<TAttributeType>(Type type, Func<TAttributeType, bool> predicate = null)
        {
            return
                GetTypeAttributes(type)
                    .OfType<TAttributeType>()
                    .Where(attr => predicate == null || predicate(attr))
                    .ToList();
        }

        public static List<TAttributeType> GetTypeAttributes<TType, TAttributeType>(Func<TAttributeType, bool> predicate = null)
        {
            return GetTypeAttributes(typeof(TType), predicate);
        }

        public static TAttributeType GetTypeAttribute<TType, TAttributeType>(Func<TAttributeType, bool> predicate = null)
        {
            return
                GetTypeAttribute(typeof(TType), predicate);
        }

        public static TAttributeType GetTypeAttribute<TAttributeType>(Type type, Func<TAttributeType, bool> predicate = null)
        {
            return
                GetTypeAttributes<TAttributeType>(type, predicate)
                    .FirstOrDefault();
        }

        public static bool HasTypeAttribute<TType, TAttributeType>(Func<TAttributeType, bool> predicate = null)
        {
            return HasTypeAttribute<TAttributeType>(typeof(TType), predicate);
        }

        public static bool HasTypeAttribute<TAttributeType>(Type type, Func<TAttributeType, bool> predicate = null)
        {
            return GetTypeAttribute<TAttributeType>(type, predicate) != null;
        }

        // Members and properties
        public static List<Attribute> GetMemberAttributes<TType>(Expression<Func<TType, object>> action)
        {
            return GetMemberAttributes(GetMember(action));
        }

        public static List<TAttributeType> GetMemberAttributes<TType, TAttributeType>(
            Expression<Func<TType, object>> action,
            Func<TAttributeType, bool> predicate = null)
            where TAttributeType : Attribute
        {
            return GetMemberAttributes<TAttributeType>(GetMember(action), predicate);
        }

        public static TAttributeType GetMemberAttribute<TType, TAttributeType>(
            Expression<Func<TType, object>> action,
            Func<TAttributeType, bool> predicate = null)
            where TAttributeType : Attribute
        {
            return GetMemberAttribute<TAttributeType>(GetMember(action), predicate);
        }

        public static bool HasMemberAttribute<TType, TAttributeType>(Expression<Func<TType, object>> action, Func<TAttributeType, bool> predicate = null) where TAttributeType : Attribute
        {
            return GetMemberAttribute(GetMember(action), predicate) != null;
        }

        // MemberInfo (and PropertyInfo since PropertyInfo inherits from MemberInfo)
        public static List<Attribute> GetMemberAttributes(this MemberInfo memberInfo)
        {
            return
                LockAndGetAttributes(memberInfo, mi => GetMemberAttributesWithMeta((MemberInfo)mi).ToArray());
        }

        public static List<Attribute> GetMemberAttributesWithMeta(this MemberInfo mi) 
        {
            List<Attribute> result =
                mi
                    .GetCustomAttributes(true)
                    .OfType<Attribute>()
                    .ToList();

            foreach(MetadataTypeAttribute meta in GetTypeAttributes<MetadataTypeAttribute>(mi.DeclaringType))
            {
                MemberInfo[] metaMembers = meta.MetadataClassType.GetMember(mi.Name);
                foreach(MemberInfo mi2 in metaMembers)
                {
                    result.AddRange(mi2.GetCustomAttributes(true).OfType<Attribute>());
                }
            }
            return result;
        }

        public static List<TAttributeType> GetMemberAttributes<TAttributeType>(this MemberInfo memberInfo, Func<TAttributeType, bool> predicate = null) where TAttributeType : Attribute
        {
            return
                GetMemberAttributes(memberInfo)
                    .OfType<TAttributeType>()
                    .Where(attr => predicate == null || predicate(attr))
                    .ToList();
        }

        public static TAttributeType GetMemberAttribute<TAttributeType>(this MemberInfo memberInfo, Func<TAttributeType, bool> predicate = null) where TAttributeType : Attribute
        {
            return
                GetMemberAttributes<TAttributeType>(memberInfo, predicate)
                    .FirstOrDefault();
        }

        public static bool HasMemberAttribute<TAttributeType>(this MemberInfo memberInfo, Func<TAttributeType, bool> predicate = null) where TAttributeType : Attribute
        {
            return
                memberInfo.GetMemberAttribute<TAttributeType>(predicate) != null;
        }

        // Internal stuff
        private static TType FirstOrDefault<TX, TType>(this IEnumerable<TX> list)
        {
            return
                list
                    .OfType<TType>()
                    .FirstOrDefault();
        }

        private static List<Attribute> LockAndGetAttributes(object key, Func<object, object[]> retrieveValue)
        {
            return
                LockAndGet<object, List<Attribute>>(_attributeCache, key, mi => retrieveValue(mi).Cast<Attribute>().ToList());
        }

        // Method for thread safely executing slow method and storing the result in a dictionary
        private static TValue LockAndGet<TKey, TValue>(Dictionary<TKey, TValue> dictionary, TKey key, Func<TKey, TValue> retrieveValue)
        {
            TValue value = default(TValue);
            lock (dictionary)
            {
                if (dictionary.TryGetValue(key, out value))
                {
                    return value;
                }
            }

            value = retrieveValue(key);

            lock (dictionary)
            {
                if (dictionary.ContainsKey(key) == false)
                {
                    dictionary.Add(key, value);
                }

                return value;
            }
        }

        public static MemberInfo GetMember<T>(Expression<Func<T, object>> expression)
        {
            MemberExpression memberExpression = expression.Body as MemberExpression;

            if (memberExpression != null)
            {
                return memberExpression.Member;
            }

            UnaryExpression unaryExpression = expression.Body as UnaryExpression;

            if (unaryExpression != null)
            {
                memberExpression = unaryExpression.Operand as MemberExpression;

                if (memberExpression != null)
                {
                    return memberExpression.Member;
                }

                MethodCallExpression methodCall = unaryExpression.Operand as MethodCallExpression;
                if (methodCall != null)
                {
                    return methodCall.Method;
                }
            }

            return null;
        }

        public static bool IsMember<T>(Expression<Func<T, object>> expression)
        {
            return GetMember<T>(expression) != null;
        }

        public static string GetMemberName<T>(T example, Expression<Func<T, object>> expression) where T : class
        {
            MemberExpression memberExpression = GetMemberExpression(expression);

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

        public static string GetMemberName<T>(Expression<Func<T, object>> expression) where T : class
        {
            MemberExpression memberExpression = GetMemberExpression(expression);

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

        public static MemberExpression GetMemberExpression<T>(Expression<Func<T, object>> expression) where T : class
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

