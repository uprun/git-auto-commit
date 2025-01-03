﻿
using System;
using System.Collections.Generic;
using System.Linq;

using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;


Console.WriteLine("Hello git-auto-commit");
var some  = new Bundle_Watcher();

some.Start();
int ok = 0;
while(true)
{

    await Task.Delay(TimeSpan.FromSeconds(0.1));
    if (ok % 20 == 0)
    {
        Console.WriteLine($"Hello git-auto-commit {DateTime.Now:HH:mm:ss}");
    }
    else
    {
        Console.Write(".");
    }
    ok ++;
    
}

public class Bundle_Watcher
{
    
    FileSystemWatcher? watcher = null;

    private string? _initial_path = null;
    public void Start()
    {
        if (watcher != null) return;
        _initial_path = Directory.GetCurrentDirectory();
        _initial_path = Path.Join(_initial_path, "..");
        _initial_path = new DirectoryInfo(_initial_path).FullName;
        Console.WriteLine($"watching {_initial_path}");
        watcher = new FileSystemWatcher(_initial_path);

        watcher.NotifyFilter = NotifyFilters.Attributes
                                | NotifyFilters.CreationTime
                                | NotifyFilters.DirectoryName
                                | NotifyFilters.FileName
                                | NotifyFilters.LastAccess
                                | NotifyFilters.LastWrite
                                | NotifyFilters.Security
                                | NotifyFilters.Size;

        watcher.Changed += OnChanged;
        watcher.Created += OnChanged;
        watcher.Deleted += OnChanged;
        watcher.Renamed += OnChanged;
        

        watcher.Filter = "*.*";
        watcher.IncludeSubdirectories = true;
        watcher.EnableRaisingEvents = true;

    } 

    private void OnChanged(object sender, FileSystemEventArgs e)
    {
        string fullPath = e.FullPath;
        var almost_project_directory = fullPath.Substring(_initial_path!.Length);
        //Console.WriteLine("Something similar to project directory: " + almost_project_directory);
        var splitted = almost_project_directory.Split(new [] {Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar}, StringSplitOptions.RemoveEmptyEntries);
        var project_directory = splitted.First();
        //Console.WriteLine($"Project is: {project_directory}" );
        var full_project_directory_path = Path.Combine(_initial_path, project_directory);
        if (fullPath.Contains("/.git/"))
        {
            //Console.WriteLine($"Ignoring {fullPath}");
            return;
        }
        // no need to filter out bin and obj they should not be added because of .gitignore config
        Console.WriteLine(fullPath);

        Console.WriteLine($"changes detected as of time: {DateTime.Now:HH-mm-ss:fff}");

        var (current_branch, current_branch_error) = git_current_branch(full_project_directory_path);

        if (current_branch_error.Contains(" not a git repository "))
        {
            Console.WriteLine("!!!!!Not a git repository");
            return;
        }

        if (current_branch.Contains(" not a git repository "))
        {
            Console.WriteLine("Not a git repository");
            return;
        }

        Console.WriteLine($"Current branch name is : {current_branch}");

        var output_git_add = run_process(full_project_directory_path, "git", $"add . --verbose");
        Console.WriteLine(output_git_add.output);

        DateTime now = DateTime.Now;
        if (string.IsNullOrEmpty(output_git_add.output))
        {
            Console.WriteLine("There is nothing to add, therefore no need to create a branch");
        }
        else
        {
            // the idea here is if you are switching branches then there is nothing to add
            // and if there is something to add then you need a new branch
            // interesting I thought that on every change a new branch will be creates
            // hmm looks like it will create a new branch after all, it might be a feature but also 
            // this means that there will be a way more branches than I wanted
            if (current_branch.Contains("main") || current_branch.Contains("prod"))
            {
                Console.WriteLine("Will create a new feature branch of \"main\" branch");
                // only allow branching from "main" branch
                
                var branch_name = $"{now:yyyy-MM-dd--HH}h{now:mm}m";
                var (checkout_output, checkout_error) = run_process(full_project_directory_path, "git", $"checkout -b {branch_name}");
                if (String.IsNullOrEmpty(checkout_error))
                {
                    current_branch = branch_name;
                }
            }
            else
            {
                Console.WriteLine("Changes are present but assumption is that we are already in the feature branch, so suspending creation of a new branch");
            }
            //$"commit -m \"{now:yyyy-MM-dd--HH}h{now:mm}m{now:ss}s\""
            // there will be no auto-commit anymore
            // only auto-add and branch creation
            //run_process(full_project_directory_path, "git", $"commit -m \"{now:yyyy-MM-dd--HH}h{now:mm}m{now:ss}s\"");
            //

        }
        
    }

    private static (string output, string error) git_current_branch(string workingDirectory)
    {
        string program = "git";
        string arguments = "branch --show-current";
        return run_process(workingDirectory, program, arguments);
    }

    private static (string output, string error) run_process(string workingDirectory, string program, string arguments)
    {
        Console.WriteLine($"running: {program} {arguments}");
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = program,
            Arguments = arguments,  
            UseShellExecute = false,
            CreateNoWindow = true,
            WorkingDirectory = workingDirectory,
            RedirectStandardError = true, //
            RedirectStandardOutput = true
            // no user name after all
        };

        var error = "";
        var output = "";

        using (Process process = Process.Start(startInfo))
        {
            using (StreamReader reader = process.StandardError)
            {
                error = reader.ReadToEnd();
            }

            using (StreamReader reader = process.StandardOutput)
            {
                output = reader.ReadToEnd();
            }
        }
        return (output, error);
    }

}
