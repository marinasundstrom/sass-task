using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Build.Framework;
using Sass;
using MSBuildTask = Microsoft.Build.Utilities.Task;

namespace SassTask
{

    public class SassCompile : MSBuildTask
    {
        private const string SASS_EXECUTABLE_FILENAME = "sass";
        private const string SASS_CONFIG_FILENAME = "sassconfig.json";

        public override bool Execute()
        {
            var task = Task.Run(async () =>
            {
                SassConfig config = new SassConfig();

                var configFilePath = ConfigPath ?? SASS_CONFIG_FILENAME;
                if (File.Exists(configFilePath))
                {
                    using (Stream stream = File.OpenRead(configFilePath))
                    {
                        await config.LoadAsync(stream);
                    }
                }

                SassCommandArgumentBuilder commandArgumentBuilder = new SassCommandArgumentBuilder(config, Environment.CurrentDirectory);
                var commandArguments = commandArgumentBuilder.BuildArgs();

                Log.LogMessage(commandArguments);

                ExecuteSassCommand(commandArguments);

                return true;
            });
            return task.Result;
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
                    sassProcess.Start();
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
