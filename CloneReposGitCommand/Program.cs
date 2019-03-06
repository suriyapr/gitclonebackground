using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CloneReposGitCommand
{
    class Program
    {
        /// <summary>
        /// Main program
        /// </summary>
        /// <param name="args">args[0] holds the GIT installed path; args[1] holds txt file which holds the repos list; arg[2] -holds the branch name; args[3] holds the target folder.</param>
        static void Main(string[] args)
        {
            if (args.Length == 0)
                return;
            string errorString = string.Empty, outputString = string.Empty;
            string gitInstalledPath = args[0]; //args[0] holds the GIT installed path
            string reposListPath = args[1]; //args[1] holds txt file which holds the repos list
            string branchName = args[2]; // //arg[2] -holds the branch name
            if (Directory.Exists(gitInstalledPath))
            {
                try
                {
                    string[] fileNames = Directory.GetFiles(gitInstalledPath);
                    if (fileNames.Contains(gitInstalledPath + "\\git.exe"))
                    {
                        if (File.Exists(reposListPath))
                        {
                            ProcessStartInfo startInfo = new ProcessStartInfo(gitInstalledPath + "\\git.exe")
                            {
                                CreateNoWindow = false,
                                RedirectStandardError = true,
                                RedirectStandardOutput = true,
                                UseShellExecute = false
                            };
                            Console.WriteLine("Enter the action which you need?\n1. Clone\n2. Pull");
                            int choice = int.Parse(Console.ReadLine());
                            if (choice == 1 || choice == 2)
                            {
                                string[] reposList = File.ReadAllLines(args[1]);
                                foreach (string repo in reposList)
                                {

                                    string folderName = args[3] + "\\" + repo.Substring(repo.LastIndexOf("/") + 1, (repo.LastIndexOf(".") - repo.LastIndexOf("/") - 1)); //args[3] holds the target folder.
                                    string gitCommand = "status";
                                    if (choice == 1)
                                        gitCommand = "clone " + repo + " -b " + branchName + " " + folderName;
                                    else if (choice == 2)
                                        gitCommand = "--git-dir=" + folderName + "\\.git" + " pull origin " + branchName;
                                    Console.WriteLine("The following command is going to start\n" + gitCommand);
                                    startInfo.Arguments = gitCommand;
                                    Process gitProcess = new Process();

                                    gitProcess.StartInfo = startInfo;
                                    gitProcess.Start();

                                    errorString = gitProcess.StandardError.ReadToEnd();
                                    outputString = gitProcess.StandardOutput.ReadToEnd();

                                    gitProcess.WaitForExit();
                                    gitProcess.Close();
                                    
                                    Console.WriteLine(errorString);
                                    Console.WriteLine(outputString);
                                }
                            }
                            else
                            {
                                Console.WriteLine("Entered choice is not valid. No further action.");
                            }
                                    
                        }
                    }
                    else
                    {
                        Console.WriteLine("GIT.exe not found in this folder or GIT not installed.");
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Error in process : " + e.Message);
                }

            }
            else
            {
                Console.WriteLine("No such folder or not GIT installed.");
            }
            Console.WriteLine("Press a key to exit...");
            Console.ReadKey(true);
        }
    }
}
