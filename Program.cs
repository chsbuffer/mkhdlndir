using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace mkhdlndir
{
    public static class Program
    {
        private static void Main(string[] args)
        {
            if (args == null || args.Length < 2)
            {
                PrintHelp();
                return;
            }

            var link = Path.GetFullPath(args[0]);
            var target = Path.GetFullPath(args[1]);

            if (!CheckTargetDirectoryExist(target) || !CreateAndCheckLinkDirectory(link))
                return;

            try
            {
                MakeHardLinkDirectory(link, target);
                Console.WriteLine("Hard Link created for {0} <<-->> {1}", link, target);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static void PrintHelp()
        {
            var name = Process.GetCurrentProcess().MainModule?.ModuleName;
            Console.WriteLine("Usage: {0} Link Target", name);
        }

        private static void MakeHardLinkDirectory(string link, string target)
        {
            var stack = new Stack<string>();
            stack.Push(target);

            while (stack.Any())
            {
                var dirInfo = new DirectoryInfo(stack.Pop());

                foreach (var directory in dirInfo.GetDirectories().Select(dir => dir.FullName))
                {
                    Directory.CreateDirectory(directory.Replace(target, link));

                    stack.Push(directory);
                }

                foreach (var file in dirInfo.GetFiles().Select(file => file.FullName))
                    Utils.CreateHardLink(file.Replace(target, link), file);
            }
        }

        #region Check

        private static bool CreateAndCheckLinkDirectory(string link)
        {
            if (!Directory.Exists(link))
                Directory.CreateDirectory(link);

            if (!Utils.IsDirectoryEmpty(link))
            {
                Console.WriteLine("Link Directory must be empty");
                return false;
            }

            return true;
        }

        private static bool CheckTargetDirectoryExist(string target)
        {
            if (!Directory.Exists(target))
            {
                Console.WriteLine("Target Directory not Exist!");
                return false;
            }

            if (Utils.IsDirectoryEmpty(target))
            {
                Console.WriteLine("Target Directory is empty!");
                return false;
            }

            return true;
        }

        #endregion
    }
}