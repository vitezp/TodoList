using System;

namespace TodoList.Shared.Extensions
{
    public static class StringExtensions
    {
        public static T ParseEnum<T>(this string value)
        {
            return (T) Enum.Parse(typeof(T), value, true);
        }
    }
}