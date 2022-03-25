using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace Mas.Common
{
    public static class FileExtentions
    {
        public static async Task<string> ToUpload(this IFormFile file, string dest)
        {
            string fileExt = Path.GetExtension(file.FileName);
            string fileName = $"{Guid.NewGuid()}{fileExt}";
            string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets","images",dest);
            string destPath = Path.Combine(folderPath, fileName);
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            using (var stream = File.Create(destPath))
            {
                await file.CopyToAsync(stream);
            }
            return $"/assets/images/{dest}/{fileName}";
        }

        public static async Task Remove(string path)
        {
            var isExist = File.Exists(path);
            if (isExist)
            {
                File.Delete(path);
                await Task.Yield();
            }
        }
    }
}
