using System;

namespace TodoList.Domain.Extensions
{
    public static class StringExtensions
    {
        public static T ParseEnum<T>(this string value)
        {
            return (T) Enum.Parse(typeof(T), value, true);
        }
    }
}