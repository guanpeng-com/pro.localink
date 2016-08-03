using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DM.AbpZeroTemplate.HttpFormatters
{
    public class FileUpload<T>
    {
        private readonly string _RawValue;

        public T Value { get; set; }
        public string FileName { get; set; }
        public string MediaType { get; set; }
        public byte[] Buffer { get; set; }

        public FileUpload(byte[] buffer, string mediaType, string fileName, string value)
        {
            Buffer = buffer;
            MediaType = mediaType;
            FileName = fileName.Replace("\"", "");
            _RawValue = value;

            Value = JsonConvert.DeserializeObject<T>(_RawValue);
        }

        public void Save(string path, string fileName = null)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            var NewPath = string.Empty;
            if (string.IsNullOrEmpty(fileName))
                NewPath = Path.Combine(path, FileName);
            else
                NewPath = Path.Combine(path, fileName);
            if (File.Exists(NewPath))
            {
                File.Delete(NewPath);
            }

            File.WriteAllBytes(NewPath, Buffer);
        }
    }
}
