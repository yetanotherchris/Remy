using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using Nancy;
using NUnit.Framework;
using Remy.Core;
using Remy.Core.Config;
using Serilog;
using Serilog.Core;

namespace Remy.Tests.Integration.Core.Config
{
    [TestFixture]
    public class ConfigFileReaderTests
    {
        private Logger _logger;
        private ConfigFileReader _configFileReader;

        [SetUp]
        public void Setup()
        {
            _logger = new LoggerConfiguration()
                .WriteTo
                .LiterateConsole()
                .CreateLogger();

            _configFileReader = new ConfigFileReader(_logger);
        }

        [Test]
        public void should_read_file_from_disk()
        {
            // Arrange
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "test-config.yml");

            // Act
            string actualContent = _configFileReader.Read(new Uri(configPath));

            // Assert
            Assert.That(actualContent, Does.Contain("name: \"Text example\""));
        }

        [Test]
        public void should_read_file_from_url()
        {
            // Arrange
            string url = GetLocalhostAddress();

            using (var task = RunBasicHttpServer(url, "- some yaml\n- more yaml"))
            {
                // Act
                string actualContent = _configFileReader.Read(new Uri(url));

                // Assert
                Assert.That(actualContent, Does.Contain("- some yaml\n- more yaml"));
            }
        }

        [Test]
        public void should_read_file_from_url_and_replace_env_vars()
        {
            // Arrange
            string url = GetLocalhostAddress();

            Environment.SetEnvironmentVariable("key1", "myvalue1", EnvironmentVariableTarget.Machine);
            Environment.SetEnvironmentVariable("key2", "myvalue2", EnvironmentVariableTarget.Process);
            Environment.SetEnvironmentVariable("key3", "myvalue3", EnvironmentVariableTarget.User);
            Environment.SetEnvironmentVariable("key1", "user values trump machine values", EnvironmentVariableTarget.User);

            using (var task = RunBasicHttpServer(url, "- some yaml {{key1}} {{key2}} {{key3}}"))
            {
                // Arrange

                // Act
                string actualContent = _configFileReader.Read(new Uri(url));

                // Assert
                Assert.That(actualContent, Does.Contain("myvalue2"));
                Assert.That(actualContent, Does.Contain("myvalue3"));
                Assert.That(actualContent, Does.Contain("user values trump machine values"));
            }
        }

        private string GetLocalhostAddress()
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();

            return $"http://localhost:{port}/";
        }

        public static Task RunBasicHttpServer(string url, string outputHtml)
        {
            return Task.Run(() =>
            {
                HttpListener listener = new HttpListener();
                listener.Prefixes.Add(url);
                listener.Start();

                // GetContext method blocks while waiting for a request.
                HttpListenerContext context = listener.GetContext();
                HttpListenerResponse response = context.Response;

                Stream stream = response.OutputStream;
                var writer = new StreamWriter(stream);
                writer.Write(outputHtml);
                writer.Close();
            });
        }

        [Test]
        public void GetFullPathForFile_should_return_full_path_for_filename()
        {
            // Arrange
            string expectedPath = Path.Combine(Directory.GetCurrentDirectory(), "myfile.txt");

            // Act
            string actualPath = ConfigFileReader.GetFullPathForFile("myfile.txt");

            // Assert
            Assert.That(actualPath, Is.EqualTo(expectedPath));
        }

        [Test]
        public void should_throw_remyexception_for_bad_filepath()
        {
            // Arrange
            string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "doesnexist");

            // Act + Assert
            Assert.Throws<RemyException>(() => _configFileReader.Read(new Uri(configPath)));
        }

        [Test]
        public void should_throw_remyexception_for_invalid_uri()
        {
            // Arrange

            // Act + Assert
            Assert.Throws<RemyException>(() => _configFileReader.Read(new Uri("http://bleh")));
        }

        [Test]
        public void should_throw_remyexception_for_unsupported_scheme()
        {
            // Arrange

            // Act + Assert
            Assert.Throws<RemyException>(() => _configFileReader.Read(new Uri("ftp://warezplace")));
        }
    }
}