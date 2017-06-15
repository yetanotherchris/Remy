using System;
using System.IO;
using System.Net;
using Serilog;

namespace Remy.Core.Config
{
    public class ConfigFileReader : IConfigFileReader
    {
        private readonly ILogger _logger;

        public ConfigFileReader(ILogger logger)
        {
            _logger = logger;
            _logger.Warning("test");
        }

        public string Read(Uri uri)
        {
            if (uri.IsFile)
            {
                string yaml = ReadLocalFile(uri);
                yaml = ReplaceAllEnvironmentalVariables(yaml);

                return yaml;
            }
            else if (uri.Scheme.StartsWith("http"))
            {
                string yaml = ReadRemoteFile(uri);
                yaml = ReplaceAllEnvironmentalVariables(yaml);

                return yaml;
            }

            throw new RemyException($"The scheme {uri.Scheme} for {uri} is not supported");
        }

        private string ReadLocalFile(Uri uri)
        {
            try
            {
                return File.ReadAllText(uri.LocalPath);
            }
            catch (IOException e)
            {
                throw new RemyException($"Unable to download open file '{uri}' - {e.Message}", e);
            }
        }

        private string ReadRemoteFile(Uri uri)
        {
            try
            {
                return new WebClient().DownloadString(uri);
            }
            catch (WebException e)
            {
                throw new RemyException($"Unable to download '{uri}' - {e.Message}", e);
            }
        }

        private string ReplaceAllEnvironmentalVariables(string yaml)
        {
            yaml = ReplaceEnvironmentalVariables(yaml, EnvironmentVariableTarget.User);
            yaml = ReplaceEnvironmentalVariables(yaml, EnvironmentVariableTarget.Process);
            yaml = ReplaceEnvironmentalVariables(yaml, EnvironmentVariableTarget.Machine);

            return yaml;
        }

        private string ReplaceEnvironmentalVariables(string yaml, EnvironmentVariableTarget target)
        {
            var envVars = Environment.GetEnvironmentVariables(target);
            _logger.Information($"Found {envVars.Count} '{target}' environment variables");
            foreach (string key in envVars.Keys)
            {
                string keyToken = "{{" + key + "}}";

                if (yaml.Contains(keyToken))
                {
                    string value = envVars[key].ToString();
                    string displayValue = value;
                    if (displayValue.Length > 10)
                    {
                        displayValue = displayValue.Substring(0, 10) + "...";
                    }

                    _logger.Information($"Found env variable '{keyToken}' in YAML and replacing it with '{displayValue}'");
                    yaml = yaml.Replace("{{" + key + "}}", envVars[key].ToString());
                }
            }

            return yaml;
        }

        public static string GetFullPathForFile(string filename)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), filename);
        }
    }
}