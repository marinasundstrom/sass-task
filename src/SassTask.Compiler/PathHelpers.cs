using System.IO;

namespace Sass
{
    public static class PathHelpers
    {
        public static string ToRootedPath(string path, string directory)
        {
            if (!Path.IsPathRooted(path))
            {
                path = Path.Combine(
                    directory,
                    path);
            }
            return path;
        }
    }
}
