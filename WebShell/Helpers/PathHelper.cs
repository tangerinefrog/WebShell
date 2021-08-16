using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace WebShell.Helpers
{
    public static class PathHelper
    {
        public static string ProcessPath(string path, string currentPath)
        {
            var temp = Path.GetFullPath(
                Path.Combine(currentPath, path)
            );

            if (Directory.Exists(temp))
            {
                return temp;
            }

            if (Path.IsPathRooted(path) && Directory.Exists(path))
            {
                return Path.GetFullPath(path);
            }

            return null;
        }

    }
}
