using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.IO;
using System.Web.Http;
using System.Net;

namespace DM.AbpZeroTemplate.HttpFormatters
{
    public class UploadMultipartMediaTypeFormatter<T> : MediaTypeFormatter
    {
        public UploadMultipartMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("multipart/form-data"));
        }

        public override bool CanReadType(Type type)
        {
            return type == typeof(FileUpload<T>);
        }

        public override bool CanWriteType(Type type)
        {
            return false;
        }
        public async override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {

            if (!content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }

            var Parts = await content.ReadAsMultipartAsync();
            var FileContent = Parts.Contents.First(x =>
                x.Headers.ContentDisposition.Name == "\"file\"");

            var DataString = "";
            foreach (var Part in Parts.Contents.Where(x => x.Headers.ContentDisposition.DispositionType == "form-data" && x.Headers.ContentDisposition.Name == "\"data\""))
            {
                var Data = await Part.ReadAsStringAsync();
                DataString = Data;
            }

            string FileName = FileContent.Headers.ContentDisposition.FileName;
            string MediaType = FileContent.Headers.ContentType.MediaType;

            using (var Imgstream = await FileContent.ReadAsStreamAsync())
            {
                byte[] Imagebuffer = ReadFully(Imgstream);
                return new FileUpload<T>(Imagebuffer, MediaType, FileName, DataString);
            }
        }

        private byte[] ReadFully(Stream input)
        {
            var Buffer = new byte[16 * 1024];
            using (var Ms = new MemoryStream())
            {
                int Read;
                while ((Read = input.Read(Buffer, 0, Buffer.Length)) > 0)
                {
                    Ms.Write(Buffer, 0, Read);
                }
                return Ms.ToArray();
            }
        }
    }
}
