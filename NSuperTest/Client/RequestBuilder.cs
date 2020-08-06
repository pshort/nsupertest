using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;

namespace NSuperTest.Client
{
    public class RequestBuilder
    {
        public HttpRequestMessage Build(
            string url, 
            HttpMethod method, 
            object body = null,
            Headers headers = null,
            Query query = null
        )
        {
            var uri = BuildUri(url, query);
            var req = new HttpRequestMessage(method, uri);

            if(body != null && IsBodiedRequest(method))
            {
                var bodySerialized = JsonConvert.SerializeObject(body);
                req.Content = new StringContent(bodySerialized, Encoding.UTF8, "application/json");
            }

            if(headers != null && headers.Count != 0)
            {
                foreach(var header in headers)
                {
                    req.Headers.Add(header.Key, header.Value);
                }
            }

            return req;
        }

        private Uri BuildUri(string path, Query query)
        {
            if(query != null && query.Count != 0)
            {
                var sb = new StringBuilder(path);
                if(path.Contains("?"))
                {
                    foreach(var q in query)
                    {
                        sb.Append($"&{q.Key}={q.Value}");
                    }
                }
                else
                {
                    var f = query.First();
                    sb.Append($"?{f.Key}={f.Value}");
                    foreach(var q in query.Skip(1))
                    {
                        sb.Append($"&{q.Key}={q.Value}");
                    }
                }
                path = sb.ToString();
            }
            return new Uri(path, UriKind.Relative);
        }

        private bool IsBodiedRequest(HttpMethod method)
        {
            return method == HttpMethod.Post || method == HttpMethod.Put || method.Method == "PATCH";
        }
    }
}
