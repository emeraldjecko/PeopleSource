using System.Collections.Generic;

namespace PeoplesSource.Models
{
    public class ErrorMessageModel
    {
        public ErrorMessageModel()
        {
            ErrorParameters = new List<ErrorMessageParamModel>();
        }

        public string ErrorClassificationCodeType { get; set; }
        public string ErrorCode { get; set; }
        public string LongMessage { get; set; }
        public string ShortMessage { get; set; }
        public string SeverityCodeType { get; set; }

        public List<ErrorMessageParamModel> ErrorParameters { get; set; }

    }

    public class ErrorMessageParamModel
    {
        public string Value { get; set; }
        public string ParamId { get; set; }
    }
}