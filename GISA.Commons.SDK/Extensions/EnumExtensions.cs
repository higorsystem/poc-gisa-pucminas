using System;
using System.ComponentModel;

namespace GISA.Commons.SDK.Extensions
{
    public static class EnumExtensions
    {
        public static string ToDescription<T>(this T enumerationValue) where T : struct
        {
            var type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException($"{nameof(enumerationValue)} must be of Enum type", nameof(enumerationValue));
            }
            var memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo.Length > 0)
            {
                var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);

                if (attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return enumerationValue.ToString();
        }

        public static T ToEnum<T>(this string param, T defaultValue)
        {
            T result;
            try
            {
                result = (T)Enum.Parse(typeof(T), param, true);
            }
            catch
            {
                result = defaultValue;
            }
            return result;
        }
    }
}