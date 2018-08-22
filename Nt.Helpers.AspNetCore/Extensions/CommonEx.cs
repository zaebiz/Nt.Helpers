using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Nt.Helpers.AspNetCore.Extensions
{
    public static class CommonEx
    {
        public static TAttribute GetAttribute<TAttribute>(this Enum value) where TAttribute : Attribute
        {
            try
            {
                var type = value.GetType();
                var name = Enum.GetName(type, value);
                return type.GetField(name)
                    .GetCustomAttributes(false)
                    .OfType<TAttribute>()
                    .SingleOrDefault();
            }
            catch (Exception)
            {

            }
            return null;
        }

        public static TAttribute GetAttribute<TEnum, TAttribute>(this TEnum value) where TAttribute : Attribute
        {
            try
            {
                var type = value.GetType();
                var name = Enum.GetName(type, value);
                return type.GetField(name)
                    .GetCustomAttributes(false)
                    .OfType<TAttribute>()
                    .SingleOrDefault();
            }
            catch (Exception)
            {

            }
            return null;
        }

        public static string GetDisplayName(this Enum element)
        {
            DisplayAttribute attribute = element.GetAttribute<DisplayAttribute>();
            if (attribute != null)
            {
                return attribute.Name;
            }
            return element.ToString();
        }

        public static string GetDisplayGroupName(this Enum element)
        {
            DisplayAttribute attribute = element.GetAttribute<DisplayAttribute>();
            if (attribute != null)
            {
                return attribute.GroupName;
            }
            return element.ToString();
        }

        // исключения
        public static IEnumerable<Exception> Try(this Exception exception)
        {
            //if (exception is DbEntityValidationException)
            //{
            //    foreach (var errors in (exception as DbEntityValidationException).EntityValidationErrors)
            //    {
            //        foreach (var e in errors.ValidationErrors)
            //        {
            //            yield return new Exception(e.ErrorMessage, exception);
            //        }
            //    }
            //}
            //else
            {
                while (exception != null)
                {
                    yield return exception;
                    exception = exception.InnerException;
                }
            }
        }

        public static Exception TryLast(this Exception exception)
        {
            return Try(exception).Last();
        }
    }
}
