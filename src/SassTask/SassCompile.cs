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

                    if (configFilePath != null)
                    {
                        configFilePath = Path.GetFullPath(configFilePath);

                        if (File.Exists(configFilePath))
                        {
                            var sassConfigLoader = new SassConfigLoader();

                            using (Stream stream = File.OpenRead(configFilePath))
                            {
                                config = await sassConfigLoader.LoadAsync(stream);
                            }
                        } 
                        else 
                        {
                            Log.LogError($"Config file \"{configFilePath}\" does not exist.");
                            return false;
                        }
                    }
                    else 
                    {
                        // Default config
                        config = new SassConfig();
                    }

                    SassCommandArgumentBuilder commandArgumentBuilder = new SassCommandArgumentBuilder(config, Environment.CurrentDirectory);
                    var commandArguments = commandArgumentBuilder.BuildArgs();

                    Log.LogMessage(commandArguments);

                    ExecuteSassCommand(commandArguments);

                    Log.LogMessage($"Sass files were successfully compiled.");

                    return true;
                }
                catch(Exception e) 
                {
                    Log.LogErrorFromException(e);
                    return false;
                }
            });
            return task.GetAwaiter().GetResult();
        }

        private void ExecuteSassCommand(string commandArguments)
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

        public string ConfigPath { get; set; }
    }
}
