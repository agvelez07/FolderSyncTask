using System;
using System.IO;
using System.Timers;

class FolderSync
{
    private static string sourcePath;
    private static string replicaPath;
    private static string logFilePath;
    private static double syncInterval;
    private static System.Timers.Timer syncTimer;

    static void Main(string[] args)
    {
        do
        {
            Console.Write("Folder Source Path: ");
            sourcePath = Console.ReadLine();

            if (Directory.Exists(sourcePath))
            {
                Console.Write("Replica Path: ");
                replicaPath = Console.ReadLine();

                if (Directory.Exists(replicaPath))
                {
                    Console.Write("Replica Path: ");
                    replicaPath = Console.ReadLine();


                }
                else
                {
                  
                    Console.WriteLine("Replica Path Does Not Exist!");
                    Console.Write("Do you wish to create a Replica Path? (y/N)");
                    string createReplicaPath = Console.ReadLine();

                    switch (createReplicaPath) { }

                }

            }
            else
            {
                Console.WriteLine("Source Path Does Not Exist!");
            }

        } while (true);
    }


   

    private static void SyncDirectory(DirectoryInfo source, DirectoryInfo replica)
    {
       
    }

   
}
