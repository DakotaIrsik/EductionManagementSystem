using Microsoft.EntityFrameworkCore;
using SilverLeaf.Common.Interfaces;
using SilverLeaf.Common.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SilverLeaf.Common.Extensions
{
    public static class AdjustableExtensions
    {
        public static IEnumerable<T> SortBy<T>(this IEnumerable<T> source, IAdjustable adj)
        {
            var queryableSource = source.AsQueryable();
            var result = queryableSource.SortBy(adj);
            return result;
        }

        public static IQueryable<T> SortBy<T>(this IQueryable<T> source, IAdjustable adj)
        {
            if (string.IsNullOrWhiteSpace(adj?.Sort))
            {
                return source;
            }

            var columns = adj.Sort.Replace(" ", "").Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries);

            bool thenBy = false;
            foreach (var column in columns)
            {
                source = SortByParam(source, column, thenBy);
                thenBy = true;
            }

            return source;
        }

        private static IEnumerable<T> SortByParam<T>(IEnumerable<T> query, string column, bool thenBy)
        {
            var queryableSource = query.AsQueryable();
            var result = SortByParam(query, column, thenBy);
            return result;
        }

        private static IQueryable<T> SortByParam<T>(IQueryable<T> query, string column, bool thenBy)
        {
            bool desc = column.StartsWith("-");

            column = column.Trim('-').Trim('+')?.ToLower();

            var hasproperty = typeof(T).GetProperties().Any(p => p.Name.ToLower() == column);

            if (!hasproperty)
            {
                return query;
            }

            string orderby;
            if (thenBy)
            {
                orderby = desc
                    ? nameof(Enumerable.ThenByDescending)
                    : nameof(Enumerable.ThenBy);
            }
            else
            {
                orderby = desc
                    ? nameof(Enumerable.OrderByDescending)
                    : nameof(Enumerable.OrderBy);
            }

            var parameter = Expression.Parameter(typeof(T), "x");
            Expression property = Expression.Property(parameter, column);
            var lambda = Expression.Lambda(property, parameter);

            var orderByMethod = typeof(Queryable).GetMethods().First(x => x.Name == orderby && x.GetParameters().Length == 2);
            var orderByGeneric = orderByMethod.MakeGenericMethod(typeof(T), property.Type);
            var result = orderByGeneric.Invoke(null, new object[] { query, lambda });

            return (IQueryable<T>)result;
        }

        public static ExpandoObject ToExpando(this object obj, IEnumerable<string> fields)
        {
            ExpandoObject expando = new ExpandoObject();
            var dict = expando as IDictionary<string, object>;

            fields = (!fields.Safe().Any()) ? obj.GetType().GetProperties().Select(p => p.Name).ToList() : fields;
            foreach (var field in fields)
            {
                var properties = obj.GetType().GetProperties();
                var property = properties.FirstOrDefault(p => p.Name.ToLower() == field?.ToLower());
                if (property != null)
                {
                    dict[property.Name] = property.GetValue(obj);
                }
                //TODO Make this infinitely recursive, so you can go as deep as you want with field selection.
                else if (field.Contains("."))
                {
                    var subObjectPropertyName = field.Substring(0, field.IndexOf("."));
                    var subObject = obj.GetType().GetProperties().FirstOrDefault(p => p.Name.ToLower() == subObjectPropertyName.ToLower()).GetValue(obj);
                    var nestedPropertyName = field.Substring(field.IndexOf(".") + 1);
                    dict[field] = subObject.GetType().GetProperties().FirstOrDefault(p => p.Name == nestedPropertyName).GetValue(subObject);
                }
                else
                {
                    dict[field] = "Property Does Not Exit";
                }
            }

            return expando;
        }

        public static object FieldSelect(this object obj, string fields)
        {
            var fieldList = fields?.Replace(" ", "").Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries)
                                   .Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

            var nestedDataType = obj.GetType().GetProperties().SingleOrDefault(p => p.Name == "Data");
            //pageable response, preserve the paging stuff and Expando the Data Elements
            if (nestedDataType != null && !nestedDataType.PropertyType.IsPrimitive)
            {
                var dataElements = (ICollection)nestedDataType.GetValue(obj);
                var adjustableDTO = new AdjustableDTO<ExpandoObject>((IAdjustable)obj, GetExpandoList((ICollection)dataElements, fieldList));
                var totalDataType = obj.GetType().GetProperties().SingleOrDefault(p => p.Name == "Total");
                adjustableDTO.Total = (long)totalDataType.GetValue(obj);
                return adjustableDTO;
            }
            //regular list of objects, make them expando
            else if (obj is ICollection)
            {
                return GetExpandoList((ICollection)obj, fieldList);
            }

            //if we got this far, it's a single object, so expando it.
            return obj.ToExpando(fieldList);
        }

        private static IEnumerable<ExpandoObject> GetExpandoList<T>(T obj, IEnumerable<string> fieldList) where T : ICollection
        {
            var expandoList = new List<ExpandoObject>();
            foreach (var item in obj)
            {
                expandoList.Add(item.ToExpando(fieldList));
            }

            return expandoList;
        }

        public async static Task<AdjustableDTO<T>> GetPaged<T>(this IQueryable<T> query, IAdjustable paging) where T : class
        {
            var result = new AdjustableDTO<T>(paging);
            result.Total = query.Count();
            result.Data = await query.Skip(paging.From)
                                     .Take(paging.Size).ToListAsync();
            result.From = paging.From;
            return result;
        }
    }
}
