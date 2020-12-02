
using System.Collections.Generic;
using System.Text;

namespace NSuperTest.Assertions
{
    public class SchemaValidationException : System.Exception
    {
        private IList<string> _errors;
        public SchemaValidationException(IList<string> errors) : base(message: BuildMessage(errors))
        {
            _errors = errors;
        }

        private static string BuildMessage(IList<string> errors)
        {
            var builder = new StringBuilder();

            builder.AppendLine("Response message has following Json Schema Errors:");
            builder.AppendLine("=======================");

            foreach(var error in errors)
            {
                builder.AppendLine(error);
            }

            return builder.ToString();
        }
    }
}