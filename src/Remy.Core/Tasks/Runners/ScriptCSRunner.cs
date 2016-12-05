using System;
using System.Linq;
using ScriptCs;
using ScriptCs.Contracts;
using ScriptCs.Hosting;
using Serilog;
using ScriptCs.Engine.Roslyn;

namespace Remy.Core.Tasks.Runners
{
	// Move this to a plugin

	public class LogP : ILogProvider
	{
		private readonly ILogger _logger;

		public LogP(ILogger logger)
		{
			_logger = logger;
		}

		public Logger GetLogger(string name)
		{
			return (level, gettext, exception, args) =>
			{
				_logger.Information(gettext());
				return true;
			};
			throw new NotImplementedException();
		}

		public IDisposable OpenNestedContext(string message)
		{
			throw new NotImplementedException();
		}

		public IDisposable OpenMappedContext(string key, string value)
		{
			throw new NotImplementedException();
		}
	}

	public class ScriptCsRunner
	{
        private readonly ILogger _logger;

        public ScriptCsRunner(ILogger logger)
        {
            _logger = logger;
        }

		public bool Run(string code)
		{
			var console = (IConsole)new ScriptConsole();
			var logProvider = new ColoredConsoleLogProvider(LogLevel.Info, console);

			var builder = new ScriptServicesBuilder(console, logProvider);
			builder.ScriptEngine<CSharpScriptEngine>();

			var services = builder.Build();

			var executor = (ScriptExecutor)services.Executor;
			executor.Initialize(Enumerable.Empty<string>(), Enumerable.Empty<IScriptPack>());
			Execute(executor);

			return true;
		}

		public void Execute(ScriptExecutor executor)
		{
			//executor.Execute("HelloWorld.csx");
			var script = @"Console.WriteLine(""Hello from scriptcs"")";
			var result = executor.ExecuteScript(script);
		}
	}
}