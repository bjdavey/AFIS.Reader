using System.ServiceModel;
using System.ServiceModel.Web;

namespace AFIS.Reader.Service
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IReaderService" in both code and config file together.
    [ServiceContract]
    public interface IReaderWCFService
    {
        //[WebInvoke(Method = "GET", ResponseFormat = WebMessageFormat.Json, UriTemplate = "GetFingerPrintWSQ")]
        [OperationContract]
        [WebGet(ResponseFormat = WebMessageFormat.Json)]
        ReaderServiceResponse GetFingerPrintWSQ();
    }

    public class ReaderServiceResponse
    {
        public bool Status { get; set; }
        public string WSQ { get; set; }
        public string Error { get; set; }
        public int? ErrorCode { get; set; }
        public string InnerException { get; set; }
    }
}
