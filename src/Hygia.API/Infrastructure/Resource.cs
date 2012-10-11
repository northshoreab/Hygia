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

    public class Resource<T> : ResponseMetaData
    {
        public T Data { get; set; }
    }

    public static class ResponseItemExtensions
    {
        public static Resource<T> AsResponseItem<T>(this T item, IEnumerable<Link> links)
        {
            return new Resource<T>
                       {
                           Data = item,
                           Links = links
                       };
        }

        public static Resource<T> AsResponseItem<T>(this T item)
        {
            return new Resource<T>
            {
                Data = item,
                Links = new List<Link>()
            };
        }

        public static Resource<T> AddLinks<T>(this Resource<T> resource, Func<T, IEnumerable<Link>> links)
        {
            resource.Links = resource.Links.Union(links(resource.Data));

            return resource;
        }
    }
}