﻿using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Reflection;
using Microsoft.AspNetCore.Diagnostics;

namespace WebApi.Helpers
{
    public static class ObjectExtensions
    {
        public static ExpandoObject ShapeData<TSource>(this TSource source, string fields)
        {
            if(source == null)
                throw new ArgumentNullException(nameof(source));
            
            var dataShapedObject = new ExpandoObject();

            if (string.IsNullOrWhiteSpace(fields))
            {
                //all properties included in expando object
                var propertyInfos =
                    typeof(TSource).GetProperties(BindingFlags.IgnoreCase | BindingFlags.Public |
                                                  BindingFlags.Instance);

                foreach (var propertyInfo in propertyInfos)
                {
                    // get value of property on source object
                    var propertyValue = propertyInfo.GetValue(source);
                    
                    //add the field to the ExpandoObject
                    ((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name, propertyValue);
                }
                
                return dataShapedObject;
              
            }

            var fieldsAfterSplit = fields.Split(',');

            foreach (var field in fieldsAfterSplit)
            {
                var propertyName = field.Trim();

                var propertyInfo = typeof(TSource).GetProperty(propertyName,
                    BindingFlags.IgnoreCase | BindingFlags.Public |
                    BindingFlags.Instance);
                
                if(propertyInfo == null)
                    throw new Exception($"Property {propertyName} wasn't found on {typeof(TSource)}");

                var propertyValue = propertyInfo.GetValue(source);
                
                ((IDictionary<string, object>)dataShapedObject).Add(propertyInfo.Name, propertyValue);
            }

            return dataShapedObject;

        }
    }
}