using FS88;
using System;

namespace AFIS.Reader.Service
{
    public class ReaderWCFService : IReaderWCFService
    {
        public ReaderServiceResponse GetFingerPrintWSQ()
        {
            try
            {
                if (Device.DeviceStatus != FS88.Enum.DeviceStatus.Stopped)
                    Device.CloseDevice();
                Device.OpenDevice();

                string wsq = Device.CaptureFingerprint(TimeSpan.FromSeconds(50));
                //string WSQ = Device.GenerateWSQ(Device.GetFrame()); 

                if (Device.DeviceStatus != FS88.Enum.DeviceStatus.Stopped)
                    Device.CloseDevice();
                return new ReaderServiceResponse()
                {
                    Status = true,
                    WSQ = wsq
                };
            }
            catch (FException ex)
            {
                return new ReaderServiceResponse()
                {
                    Status = false,
                    Error = ex.Message,
                    ErrorCode = 2,
                    InnerException = (ex.InnerException != null) ? ex.InnerException.Message : null
                };
            }
            catch (Exception ex)
            {
                return new ReaderServiceResponse()
                {
                    Status = false,
                    Error = ex.Message,
                    ErrorCode = 1,
                    InnerException = (ex.InnerException != null) ? ex.InnerException.Message : null
                };
            }
        }
    }
}
