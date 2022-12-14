using BackupFiles.CopyFile;
using BackupFiles.CreatingSettingsFile;
using NLog;
using NLog.Config;

LogManager.Configuration = new XmlLoggingConfiguration(@"NLog.config");

//-----------------Создание_файла_настроек-----------------
while (true)
{
    Console.WriteLine("Создать файл настроек? YES | NO | EXIT");
    string? flag = Console.ReadLine();
    flag = flag.ToLower();
    switch (flag)
    {
        case "yes":
            string? str;
            string targetFolder = "error";
            string loggingLevel = "error";
            string fileName = "error";

            Console.WriteLine("Количество исходных папок: ");
            int sizeSourceFolders = Convert.ToInt32(Console.ReadLine());
            string[] sourceFolders = new string[sizeSourceFolders];

            Console.WriteLine("Массив путей исходных папок: ");
            for (int i = 0; i < sizeSourceFolders; i++)
            {
                str = Console.ReadLine();
                if (str != null) { sourceFolders[i] = str; }
            }

            Console.WriteLine("Путь к целевой папке");
            str = Console.ReadLine();
            if (str != null) { targetFolder = str; }

            Console.WriteLine("Уровень логирования (debug, info, error)");
            str = Console.ReadLine();
            if (str != null) { loggingLevel = str; }

            Console.WriteLine("Путь файла настроек");
            str = Console.ReadLine();
            if (str != null) { fileName = str; }

            ClassSettingFile settingFile = new(sourceFolders, targetFolder, loggingLevel);
            settingFile.CreatFileJson(fileName);

            bool way = true;
            while (way)
            {
                Console.WriteLine("Перейти к копированию? YES | NO | EXIT");
                flag = Console.ReadLine();
                flag = flag.ToLower();

                switch (flag)
                {
                    case "yes": await CopyFiles(); break;
                    case "no": way = false; break;
                    case "exit": return;
                    default: break;
                }
            }
            break;
        case "no":
            way = true;
            while (way)
            {
                Console.WriteLine("Перейти к копированию? YES | NO | EXIT");
                flag = Console.ReadLine();
                flag = flag.ToLower();

                switch (flag)
                {
                    case "yes": await CopyFiles(); break;
                    case "no": way = false; break;
                    case "exit": return;
                    default: break;
                }
            }
            break;
        case "exit": return;
        default: continue;
    }
}

//--------------------------копирование------------------------
async Task<bool> CopyFiles(){
    Console.WriteLine("Блок копирования файлов");
    string? str;
    ClassSettingFile settingFile;
    string fileName = "error";

    Console.WriteLine("Путь к файлу настроек:");
    str = Console.ReadLine();
    if (str != null) { fileName = str; }
    settingFile = new();
    await settingFile.ReadFileJson(fileName);
    LogLevel level = settingFile.GetLoggingLevel();
    LogManager.Configuration.AddRule(level, level, "bflog", "*");
    Logger LOG = LogManager.GetCurrentClassLogger();

    Console.WriteLine("\nМассив путей исходных папок: ");
    for (int i = 0; i < settingFile.SourceFolders.Length; i++)
    {
        Console.WriteLine($"Папка {i + 1}: {settingFile.SourceFolders[i]}");
        LOG.Debug($"Вывод пути папки {settingFile.SourceFolders[i]} на консоль");
    }

    Console.WriteLine($"\nПуть к целевой папке: {settingFile.TargetFolder}");
    LOG.Debug($"Вывод пути папки {settingFile.TargetFolder} на консоль");

    Console.WriteLine($"\nУровень логирования: {settingFile.LoggingLevel}");
    LOG.Debug($"Вывод информации об уровне логирования на консоль");
    LOG.Info($"Вывод информации из файла настроек");

    Console.WriteLine($"\nНачалось копирование файлов: {DateTime.Now}");
    LOG.Debug($"Начало копированя");
    LOG.Info($"Начало копированя");

    ClassCopyFile.CopyDirectories(settingFile.SourceFolders, settingFile.TargetFolder);

    Console.WriteLine($"\nКопирование в целевую папку: {settingFile.TargetFolder} завершено: {DateTime.Now}");
    LOG.Debug($"Окончание копированя");
    LOG.Info($"Окончание копированя");

    LOG.Info($"Завершение программы");
    return true;
}