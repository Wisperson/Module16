using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module16
{
    public class Practice
    {
        public static void Start()
        {
            Console.WriteLine("Welcome to File Change Logger!");

            // Получение пути к директории от пользователя
            Console.Write("Enter the directory path to monitor: ");
            directoryPath = Console.ReadLine();

            // Получение пути к лог-файлу от пользователя
            Console.Write("Enter the log file path: ");
            logFilePath = Console.ReadLine();

            // Начало отслеживания изменений
            StartFileMonitoring();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
        private static string directoryPath;
        private static string logFilePath;

        private static void StartFileMonitoring()
        {
            try
            {
                using (FileSystemWatcher watcher = new FileSystemWatcher())
                {
                    watcher.Path = directoryPath;

                    // Настройка отслеживаемых событий
                    watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;

                    // Подписка на события
                    watcher.Changed += OnChanged;
                    watcher.Created += OnChanged;
                    watcher.Deleted += OnChanged;
                    watcher.Renamed += OnRenamed;

                    // Включение отслеживания
                    watcher.EnableRaisingEvents = true;

                    Console.WriteLine($"Monitoring directory: {directoryPath}");
                    Console.WriteLine($"Logging changes to: {logFilePath}");

                    Console.WriteLine("Press 'q' to stop monitoring.");

                    while (true)
                    {
                        char key = Console.ReadKey().KeyChar;
                        if (key == 'q' || key == 'Q')
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void OnChanged(object source, FileSystemEventArgs e)
        {
            LogChange($"File {e.ChangeType}: {e.FullPath}");
        }

        private static void OnRenamed(object source, RenamedEventArgs e)
        {
            LogChange($"File renamed: {e.OldFullPath} to {e.FullPath}");
        }

        private static void LogChange(string logMessage)
        {
            try
            {
                // Запись изменений в лог-файл
                using (StreamWriter sw = File.AppendText(logFilePath))
                {
                    sw.WriteLine($"{DateTime.Now} - {logMessage}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }
    }
}
