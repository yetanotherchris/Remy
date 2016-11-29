using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Remy.Core.Tasks.Runners;
using Serilog;

namespace Remy.Tests.Integration
{
	public class ScriptCsRunnerTests
	{
		private ILogger _logger;
		private StringBuilder _logStringBuilder;

		[SetUp]
		public void Setup()
		{
			_logStringBuilder = new StringBuilder();
			var logMessages = new StringWriter(_logStringBuilder);

			_logger = new LoggerConfiguration()
				.WriteTo
				.TextWriter(logMessages)
				.WriteTo
				.LiterateConsole()
				.CreateLogger();
		}

		[Test]
		public void should()
		{
			// Arrange
			var runner = new ScriptCsRunner(_logger);

			// Act
			runner.Run("test);");

			// Assert
		}
	}
}
