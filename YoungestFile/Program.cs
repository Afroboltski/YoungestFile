namespace YoungestFile
{
    internal class Program
    {
        private static readonly string USAGE = "Usage: " + Environment.NewLine +
            "\tyf.exe -help"+Environment.NewLine+
                "\t\tDisplayes this help message." + Environment.NewLine +
            "\tyf.exe [DIRECTORY NAME] [MAX NUMBER OF FILES TO LIST]" + Environment.NewLine +
                "\t\t[DIRECTORY NAME] is the directory name to search. if ommitted, the current working directory is used." + Environment.NewLine +
                "\t\t[MAX NUMBER OF FILES TO LIST] limits the output in the console window to the specified number of files. If ommitted, all of them are displayed.";

        static int Main(string[] args)
        {

#if DEBUG
            if(args.Length == 0)
                args = new string[] {"C:\\temp", "10" };
#endif

            if(args.Length > 2)
            {
                Console.Error.WriteLine("Args are incorect. {0}", USAGE);
            }

            if (args.Length >= 1)
            {
                if (args[0].Equals("h", StringComparison.OrdinalIgnoreCase) ||
                    args[0].Equals("-h", StringComparison.OrdinalIgnoreCase) ||
                    args[0].Equals("--h", StringComparison.OrdinalIgnoreCase) ||
                    args[0].Equals("/h", StringComparison.OrdinalIgnoreCase) ||
                    args[0].Equals("help", StringComparison.OrdinalIgnoreCase) ||
                    args[0].Equals("-help", StringComparison.OrdinalIgnoreCase) ||
                    args[0].Equals("--help", StringComparison.OrdinalIgnoreCase) ||
                    args[0].Equals("/help", StringComparison.OrdinalIgnoreCase) ||
                    args[0].Equals("/?", StringComparison.OrdinalIgnoreCase) ||
                    args[0].Equals("-?", StringComparison.OrdinalIgnoreCase) ||
                    args[0].Equals("--?", StringComparison.OrdinalIgnoreCase) ||
                    args[0].Equals("?", StringComparison.OrdinalIgnoreCase))
                {
                    Console.Error.WriteLine(USAGE);
                    return 1;
                }
            }

            string path = Environment.CurrentDirectory;
            int maxCount = -1;

            switch (args.Length)
            {
                case 0:
                    path = Environment.CurrentDirectory;
                    break;
                case 1:
                    if (int.TryParse(args[0], out int tempMaxCount1))
                    {
                        if (tempMaxCount1 >= 0)
                        {
                            maxCount = tempMaxCount1;
                        }
                        else
                        {
                            maxCount = -1;
                        }
                        path = Environment.CurrentDirectory;
                    }
                    else
                    {
                        path = args[0];
                    }
                    break;
                case 2:
                    path = args[0];
                    if (int.TryParse(args[1], out int tempMaxCount2))
                    {
                        if(tempMaxCount2 >= 0)
                        {
                            maxCount = tempMaxCount2;
                        }
                        else
                        {
                            maxCount = -1;
                        }
                    }
                    else
                    {
                        Console.Error.WriteLine("Invalid maximum count \"{0}\". {1}{2}", path, Environment.NewLine, USAGE);
                        return 1;
                    }
                    break;
            }

            try
            {
                if (!FileValidator.ValidatePath(path, true, false))
                {
                    Console.Error.WriteLine("Invalid directory name \"{0}\". {1}{2}", path, Environment.NewLine, USAGE);
                    return 1;
                }

                Console.WriteLine("Most recently modified files (recursively searched) in \"{0}\":", path);

                DirectoryInfo folder = new DirectoryInfo(path);
                EnumerationOptions options = new EnumerationOptions();

                options.AttributesToSkip = FileAttributes.System | FileAttributes.Temporary | FileAttributes.Offline;
                options.IgnoreInaccessible = true;
                options.RecurseSubdirectories = true;
                options.MatchType = MatchType.Win32;

                IEnumerable<FileInfo> files = folder.EnumerateFiles("", options);
                IOrderedEnumerable<FileInfo> orderedFiles = files.OrderByDescending(fi => fi.LastWriteTime);

                int maxLen = -1;
                int i = 0;
                foreach (FileInfo file in orderedFiles)
                {
                    if (maxCount >= 0 && ++i > maxCount)
                    {
                        break;
                    }
                    if (file.Name.Length > maxLen)
                    {
                        maxLen = file.Name.Length;
                    }
                }

                i = 0;
                string title = string.Format("{0}   {1}\t{2}", "FILE NAME".PadRight(maxLen, ' '), "DATE/TIME MODIFIED".PadRight(DateTime.Now.ToShortDateString().Length + 1 + DateTime.Now.ToShortDateString().Length, ' '), "PATH TO FILE (Relative to \"" + (path.EndsWith('\\') ? path : (path + "\\")) + "\")");
                string sep = new string('-', title.Length + maxLen);
                Console.WriteLine(title);
                Console.WriteLine(sep);
                foreach (FileInfo file in orderedFiles)
                {
                    if (maxCount >= 0 && ++i > maxCount)
                    {
                        break;
                    }
                    string constLenName = file.Name.PadRight(maxLen, ' ');
                    string relativePath = Path.GetRelativePath(path, Path.GetDirectoryName(file.FullName));
                    Console.WriteLine("{0}   {1} {2}\t{3}", constLenName, file.LastWriteTime.ToShortDateString(), file.LastWriteTime.ToShortTimeString(), relativePath);
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("An unexpected error occurred: {0}",ex.Message);
                Console.WriteLine("Program will exit.");
                return -1;
            }

            return 0;
        }
    }
}