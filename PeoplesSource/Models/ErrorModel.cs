using System.Collections.Generic;

namespace PeoplesSource.Models
{

    public class ErrorModel
    {
        public ErrorModel()
        {
            ErrorParameters = new List<ErrorParameterModel>();
        }

        public string ErrorCategory { get; set; }
        public string ErrorDomain { get; set; }
        public long ErrorId { get; set; }
        public string ErrorExceptionId { get; set; }
        public string Message { get; set; }
        public string Severity { get; set; }
        public string Subdomain { get; set; }
        public List<ErrorParameterModel> ErrorParameters { get; set; }
    }

    public class ErrorParameterModel
    {
        public string Value { get; set; }
        public string Name { get; set; }
    }
}