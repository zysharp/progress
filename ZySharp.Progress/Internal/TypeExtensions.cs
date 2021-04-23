using System;
using System.Collections.Generic;

namespace ZySharp.Progress.Internal
{
    internal static class TypeExtensions
    {
        private static readonly HashSet<Type> NumericTypes = new()
        {
            typeof(sbyte),
            typeof(byte),
            typeof(int),
            typeof(uint),
            typeof(short),
            typeof(ushort),
            typeof(long),
            typeof(ulong),
            typeof(decimal),
            typeof(float),
            typeof(double),
        };

        public static bool IsNumeric(this Type type)
        {
            return NumericTypes.Contains(type);
        }
    }
}