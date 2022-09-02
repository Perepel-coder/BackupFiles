
namespace BackupFiles.CopyFile
{
    internal static class ClassCopyFile
    {
        public static void CopyDirectory(string sourceFolder, string targetFolder)
        {
            sourceFolder = PullOutLink(sourceFolder);
            if (Directory.Exists(sourceFolder))
            {
                string timeNow = DateTime.Now.ToString().Replace(":", "_");
                Directory.CreateDirectory($"{targetFolder}\\{Path.GetFileName(sourceFolder)}_copy");

                foreach (string file in Directory.GetFiles(sourceFolder))
                {
                    string newFile = $"{targetFolder}\\{Path.GetFileName(sourceFolder)}_copy\\{Path.GetFileName(file)}";
                    File.Copy(file, newFile);
                }
                foreach (string folder in Directory.GetDirectories(sourceFolder))
                {
                    string myFolder = PullOutLink(folder);
                    CopyDirectory(myFolder, $"{targetFolder}\\{Path.GetFileName(sourceFolder)}_copy");
                }
            }
            else
            {
                Console.WriteLine("Файл не существует. Копирование прервано");
                Environment.Exit(0);
            }
        }
        public static void CopyDirectories(string[] sourceFolders, string targetFolder)
        {
            string timeNow = DateTime.Now.ToString().Replace(":", "_");
            targetFolder = $"{targetFolder}\\Backup_{timeNow}";
            foreach (var sourceFolder in sourceFolders)
            {
                string actualSourceFolder = PullOutLink(sourceFolder);
                if (Directory.Exists(actualSourceFolder))
                {
                    Directory.CreateDirectory($"{targetFolder}\\{Path.GetFileName(actualSourceFolder)}_copy");

                    foreach (string file in Directory.GetFiles(actualSourceFolder))
                    {
                        File.Copy(file, $"{targetFolder}\\{Path.GetFileName(actualSourceFolder)}_copy\\{Path.GetFileName(file)}");
                    }
                    foreach (string folder in Directory.GetDirectories(actualSourceFolder))
                    {
                        string myFolder = PullOutLink(folder);
                        CopyDirectory(myFolder, $"{targetFolder}\\{Path.GetFileName(actualSourceFolder)}_copy");
                    }
                }
                else
                {
                    Console.WriteLine("Папка не существует. Копирование прервано");
                    return;
                }
            }
            
        }

        public static string PullOutLink(string sourceFolder)
        {
            if (sourceFolder.IndexOf(".lnk") >= 0)
            {
                IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                return (((IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(sourceFolder)).TargetPath);
            }
            return sourceFolder;
        }
    }
}
