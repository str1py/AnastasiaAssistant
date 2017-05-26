using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AnastasiaAssistantProject.MainAppLogic
{
    class OpenFTPconnection
    {

            public readonly string Login = "str1pyblog";
            public readonly string Password = "mSsEtm23NixhpbDTbtCqq2owmRELkHxenMFnNMe5w01gj2yHTB8Qm16ZJ6kT";


            void UploadFile()
            {

            }
            protected FtpWebRequest ConnectionForDownloadFile(string url, string file)
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url + file);
                request.Credentials = new NetworkCredential(Login.Normalize(), Password.Normalize());
                request.UseBinary = true;
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.KeepAlive = true;
                return request;
            }     
    }
}
