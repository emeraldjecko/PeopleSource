using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Security;

namespace PeoplesSource.Helpers
{
    public static class PdfHelper
    {

        public static byte[] WkHtmlToPdf(string url, string wkHtmlDir, bool delay)
        {
            //// Get the authentication cookie
            var cookieName = FormsAuthentication.FormsCookieName;
            var authCookie = HttpContext.Current.Request.Cookies[cookieName];

            const string fileName = " - ";
            var wkHtmlExe = wkHtmlDir + "\\wkhtmltopdf.exe";
            if (delay)
                wkHtmlExe = wkHtmlDir + "\\wkhtmltopdf_0_11_rc2.exe";

            var p = new Process
            {
                StartInfo =
                {
                    CreateNoWindow = true,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    RedirectStandardInput = true,
                    UseShellExecute = false,
                    FileName = wkHtmlExe,
                    WorkingDirectory = wkHtmlDir
                }
            };

            var switches = "";
            switches += "--print-media-type ";
            if (authCookie != null) switches += string.Format("--cookie {0} {1} ", authCookie.Name, authCookie.Value);
            switches += "--margin-top 20mm --margin-bottom 10mm --margin-right 10mm --margin-left 10mm ";
            switches += "--page-size A4 ";
            //switches += "--header-html http://" + Request.Url.Authority + "/Content/html/header.html ";
            if (delay)
                switches += "--javascript-delay 1000";
            p.StartInfo.Arguments = switches + " \"" + url + "\" " + fileName;
            p.Start();

            //read output
            var buffer = new byte[32768];
            byte[] file;
            using (var ms = new MemoryStream())
            {
                while (true)
                {
                    var read = p.StandardOutput.BaseStream.Read(buffer, 0, buffer.Length);

                    if (read <= 0)
                    {
                        break;
                    }
                    ms.Write(buffer, 0, read);
                }
                file = ms.ToArray();
            }

            // wait or exit
            p.WaitForExit(60000);

            // read the exit code, close process
            var returnCode = p.ExitCode;
            p.Close();

            var filePath = wkHtmlDir + "\\log.txt";
            FileHelper.WriteLine(filePath, string.Format("Url: {0}", url));
            FileHelper.WriteLine(filePath, string.Format("ReturnCode: {0}", returnCode.ToString(CultureInfo.InvariantCulture)));
            FileHelper.WriteLine(filePath, string.Format("File Length: {0}", (file.Length)));


            return returnCode == 0 ? file : null;
        }

    }
}