using System;
using System.Collections.Generic;
using System.Linq;
using WebApi.Services;
using System.Linq.Dynamic.Core;

namespace WebApi.Helpers
{
    public static class IQueryableExtentions
    {
        public static IQueryable<T> ApplySort<T>(this IQueryable<T> source, string orderBy,
            Dictionary<string, PropertyMappingValue> mappingDictionary)
        {
            if(source == null)
                throw new ArgumentNullException(nameof(source));
            
            if(mappingDictionary == null)
                throw new ArgumentNullException(nameof(mappingDictionary));

            if (string.IsNullOrWhiteSpace(orderBy))
                return source;
            
            //the orderby string is separated by "," so we split it
            var orderByAfterSplit = orderBy.Split(',');
            
            //apply each orderby clause in reverse order = otherwise the iqueryable will be ordered in wrong order
            foreach (var orderByClause in orderByAfterSplit.Reverse())
            {
                //trim the orderby clause as it might contain leading or trailing spaces.  Can't trim foreach so use another var
                var trimmedOrderByClause = orderByClause.Trim();
                
                //if the sort option ends with 'desc' we order descending, otherwise ascending
                var orderDescending = trimmedOrderByClause.EndsWith(" desc");
                
                // remove asc or desc from the orderby clause, so we ge the property name to look for in dictionary
                var indexOfFirstSpace = trimmedOrderByClause.IndexOf(" ", StringComparison.Ordinal);
                var propertyName = indexOfFirstSpace == -1
                    ? trimmedOrderByClause
                    : trimmedOrderByClause.Remove(indexOfFirstSpace);
                
                // find the matching property
                if(!mappingDictionary.ContainsKey(propertyName))
                    throw new ArgumentException($"Key mapping for {propertyName} is missing");
                
                //get the propertyMappingValue
                var propertyMappingValue = mappingDictionary[propertyName];
                
                if (propertyMappingValue == null)
                    throw new ArgumentNullException(nameof(propertyMappingValue));
                
                //Run through the property names in reverse so the orderby clauses are applied in the correct order
                foreach (var destinationProperty in propertyMappingValue.DestinationProperties.Reverse())
                {
                    //reverse sort order if necessary
                    if (propertyMappingValue.Revert)
                        orderDescending = !orderDescending;

                    source = source.OrderBy(destinationProperty + (orderDescending ? " descending" : " ascending"));
                }
            }

            return source;
        }
    }
}