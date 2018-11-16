using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using GameDevWare.Serialization;

namespace Colyseus
{
    public static class ObjectExtensions
    {
        public static T ToObject<T>(object source) where T : class, new()
        {
            var someObject = new T();
            var someObjectType = someObject.GetType();

            foreach (var item in (IDictionary<string, object>)source) {
                var propInfo = someObjectType.GetProperty(item.Key);
                if (propInfo.PropertyType.IsEnum)
                {
                    propInfo.SetValue(someObject, Enum.Parse(propInfo.PropertyType, item.Value.ToString()), null);
                }
                else
                {
                    propInfo.SetValue(someObject, item.Value, null);
                }
            }

            return someObject;
        }

        public static IDictionary<string, object> ToDictionary<T>(T source)
        {
            var type = typeof(T);
            var dictionary = new IndexedDictionary<string, object>();
            var membersToWrite = type.GetProperties().Where(p => p.GetCustomAttribute(typeof(DataMemberAttribute)) != null);
            foreach (var prop in membersToWrite)
            {
                if (prop.PropertyType.IsEnum)
                {
                    dictionary.Add(prop.Name, prop.GetValue(source).ToString());
                }
                else
                {
                    dictionary.Add(prop.Name, prop.GetValue(source));
                }
            }
            return dictionary;
        }
    }
}
