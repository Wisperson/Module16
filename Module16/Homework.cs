using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Module16
{
    public class Homework
    {
        public static void Start()
        {
            Console.WriteLine("Welcome to Simple File Manager!");

            while (true)
            {
                Console.WriteLine("\nChoose an option:");
                Console.WriteLine("1. View directory contents");
                Console.WriteLine("2. Create file/directory");
                Console.WriteLine("3. Delete file/directory");
                Console.WriteLine("4. Copy file/directory");
                Console.WriteLine("5. Move file/directory");
                Console.WriteLine("6. Read from file");
                Console.WriteLine("7. Write to file");
                Console.WriteLine("8. Exit");

                int choice;
                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 1:
                            ViewDirectoryContents();
                            break;
                        case 2:
                            CreateFileOrDirectory();
                            break;
                        case 3:
                            DeleteFileOrDirectory();
                            break;
                        case 4:
                            CopyFileOrDirectory();
                            break;
                        case 5:
                            MoveFileOrDirectory();
                            break;
                        case 6:
                            ReadFromFile();
                            break;
                        case 7:
                            WriteToFile();
                            break;
                        case 8:
                            Environment.Exit(0);
                            break;
                        default:
                            Console.WriteLine("Invalid choice. Please try again.");
                            break;
                    }
                }
                else
                {
                    Console.WriteLine("Invalid input. Please enter a number.");
                }
            }
        }

        private static void ViewDirectoryContents()
        {
            Console.Write("Enter directory path: ");
            string path = Console.ReadLine();

            if (Directory.Exists(path))
            {
                string[] files = Directory.GetFiles(path);
                string[] directories = Directory.GetDirectories(path);

                Console.WriteLine($"Files in {path}:");
                foreach (string file in files)
                {
                    Console.WriteLine(Path.GetFileName(file));
                }

                Console.WriteLine($"Directories in {path}:");
                foreach (string directory in directories)
                {
                    Console.WriteLine(Path.GetFileName(directory));
                }
            }
            else
            {
                Console.WriteLine("Directory not found.");
            }
        }

        private static void CreateFileOrDirectory()
        {
            Console.Write("Enter path for new file/directory: ");
            string path = Console.ReadLine();

            Console.WriteLine("Choose type:");
            Console.WriteLine("1. File");
            Console.WriteLine("2. Directory");

            int choice;
            if (int.TryParse(Console.ReadLine(), out choice))
            {
                switch (choice)
                {
                    case 1:
                        File.Create(path).Close();
                        LogAction($"Created file: {path}");
                        Console.WriteLine("File created successfully.");
                        break;
                    case 2:
                        Directory.CreateDirectory(path);
                        LogAction($"Created directory: {path}");
                        Console.WriteLine("Directory created successfully.");
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid input. Please enter a number.");
            }
        }

        private static void DeleteFileOrDirectory()
        {
            Console.Write("Enter path to file/directory to delete: ");
            string path = Console.ReadLine();

            if (File.Exists(path))
            {
                File.Delete(path);
                LogAction($"Deleted file: {path}");
                Console.WriteLine("File deleted successfully.");
            }
            else if (Directory.Exists(path))
            {
                Directory.Delete(path, true);
                LogAction($"Deleted directory: {path}");
                Console.WriteLine("Directory deleted successfully.");
            }
            else
            {
                Console.WriteLine("File/directory not found.");
            }
        }

        private static void CopyFileOrDirectory()
        {
            Console.Write("Enter source path: ");
            string sourcePath = Console.ReadLine();

            Console.Write("Enter destination path: ");
            string destinationPath = Console.ReadLine();

            try
            {
                if (File.Exists(sourcePath))
                {
                    File.Copy(sourcePath, Path.Combine(destinationPath, Path.GetFileName(sourcePath)), true);
                    LogAction($"Copied file: {sourcePath} to {destinationPath}");
                    Console.WriteLine("File copied successfully.");
                }
                else if (Directory.Exists(sourcePath))
                {
                    CopyDirectory(sourcePath, Path.Combine(destinationPath, Path.GetFileName(sourcePath)));
                    LogAction($"Copied directory: {sourcePath} to {destinationPath}");
                    Console.WriteLine("Directory copied successfully.");
                }
                else
                {
                    Console.WriteLine("Source not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void MoveFileOrDirectory()
        {
            Console.Write("Enter source path: ");
            string sourcePath = Console.ReadLine();

            Console.Write("Enter destination path: ");
            string destinationPath = Console.ReadLine();

            try
            {
                if (File.Exists(sourcePath))
                {
                    File.Move(sourcePath, Path.Combine(destinationPath, Path.GetFileName(sourcePath)));
                    LogAction($"Moved file: {sourcePath} to {destinationPath}");
                    Console.WriteLine("File moved successfully.");
                }
                else if (Directory.Exists(sourcePath))
                {
                    Directory.Move(sourcePath, Path.Combine(destinationPath, Path.GetFileName(sourcePath)));
                    LogAction($"Moved directory: {sourcePath} to {destinationPath}");
                    Console.WriteLine("Directory moved successfully.");
                }
                else
                {
                    Console.WriteLine("Source not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void ReadFromFile()
        {
            Console.Write("Enter path to file to read: ");
            string path = Console.ReadLine();

            try
            {
                if (File.Exists(path))
                {
                    string content = File.ReadAllText(path);
                    Console.WriteLine($"Content of {path}:\n{content}");
                }
                else
                {
                    Console.WriteLine("File not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void WriteToFile()
        {
            Console.Write("Enter path to file to write: ");
            string path = Console.ReadLine();

            Console.WriteLine("Enter text to write to the file (press Ctrl + Z and Enter to finish):");

            try
            {
                using (StreamWriter sw = new StreamWriter(path))
                {
                    string line;
                    while ((line = Console.ReadLine()) != null)
                    {
                        sw.WriteLine(line);
                    }
                }

                LogAction($"Wrote to file: {path}");
                Console.WriteLine("Content written to file successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }

        private static void LogAction(string action)
        {
            try
            {
                using (StreamWriter sw = File.AppendText(action))
                {
                    sw.WriteLine($"{DateTime.Now} - {action}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error writing to log file: {ex.Message}");
            }
        }

        private static void CopyDirectory(string sourcePath, string destinationPath)
        {
            Directory.CreateDirectory(destinationPath);

            foreach (string file in Directory.GetFiles(sourcePath))
            {
                string fileName = Path.GetFileName(file);
                string destFile = Path.Combine(destinationPath, fileName);
                File.Copy(file, destFile, true);
            }

            foreach (string subDirectory in Directory.GetDirectories(sourcePath))
            {
                string directoryName = Path.GetFileName(subDirectory);
                string destDirectory = Path.Combine(destinationPath, directoryName);
                CopyDirectory(subDirectory, destDirectory);
            }
        }
    }
}
