/* MIT License
Copyright(c) 2016 RedBlueGames
Code written by Doug Cox
*/
 
using System;
using System.Diagnostics;
using System.IO;
using UnityEngine;
 
namespace Build
{

    /// <summary>
    /// GitException includes the error output from a Git.Run() command as well as the
    /// ExitCode it returned.
    /// </summary>
    public class GitException : InvalidOperationException
    {
        public GitException(int exitCode, string errors) : base(errors) =>
            this.ExitCode = exitCode;

        /// <summary>
        /// The exit code returned when running the Git command.
        /// </summary>
        public readonly int ExitCode;
    }

    public static class Git
    {
        /* Properties ============================================================================================================= */

        /// <summary>
        /// Retrieves the build version from git based on the most recent matching tag and
        /// commit history. This returns the version as: {major.minor.build} where 'build'
        /// represents the nth commit after the tagged commit.
        /// Note: The initial 'v' and the commit hash code are removed.
        /// </summary>
        public static string BuildVersion {
            get {
                var version = Run(@"describe --tags --long --match ""v[0-9]*""");
                // Remove initial 'v' and ending git commit hash.
                version = version.Replace('-', '.');
                version = version.Substring(1, version.LastIndexOf('.') - 1);
                return version;
            }
        }

        /// <summary>
        /// The hash of current commit.
        /// </summary>
        public static string Hash => Run(@"rev-parse HEAD").Trim();

        /// <summary>
        /// The currently active branch.
        /// </summary>
        public static string Branch => Run(@"rev-parse --abbrev-ref HEAD").Trim();

        /// <summary>
        /// Returns a listing of all uncommitted or untracked (added) files.
        /// </summary>
        public static string Status => Run(@"status --porcelain");


        /* Methods ================================================================================================================ */

        /// <summary>
        /// Runs git.exe with the specified arguments and returns the output.
        /// </summary>
        public static string Run(string arguments)
        {
            using (var process = new System.Diagnostics.Process()) {
                ProcessStartInfo pInfo = new ProcessStartInfo();
                pInfo.FileName = @"git.exe";
                pInfo.Arguments = arguments;
                pInfo.WorkingDirectory = Application.dataPath;
                pInfo.RedirectStandardOutput = true;
                pInfo.RedirectStandardError = true;
                pInfo.UseShellExecute = false;

                process.StartInfo = pInfo;
                process.Start();
                process.WaitForExit();

                int exitCode = process.ExitCode;

                StreamReader reader;
                if (exitCode == 0) {
                    reader = process.StandardOutput;
                    string output = reader.ReadToEnd();
                    return output;
                } else {
                    reader = process.StandardError;
                    string errors = reader.ReadToEnd();

                    throw new GitException(exitCode, errors);
                }
            }
        }
    }
}