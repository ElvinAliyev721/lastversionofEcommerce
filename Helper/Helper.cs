using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Code.Helper
{
    public static class Helper
    {
        public static void DeleteFile(string fileName, string root, string folder)
        {
            string resultPath = Path.Combine(root, folder, fileName);

            bool isExist = System.IO.File.Exists(resultPath);
            if (isExist)
            {
                System.IO.File.Delete(resultPath);
            }
        }
    }
}
