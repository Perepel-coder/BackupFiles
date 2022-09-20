using NLog;

namespace BackupFiles.CopyFile
{
    internal static class ClassCopyFile
    {
        private static Logger LOG = LogManager.GetCurrentClassLogger();

        public static void CopyDirectory(string sourceFolder, string targetFolder)
        {      
            try
            {
                sourceFolder = PullOutLink(sourceFolder);
                Directory.CreateDirectory($"{targetFolder}\\{Path.GetFileName(sourceFolder)}_copy");

                foreach (string file in Directory.GetFiles(sourceFolder))
                {
                    string target = $"{targetFolder}\\{Path.GetFileName(sourceFolder)}_copy\\{Path.GetFileName(file)}";
                    File.Copy(file, target);
                    LOG.Debug($"Скопирован файл {file} в папку {target}");
                }
                foreach (string folder in Directory.GetDirectories(sourceFolder))
                {
                    string myFolder = PullOutLink(folder);
                    CopyDirectory(myFolder, $"{targetFolder}\\{Path.GetFileName(sourceFolder)}_copy");
                }
            }
            catch (Exception ex)
            {
                LOG.Error($"Исключение {ex.Message} | Метод: {ex.TargetSite} | Трассировка стека: {ex.StackTrace}");
                Console.WriteLine($"Ошибка выполнения. Копирование прервано. Исключение {ex.Message}");
                Environment.Exit(0);
            }
        }
        public static void CopyDirectories(string[] sourceFolders, string targetFolder)
        {
            string timeNow = DateTime.Now.ToString().Replace(":", "_");
            LOG.Debug($"Определено текущее время");
            var oldTargetFolder = targetFolder;
            targetFolder = $"{targetFolder}\\Backup_{timeNow}";
            foreach (var sourceFolder in sourceFolders)
            {
                try 
                {
                    if (sourceFolder == oldTargetFolder) { throw new Exception("Имена исходной и конечной папок совпадают"); }
                    string actualSourceFolder = PullOutLink(sourceFolder);
                    Directory.CreateDirectory($"{targetFolder}\\{Path.GetFileName(actualSourceFolder)}_copy");

                    foreach (string file in Directory.GetFiles(actualSourceFolder))
                    {
                        string target = $"{targetFolder}\\{Path.GetFileName(actualSourceFolder)}_copy\\{Path.GetFileName(file)}";
                        File.Copy(file, target);
                        LOG.Debug($"Скопирован файл {file} в папку {target}");
                    }
                    foreach (string folder in Directory.GetDirectories(actualSourceFolder))
                    {
                        string myFolder = PullOutLink(folder);
                        CopyDirectory(myFolder, $"{targetFolder}\\{Path.GetFileName(actualSourceFolder)}_copy");
                    }
                    LOG.Info($"Обработана папка {sourceFolder}");
                }
                catch (Exception ex)
                {
                    LOG.Error($"Исключение{ex.Message} | Метод: {ex.TargetSite} | Трассировка стека: {ex.StackTrace}");
                    Console.WriteLine($"Ошибка выполнения. Копирование прервано. Исключение {ex.Message}");
                    Environment.Exit(0);
                }
            }
            
        }

        public static string PullOutLink(string sourceFolder)
        {
            try
            {
                if (sourceFolder.IndexOf(".lnk") >= 0)
                {
                    IWshRuntimeLibrary.WshShell shell = new IWshRuntimeLibrary.WshShell();
                    LOG.Debug($"Определен путь файла: {sourceFolder}");
                    return (((IWshRuntimeLibrary.IWshShortcut)shell.CreateShortcut(sourceFolder)).TargetPath);
                }
                else
                {
                    LOG.Debug($"Определен путь файла: {sourceFolder}");
                }
            }
            catch (Exception ex)
            {
                LOG.Error($"Исключение{ex.Message} | Метод: {ex.TargetSite} | Трассировка стека: {ex.StackTrace}");
                Console.WriteLine($"Ошибка выполнения. Копирование прервано. Исключение{ex.Message}");
                Environment.Exit(0);
            }
            LOG.Info($"Проверка пути к файлу");
            return sourceFolder;
        }
    }
}
