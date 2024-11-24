
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

    
    public void Start()
    {
        if (watcher != null) return;
        var path = Directory.GetCurrentDirectory();
        path = Path.Join(path, "..");
        Console.WriteLine($"watching {path}");
        watcher = new FileSystemWatcher(path);

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

        if (e.FullPath.Contains("/.git/"))
        {
            Console.WriteLine($"Ignoring {e.FullPath}");
            return;
        }

        if (e.FullPath.Contains("/bin/"))
        {
            Console.WriteLine($"Ignoring {e.FullPath}");
            return;
        }

        if (e.FullPath.Contains("/obj/"))
        {
            Console.WriteLine($"Ignoring {e.FullPath}");
            return;
        }
        Console.WriteLine(e.FullPath);
        Console.WriteLine($"changes detected as of time: {DateTime.Now:HH-mm-ss:fff}");

        var output_git_add = git_add();
        if (string.IsNullOrEmpty(output_git_add) == false )
        {
            
            // the idea here is if you are switching branches then there is nothing to add
            // and if there is something to add then you need a new branch
            // interesting I thought that on every change a new branch will be creates
            // hmm looks like it will create a new branch after all, it might be a feature but also 
            // this means that there will be a way more branches than I wanted
            if (current_branch.Contains("main") || current_branch.Contains("prod"))
            {
                // only allow branching from "main" branch
                git_create_new_branch();
            }
            
        }
        else
        {
            Console.WriteLine("There is nothing to add, therefore no need to create a branch");
        }
        git_commit();
    }

    private static string git_current_branch()
    {
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "git",
            Arguments = "branch --show-current",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true
        };

        using (Process process = Process.Start(startInfo))
        {
            using (StreamReader reader = process.StandardOutput)
            {
                string result = reader.ReadToEnd();
                return result;
            }
        }
    }

    private static string git_create_new_branch()
    {
        DateTime now = DateTime.Now;
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "git",
            Arguments = $"checkout -b {now:yyyy-MM-dd--HH}h{now:mm}m",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            //WorkingDirectory = 
        };

        using (Process process = Process.Start(startInfo))
        {
            using (StreamReader reader = process.StandardOutput)
            {
                string result = reader.ReadToEnd();
                Console.WriteLine(result);
                return result; 
            }
        }
    }

    private static string git_add()
    {
        DateTime now = DateTime.Now;
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "git",
            Arguments = $"add . --verbose",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            //WorkingDirectory = 
        };

        using (Process process = Process.Start(startInfo))
        {
            using (StreamReader reader = process.StandardOutput)
            {
                string result = reader.ReadToEnd();
                Console.WriteLine(result);
                return result;
            }
        }
    }

    private static string git_commit()
    {
        DateTime now = DateTime.Now;
        ProcessStartInfo startInfo = new ProcessStartInfo
        {
            FileName = "git",
            Arguments = $"commit -m \"{now:yyyy-MM-dd--HH}h{now:mm}m{now:ss}s\"",
            RedirectStandardOutput = true,
            UseShellExecute = false,
            CreateNoWindow = true,
            //WorkingDirectory = 
        };

        using (Process process = Process.Start(startInfo))
        {
            using (StreamReader reader = process.StandardOutput)
            {
                string result = reader.ReadToEnd();
                Console.WriteLine(result);
                return result;
            }
        }
    }








}
