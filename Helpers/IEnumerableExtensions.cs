using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;

namespace WebApi.Helpers
{
    public static class IEnumerableExtensions
    {
        public static IEnumerable<ExpandoObject> ShapeData<TSource>(this IEnumerable<TSource> source, string fields)
        {
            if(source == null)
                throw new ArgumentNullException(nameof(source));
            
            // create a list to hold our expanded objects
            var expandedObjectList = new List<ExpandoObject>();
            
            //create a list with PropertyInfo objects on TSource.  Reflection is expensive, so rather than doing it
            //for each object in the list, we do it once and reuse the results.  After all, part of the reflection is on
            //the type of the object(TSource), not on the instance
            var propertyInfoList = new List<PropertyInfo>();

            if (string.IsNullOrWhiteSpace(fields))
            {
                //all public properties should be in the expanded object
                var propertyInfos = typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance);

                propertyInfoList.AddRange(propertyInfos);
            }
            else
            {
                //only the public properties that match the fields should be in the expanded object
                //the fields are separated by "," so we split it
                var fieldsAfterSplit = fields.Split(',');

                foreach (var field in fieldsAfterSplit)
                {
                    //trim each field as it might contain spaces
                    var propertyName = field.Trim();
                    
                    //use reflection to get the property on the source object.  we need to include public and instance
                    //because specifying a binding flag overwrites the already existing binding flags
                    var propertyInfo = typeof(TSource).GetProperty(propertyName,
                        BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);
                    
                    if(propertyInfo == null)
                        throw new Exception($"Property {propertyName} wasn't found on {typeof(TSource)}");
                    
                    propertyInfoList.Add(propertyInfo);
                }
            }
            
            //run through the source objects
            foreach (var sourceObject in source)
            {
                // create an ExpandoObject that will hold the selected properties and values
                var dataShapedObject = new ExpandoObject();
                
                //Get the value of each property we have to return.  for that, we run through the list
                foreach (var propertyInfo in propertyInfoList)
                {
                    //GetValue returns the value of the property on the source object
                    var propertyValue = propertyInfo.GetValue(sourceObject);
                    
                    //add the field to the ExpandoObject
                    ((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name, propertyValue);
                }
                
                //add the expandoObject to the list
                expandedObjectList.Add(dataShapedObject);
            }
            
            // return the list
            return expandedObjectList;
        }
    }
}