using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

namespace Funbit.Ets.Telemetry.Server.Controllers
{
    public class StaticFileController : ApiController
    {
        const string BaseDirectory = "Html";

        protected HttpResponseMessage ServeStaticFile(string directory, string fileName)
        {
            // basic safety check (do not serve files outside base www directory)
            var path = directory + fileName;
            if (path.Contains("..") || path.Contains(":") || path.Contains("//") || path.Contains(@"\\"))
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Page not found!");

            string extension = Path.GetExtension(fileName);
            string contentType;
            switch (extension)
            {
                case ".htm":
                case ".html":
                    {
                        contentType = "text/html";
                        break;
                    }
                case ".jpg":
                    {
                        contentType = "image/jpeg";
                        break;
                    }
                case ".png":
                    {
                        contentType = "image/png";
                        break;
                    }
                case ".css":
                    {
                        contentType = "text/css";
                        break;
                    }
                case ".js":
                    {
                        contentType = "application/javascript";
                        break;
                    }
                default:
                {
                    contentType = "application/octet-stream";
                    break;
                }
            }

            try
            {
                string absoluteFileName = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,
                    BaseDirectory, directory, fileName);
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StreamContent(
                    File.Open(absoluteFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                return response;
            }
            catch 
            {
                return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Server error.");
            }
        }
    }
}