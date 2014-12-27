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
        protected HttpResponseMessage ServeStaticFile(string appName, string directory, string fileName)
        {
            // basic safety check
            if (fileName.Contains("..") || 
                fileName.Contains(":") || 
                fileName.Contains("//") || 
                fileName.Contains(@"\\"))
                throw new HttpResponseException(HttpStatusCode.BadRequest);

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
                    "Apps", appName, directory, fileName);
                var response = Request.CreateResponse(HttpStatusCode.OK);
                response.Content = new StreamContent(File.Open(absoluteFileName, FileMode.Open));
                response.Content.Headers.ContentType = new MediaTypeHeaderValue(contentType);
                return response;
            }
            catch 
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }
        }
    }
}