using System;
using System.IO;
using System.Timers;

class FolderSync
{
    private static string sourcePath;
    private static string replicaPath;
    private static double syncInterval;
    private static System.Timers.Timer syncTimer;

    static void Main(string[] args)
    {
        Console.Write("Folder Source Path: ");
        sourcePath = Console.ReadLine();

        if (Directory.Exists(sourcePath))
        {
            Console.Write("Replica Path: ");
            replicaPath = Console.ReadLine();

            if (!Directory.Exists(replicaPath))
            {
                bool createReplicaPath = true;
                while (createReplicaPath)
                {
                    Console.WriteLine("Replica Path Does Not Exist!");
                    Console.Write("Do you wish to create a new replica? (y/N): ");
                    string option = Console.ReadLine()?.Trim().ToLower();

                    switch (option)
                    {
                        case "y":
                        case "yes":
                            CopyFilesRecursively(sourcePath, replicaPath);
                            createReplicaPath = false;
                            break;

                        case "n":
                        case "no":
                            createReplicaPath = false;
                            Console.Clear();
                            break;

                        default:
                            Console.WriteLine("Please insert 'y' or 'n'!");
                            break;
                    }
                }
            }
            else
            {
                Console.WriteLine("Replica Path already exists.");
                // Optional: Start periodic synchronization here
            }
        }
        else
        {
            Console.WriteLine("Source Path Does Not Exist!");
        }
    }

    private static void CopyFilesRecursively(string sourcePath, string targetPath)
    {
        try
        {
            Directory.CreateDirectory(targetPath);

            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                string targetDirPath = dirPath.Replace(sourcePath, targetPath);
                Directory.CreateDirectory(targetDirPath);
            }

            foreach (string filePath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                string targetFilePath = filePath.Replace(sourcePath, targetPath);
                File.Copy(filePath, targetFilePath, true);
                Console.WriteLine($"Copied '{filePath}' to '{targetFilePath}'.");
            }

            Console.WriteLine("Synchronization Completed!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error during synchronization: {ex.Message}");
        }
    }
}
