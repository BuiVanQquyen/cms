using System;
using System.Linq;

namespace MeeyPage.Core
{
    public static class EntityExtension
    {
        

        public static T ConvertToType<T>(this object value) where T : new()
        {
            T returnValue = default(T);
            if (value == null)
                return new T();
            else if (value is T)
            {
                returnValue = (T)value;
            }
            else
            {
                try
                {
                    returnValue = (T)Convert.ChangeType(value, typeof(T));
                }
                catch (InvalidCastException)
                {
                    returnValue = default(T);
                }
            }

            return returnValue;
        }
        /// <summary>
        /// Lấy giá trịn của 1 attribute bất kì
        /// </summary>
        /// <typeparam name="TAttribute">The type of the attribute.</typeparam>
        /// <typeparam name="TValue">The type of the value.</typeparam>
        /// <param name="type">The type.</param>
        /// <param name="valueSelector">The value selector.</param>
        /// <returns></returns>
        public static TValue GetAttributeTableName<TAttribute, TValue>(this Type type, Func<TAttribute, TValue> valueSelector) where TAttribute : Attribute
        {
            if (type.GetCustomAttributes(typeof(TAttribute), true).FirstOrDefault() is TAttribute att)
            {
                return valueSelector(att);
            }

            return default(TValue);
        }

    }
}
