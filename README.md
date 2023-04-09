
### Fingerprint reader service for most of Futronic Fingerprint Scanners

NOTE: Works on **Microsoft Windows ONLY !!**

1) Install AFIS.Reader.Service.Installer.msi from https://github.com/bjdavey/AFIS.Reader/releases/latest/download/AFIS.Reader.Service.Installer.msi

2) **GetFingerPrintWSQ**: Use this method to read the fingerprint image from the device in WSQ format.

•	URL  
/AFIS/GetFingerPrintWSQ

•	Full URL  
http://localhost:8000/AFIS/GetFingerPrintWSQ

•	Method  
GET

•	Content-Type  
none

•	URL Parameters  
none

•	Data Parameters  
none 

•	Response:  
Error: (Error Message | null)  
ErrorCode: (Error Code | null)  
InnerException: (Exception Details | null)  
Status: (true | false) *indicates if the request succeeded  
WSQ:  (WSQ string of the first fingerprint image)
