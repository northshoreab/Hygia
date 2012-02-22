namespace Hygia
{
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Dynamic;
    using System.Linq;

    public static class DynamicHelpers
    {


        public static ExpandoObject Combine(dynamic item1, dynamic item2)
        {
            IDictionary<string, object> dictionary1;
            IDictionary<string, object> dictionary2;
            if (item1 is ExpandoObject)
                dictionary1 = (IDictionary<string, object>)item1;
            else
                dictionary1 = (IDictionary<string, object>)ToDynamic(item1);

            if (item2 is ExpandoObject)
                dictionary2 = (IDictionary<string, object>)item2;
            else
                dictionary2 = (IDictionary<string, object>)ToDynamic(item2);

            var result = new ExpandoObject();
            var d = result as IDictionary<string, object>; //work with the Expando as a Dictionary

            foreach (var pair in dictionary1.Concat(dictionary2))
            {
                d[pair.Key] = pair.Value;
            }

            return result;
        }

        public static dynamic ToDynamic(object value)
        {
            IDictionary<string, object> expando = new ExpandoObject();

            foreach (PropertyDescriptor property in TypeDescriptor.GetProperties(value.GetType()))
                expando.Add(property.Name, property.GetValue(value));

            return expando as ExpandoObject;
        }
    }
}