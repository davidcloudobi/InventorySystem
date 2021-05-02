using System;
using System.Drawing;
using System.IO;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace Domain.Helper
{
    public class FileImageServices
    {

        public async Task<ImageProcess> Add(IFormFile formFile, string name)
        {
            if (formFile == null) return null;
            if (formFile.Length <= 0) return null;
            var folder = @"C:\Shopify\Images\";
            if (!Directory.Exists(folder))
                Directory.CreateDirectory(folder);
            var filePath = Path.Combine(folder, name + "-" + Guid.NewGuid() + "-" + formFile.FileName);
            if (File.Exists(filePath))
            {
                filePath = Path.Combine(folder, "-" + Guid.NewGuid() + formFile.FileName);
                if (File.Exists(filePath)) return null;
                await using var stream = File.Create(filePath);
                await formFile.CopyToAsync(stream);
            }
            else
            {
                await using var stream = File.Create(filePath);
                await formFile.CopyToAsync(stream);
            }

            //var url =  CloudUpload(filePath);

            //  var url =  await OracleObjectStorage.Upload(filePath, name, formFile.FileName);
            //CloudinaryUrl = url,
            return new ImageProcess()
            {
                FilePath = filePath
            };
        }


        public static byte[] GetSingle(string imgString)
        {
            if (imgString == null) return null;
            Image img = Image.FromFile(imgString);
            using var ms = new MemoryStream();
            img.Save(ms, img.RawFormat);
            return ms.ToArray();

        }

        private string CloudUpload(string filepath)
        {

            var name = Environment.GetEnvironmentVariable("Cloudinary_Name");
            var key = Environment.GetEnvironmentVariable("Cloudinary_Apikey");
            var secret = Environment.GetEnvironmentVariable("Cloudinary_Secret");

            Account account = new Account(name, key, secret);

            Cloudinary cloudinary = new Cloudinary(account);
            var uploadParams = new ImageUploadParams()
            {
                File = new FileDescription(filepath)
            };
            var uploadResult = cloudinary.Upload(uploadParams);
            return uploadResult.Url.ToString();

        }
    }

    public class ImageProcess
    {
        public string FilePath { get; set; }
        // public string CloudinaryUrl { get; set; }
    }
}
