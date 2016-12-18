using System;
using System.Text;
using System.Net;
using System.IO;
namespace ananstasiaanssistanttest
{
    class Program
    {
        static void Main(string[] args)
        {
            FtpWebRequest request = (FtpWebRequest)WebRequest.Create("ftp://waws-prod-bay-029.ftp.azurewebsites.windows.net/Avatars/IMG_2064.JPG");
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential("str1pyblog", "mSsEtm23NixhpbDTbtCqq2owmRELkHxenMFnNMe5w01gj2yHTB8Qm16ZJ6kT");

            Uri imagePlayUri = (new Uri("ftp://waws-prod-bay-029.ftp.azurewebsites.windows.net/Avatars/IMG_2064.JPG"));
           string asss = imagePlayUri.ToString();
            Console.WriteLine(asss);
            Console.ReadKey();
        }
    }
}
        

