using System;
using System.Collections.Generic;
using System.Linq;

namespace Hygia.API.Infrastructure
{
    public class Link
    {
        public string Rel { get; set; }
        public string Href { get; set; }
        public bool Templated { get; set; }
    }

    public class MetaData
    {
        public string Version { get; set; }
    }

    public class ResponseMetaData
    {
        //TODO: require selfUrl
        public ResponseMetaData(string selfUrl = "/")
        {
            Links = new List<Link> 
            { 
                new Link { Href = selfUrl, Rel = "self" }
            };
        }

        public IEnumerable<Link> Links { get; set; }
        public MetaData MetaData { get; set; }        
    }

    public class ResponseItem<T> : ResponseMetaData
    {
        public T Data { get; set; }
    }

    public static class ResponseItemExtensions
    {
        public static ResponseItem<T> AsResponseItem<T>(this T item, IEnumerable<Link> links = null)
        {
            return new ResponseItem<T>
                       {
                           Data = item,
                           Links = links ?? new List<Link>()
                       };
        }

        public static ResponseItem<T> AddLinks<T>(this ResponseItem<T> responseItem, Func<T, IEnumerable<Link>> links)
        {
            responseItem.Links = responseItem.Links.Union(links(responseItem.Data));

            return responseItem;
        }
    }
}