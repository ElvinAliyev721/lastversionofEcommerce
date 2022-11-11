using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Code.Extentions
{
    public static class FileManager
    {
        public static bool CheckFile(this IFormFile file, string type)
        {
            return file.ContentType.Contains(type);
        }
        public static bool CheckFileSize(this IFormFile file, int size)
        {
            return ((file.Length / 1024) > size);
        }
        public static async Task<string> SaveFile(this IFormFile file, string fileroot, string folder)
        {
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + Guid.NewGuid().ToString() + "_" + file.FileName;

            string resultPath = Path.Combine(fileroot, folder, fileName);

            using (FileStream fileStream = new FileStream(resultPath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return fileName;
        }
    }
}
