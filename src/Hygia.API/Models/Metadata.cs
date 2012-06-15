using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;

namespace Hygia.API.Models
{
    [DataContract]
    public class Metadata<T> where T : class
    {
        [DataMember]
        public int TotalResults { get; set; }

        [DataMember]
        public int ReturnedResults { get; set; }

        [DataMember]
        public T[] Results { get; set; }

        [DataMember]
        public DateTime Timestamp { get; set; }

        [DataMember]
        public string Status { get; set; }

        public Metadata(HttpResponseMessage httpResponse, bool isIQueryable)
        {
            Timestamp = DateTime.Now;

            if (httpResponse.Content != null && httpResponse.IsSuccessStatusCode)
            {
                TotalResults = 1;
                ReturnedResults = 1;
                Status = "Success";

                if (isIQueryable)
                {
                    IEnumerable<T> enumResponseObject;
                    httpResponse.TryGetContentValue(out enumResponseObject);
                    Results = enumResponseObject.ToArray();
                    ReturnedResults = enumResponseObject.Count();
                }
                else
                {
                    T responseObject;
                    httpResponse.TryGetContentValue(out responseObject);
                    Results = new[] { responseObject };
                }
            }

            else
            {
                Status = "Error";
                ReturnedResults = 0;
            }
        }
    }
}