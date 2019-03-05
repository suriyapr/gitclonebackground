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
            if (Directory.Exists(gitInstalledPath))
            {
                string[] fileNames = Directory.GetFiles(gitInstalledPath);
                if (fileNames.Contains(gitInstalledPath + "\\git.exe"))
                {
                    if (File.Exists(args[1])) //args[1] holds txt file which holds the repos list.
                    {
                        ProcessStartInfo startInfo = new ProcessStartInfo(gitInstalledPath + "\\git.exe")
                        {
                            CreateNoWindow = false,
                            RedirectStandardError = true,
                            RedirectStandardOutput = true,
                            UseShellExecute = false
                        };
                        string[] reposList = File.ReadAllLines(args[1]);
                        foreach (string repo in reposList)
                        {
                            try
                            {
                                string cloneCommand = "clone " + repo + " -b " + args[2] + " " + args[3] + "\\" + repo.Substring(repo.LastIndexOf("/") + 1, (repo.LastIndexOf(".") - repo.LastIndexOf("/") - 1)); //arg[2] -holds the branch name; args[3] holds the target folder.
                                Console.WriteLine("The following command is going to start\n" + cloneCommand);
                                startInfo.Arguments = cloneCommand;
                                Process gitProcess = new Process();

                                gitProcess.StartInfo = startInfo;
                                gitProcess.Start();

                                errorString = gitProcess.StandardError.ReadToEnd();
                                outputString = gitProcess.StandardOutput.ReadToEnd();

                                gitProcess.WaitForExit();
                                gitProcess.Close();

                                Console.WriteLine("Successfully executed the command.");
                            }
                            catch (Exception e)
                            {
                                Console.WriteLine("Error in process : " + e.Message + "\nError string:" + errorString);
                            }
                            finally
                            {
                                Console.WriteLine(outputString);
                            }
                        }
                    }
                }
                else
                {
                    Console.WriteLine("This folder is not a GIT folder.");
                }

            }
            else
            {
                Console.WriteLine("No such folder.");
            }

            
        }
    }
}
