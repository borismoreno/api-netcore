using System;
using System.Reflection;

namespace ApiNetCore.Models
{
    public static class EnumString
    {
        public static string GetStringValue(Enum value)
        {
            string output = null;
            Type type = value.GetType();
            FieldInfo fi = type.GetField(value.ToString());
            EnumValueAttribute[] attributes = fi.GetCustomAttributes(typeof(EnumValueAttribute), false) as EnumValueAttribute[];
            if (attributes.Length > 0)
            {
                output = attributes[0].Value;
            }
            return output;
        }
    }
}