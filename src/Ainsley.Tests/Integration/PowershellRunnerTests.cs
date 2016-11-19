using System.IO;
using System.Text;
using Ainsley.Core.Tasks;
using NUnit.Framework;
using Serilog;
using Serilog.Core;

namespace Ainsley.Tests.Integration
{
    [TestFixture]
    public class PowershellRunnerTests
    {
        private Logger _logger;
        private StringBuilder _logStringBuilder;

        [SetUp]
        public void Setup()
        {
            _logStringBuilder = new StringBuilder();
            var logMessages = new StringWriter(_logStringBuilder);

            _logger = new LoggerConfiguration()
                .WriteTo
                .TextWriter(logMessages)
                .CreateLogger();
        }

        [Test]
        public void should_ignore_empty_command_list()
        {
            // given
            var runner = new PowershellRunner(_logger);

            // when
            bool result = runner.RunCommands(new string[] {});

            // then
            Assert.That(result, Is.True);
        }

        [Test]
        public void should_capture_stdout()
        {
            // given
            var runner = new PowershellRunner(_logger);

            // when
            bool result = runner.RunCommands(new string[]
            {
                "rm ~/sparklingdietcoke -Force -ErrorAction Ignore",
                "mkdir ~/sparklingdietcoke",
                "dir ~/",
                "rm ~/sparklingdietcoke -Force"
            });

            // then
            Assert.That(result, Is.True);
            Assert.That(_logStringBuilder.ToString(), Does.Contain("sparklingdietcoke"));
        }

        [Test]
        public void should_capture_stderr()
        {
            // given
            var runner = new PowershellRunner(_logger);

            // when
            bool result = runner.RunCommands(new string[]
            {
                "mkdir ~/ainsley-sparklingdietcoke",
                "mkdir ~/ainsley-sparklingdietcoke"
            });

            // then
            Assert.That(result, Is.False);
            Assert.That(_logStringBuilder.ToString(), Does.Contain("sparklingdietcoke"));
        }
    }
}