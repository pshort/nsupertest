using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace NSuperTest.Client
{
    public class GetRequest : Request
    {
        public GetRequest(string url)
        {
            Url = url;
            Method = HttpMethod.Get;
        }
    }

    public class PostRequest : Request
    {
        public PostRequest(string url, object body)
        {
            Url = url;
            Method = HttpMethod.Post;
            Body = body;
        }
    }

    public class PutRequest : Request
    {
        public PutRequest(string url, object body)
        {
            Url = url;
            Method = HttpMethod.Put;
            Body = body;
        }
    }

    public class PatchRequest : Request
    {
        public PatchRequest(string url, object body)
        {
            Url = url;
            Method = new HttpMethod("PATCH");
            Body = body;
        }
    }

    public class DeleteRequest : Request
    {
        public DeleteRequest(string url)
        {
            Url = url;
            Method = HttpMethod.Delete;
        }
    }

    public abstract class Request
    {
        public string Url { get; set; }
        public HttpMethod Method { get; protected set; }
        public object Body { get; set; }
        public Headers Headers { get; set; }
        public Query Query { get; set; }
    }
}
