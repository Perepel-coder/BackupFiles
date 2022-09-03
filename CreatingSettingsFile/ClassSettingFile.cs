using System.Text.Json;
using NLog;

namespace BackupFiles.CreatingSettingsFile
{
    internal class ClassSettingFile
    {
        public string[] SourceFolders { get; set; }
        public string TargetFolder { get; set; }
        public string LoggingLevel { get; set; }

        public ClassSettingFile() { }
        public ClassSettingFile(string[] sourceFolders, string targetFolder, string loggingLevel) 
        {
            this.SourceFolders = sourceFolders;
            this.TargetFolder = targetFolder;
            this.LoggingLevel = loggingLevel;
        }
        public async void CreatFileJson(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                await JsonSerializer.SerializeAsync<ClassSettingFile>(fs, this);
            }
        }

        public async Task ReadFileJson(string fileName)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.OpenOrCreate))
            {
                var settingFile = await JsonSerializer.DeserializeAsync<ClassSettingFile>(fs);
                if (settingFile == null)
                {
                    Environment.Exit(0);
                }
                this.SourceFolders = settingFile.SourceFolders;
                this.TargetFolder = settingFile.TargetFolder;
                this.LoggingLevel = settingFile.LoggingLevel;
            }
        }

        public LogLevel GetLoggingLevel()
        {
            LoggingLevel = LoggingLevel.ToLower();
            switch (LoggingLevel)
            {
                case "debug": return LogLevel.Debug;
                case "info": return LogLevel.Info;
                case "error": return LogLevel.Error;
                default: 
                    Console.WriteLine("Уровень логирования не определен! Стандартный уровень логирования - Error");
                    return LogLevel.Error;
            }
        }
    }
}