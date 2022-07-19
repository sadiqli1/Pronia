using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Pronia.Extensions
{
    public static class FileValidator
    {
        public static async Task<string> FileCreate(this IFormFile file, string root, string folder)
        {
            string filename = string.Concat(Guid.NewGuid(), file.FileName);
            string path = Path.Combine(root, folder);
            string filePath = Path.Combine(path, filename);
            try
            {
                using (FileStream stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            catch (Exception)
            {
                throw new FileLoadException();
            }
            return filename;
        }
        public static void FileDelete(string root, string folder, string image)
        {
            string filePath = Path.Combine(root, folder, image);
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }
        }
        public static bool ImageisOkay(this IFormFile file, int mb)
        {
            return file.Length / 1024 / 1024 < mb && file.ContentType.Contains("image/");
        }
    }
}
