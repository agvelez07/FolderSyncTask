using System;
using System.IO;
using System.Timers;
using TestTask;

class FolderSync
{
    private static string sourcePath;
    private static string replicaPath;
    private static string logFilePath;
    private static double syncInterval;
    private static System.Timers.Timer syncTimer; 

    static void Main(string[] args)
    {
        // Check if the correct number of command-line arguments is provided
        if (args.Length < 4)
        {
            Console.WriteLine("Usage: FolderSync <sourcePath> <replicaPath> <syncInterval> <logFilePath>");
            return;
        }

        // Assign command-line arguments
        sourcePath = args[0]?.Trim();
        replicaPath = args[1]?.Trim();
        logFilePath = args[3]?.Trim();

        // Validate the synchronization interval
        if (!double.TryParse(args[2], out syncInterval) || syncInterval <= 0)
        {
            Console.WriteLine("Invalid synchronization interval. It should be a positive number.");
            return;
        }

        // Check if the source path exists
        if (!Directory.Exists(sourcePath))
        {
            Console.WriteLine("Source Path Does Not Exist!");
            return;
        }

        // Check if the replica path exists
        if (!Directory.Exists(replicaPath))
        {
            Console.WriteLine("Replica Path Does Not Exist!");
            Console.Write("Do you wish to create a new replica? (y/n): ");
            string option = Console.ReadLine()?.Trim().ToLower();

            if (option == "y" || option == "yes")
            {
                // Create the replica directory and copy files from the source
                Directory.CreateDirectory(replicaPath);
                CopyFilesRecursively();
            }
            else
            {
                // Exit if the user does not want to create the replica path
                Console.WriteLine("Replica Path not created. Exiting.");
                return;
            }
        }

        // Set up and start the timer to perform synchronization at specified intervals
        syncTimer = new System.Timers.Timer(syncInterval); // Fully qualify the Timer class
        syncTimer.Elapsed += OnTimedEvent; // Assign the method to be called when the timer elapses
        syncTimer.AutoReset = true; // Set the timer to automatically restart after each interval.
        syncTimer.Enabled = true; // Enable the timer.

        // Notify the user that the synchronization service has started.
        Console.WriteLine("Synchronization service started. Press [Enter] to exit.");
        Console.ReadLine();  // Keep the program running until the user presses Enter
    }

    // Method called when the timer elapses
    private static void OnTimedEvent(object source, ElapsedEventArgs e)
    {
        Console.WriteLine("Synchronization started...");
        Logger.Log(logFilePath, "Synchronization started."); // Log the start of synchronization

        try
        {
            // Update the replica directory to match the source directory
            UpdateReplicaDirectory();
            Logger.Log(logFilePath, "Synchronization completed successfully."); // Log successful completion
            Console.WriteLine("Synchronization completed.");
        }
        catch (Exception ex)
        {
            // Log errors that occur during synchronization
            Logger.Log(logFilePath, $"Error during synchronization: {ex}");
            Console.WriteLine($"Error during synchronization: {ex}");
        }
    }

    // Method to copy files and directories
    private static void CopyFilesRecursively()
    {
        try
        {
            // Copy all directories
            foreach (string dirPath in Directory.GetDirectories(sourcePath, "*", SearchOption.AllDirectories))
            {
                string targetDirPath = dirPath.Replace(sourcePath, replicaPath);
                Directory.CreateDirectory(targetDirPath);
            }

            // Copy all files
            foreach (string filePath in Directory.GetFiles(sourcePath, "*.*", SearchOption.AllDirectories))
            {
                string targetFilePath = filePath.Replace(sourcePath, replicaPath);
                File.Copy(filePath, targetFilePath, true); // Overwrite existing files
                Logger.Log(logFilePath, $"Copied '{filePath}' to '{targetFilePath}'."); // Log file copy
            }

            Console.WriteLine("Synchronization Completed!");
        }
        catch (Exception ex)
        {
            // Log any errors that occur during file copy
            Logger.Log(logFilePath, $"Error during file copy: {ex}");
            throw; //throw the exception to be caught in the OnTimedEvent method
        }
    }

    // Method to update the replica directory to match the source directory
    private static void UpdateReplicaDirectory()
    {
        // First, copy files from source to replica
        CopyFilesRecursively();

        // Remove files in the replica that no longer exist in the source
        foreach (string targetFilePath in Directory.GetFiles(replicaPath, "*.*", SearchOption.AllDirectories))
        {
            string sourceFilePath = targetFilePath.Replace(replicaPath, sourcePath);

            if (!File.Exists(sourceFilePath))
            {
                File.Delete(targetFilePath); // Delete file if it does not exist in source
                Logger.Log(logFilePath, $"Deleted '{targetFilePath}'."); // Log file deletion
            }
        }

        // Remove directories in the replica that no longer exist in the source
        foreach (string targetDirPath in Directory.GetDirectories(replicaPath, "*", SearchOption.AllDirectories))
        {
            string sourceDirPath = targetDirPath.Replace(replicaPath, sourcePath);

            if (!Directory.Exists(sourceDirPath))
            {
                Directory.Delete(targetDirPath, true); // Delete directory and its contents
                Logger.Log(logFilePath, $"Removed directory '{targetDirPath}'."); // Log directory removal
            }
        }

        Console.WriteLine("Update Completed!");
    }
}
