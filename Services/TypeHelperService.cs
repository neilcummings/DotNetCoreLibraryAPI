using System;
using System.Reflection;

namespace WebApi.Services
{
    public class TypeHelperService : ITypeHelperService
    {
        public bool TypeHasProperties<T>(string fields)
        {
            if (string.IsNullOrWhiteSpace(fields))
                return true;

            var fieldsAfterSplit = fields.Split(',');

            foreach (var field in fieldsAfterSplit)
            {
                //trim each field as it might contain spaces
                var propertyName = field.Trim();
                    
                //use reflection to get the property on the source object.  we need to include public and instance
                //because specifying a binding flag overwrites the already existing binding flags
                var propertyInfo = typeof(T).GetProperty(propertyName,
                    BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

                if (propertyInfo == null)
                    return false;
                
            }
            
            //all checks out so return true
            return true;
        }
    }
}