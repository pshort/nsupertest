
using System.Collections.Generic;

namespace NSuperTest.Models
{
    public class ErrorList : Dictionary<string, List<string>>
    {

    }

    public class BadRequestResponse
    {
        public string Type { get; set; }
        public string Title { get; set; }
        public int Status { get; set; }
        public string TraceId { get; set; }
        public ErrorList Errors { get; set; }
    }
}