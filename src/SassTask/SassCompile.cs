using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Sass;
using MSBuildTask = Microsoft.Build.Utilities.Task;

namespace SassTask
{

    public sealed class SassCompile : MSBuildTask
    {
        private const string SASS_EXECUTABLE_FILENAME = "sass";
        private const string SASS_CONFIG_FILENAME = "sassconfig.json";

        public override bool Execute()
        {
            var task = Task.Run(async () =>
            {
                try
                {
                    SassConfig config = null;

                    var configFilePath = ConfigPath ?? SASS_CONFIG_FILENAME;


                    if (File.Exists(configFilePath))
                    {
                        var sassConfigLoader = new SassConfigLoader();

                        using (Stream stream = File.OpenRead(configFilePath))
                        {
                            config = await sassConfigLoader.LoadAsync(stream);
                        }
                    }

                    SassCommandArgumentBuilder commandArgumentBuilder = new SassCommandArgumentBuilder(config, Environment.CurrentDirectory);
                    var commandArguments = commandArgumentBuilder.BuildArgs();

                    Log.LogMessage(commandArguments);

                    ExecuteSassCommand(commandArguments);

                    if (!string.IsNullOrEmpty(config?.CompilerOptions.OutFile))
                    {
                        await MergeFilesAsync(config);
                    }

                    return true;
                }
                catch(Exception e) 
                {
                    Console.WriteLine(e);
                    return false;
                }
            });
            return task.GetAwaiter().GetResult();
        }

        private static async Task MergeFilesAsync(SassConfig config)
        {
            var outDirPath = Environment.CurrentDirectory;
            if (config.CompilerOptions?.OutDir != null)
            {
                outDirPath = PathHelpers.ToRootedPath(config.CompilerOptions.OutDir, Environment.CurrentDirectory);
            }

            var outFilePath = Path.Combine(outDirPath, config?.CompilerOptions.OutFile);

            var filePaths = Directory
                .GetFiles(outDirPath, "*.css", SearchOption.AllDirectories)
                .Where(p => !p.Contains(outFilePath));

            var cssFileMerger = new CssFileMerger();
            await cssFileMerger.MergeFilesAsync(
                filePaths.ToArray(), 
                outFilePath, 
                compress: config.CompilerOptions.Style == CssStyle.Compressed, 
                true);
        }

        private void ExecuteSassCommand(string commandArguments)
        {
            try
            {
                using (Process sassProcess = new Process())
                {
                    sassProcess.StartInfo.UseShellExecute = false;
                    sassProcess.StartInfo.FileName = SASS_EXECUTABLE_FILENAME;
                    sassProcess.StartInfo.Arguments = commandArguments;
                    sassProcess.StartInfo.CreateNoWindow = true;
                    sassProcess.EnableRaisingEvents = true;
                    sassProcess.Start();
                    sassProcess.WaitForExit(10*1000); //10 seconds
                }
            }
            catch (Exception e)
            {
                Log.LogError(e.Message);
            }
        }

        public string ConfigPath { get; set; }
    }
}
