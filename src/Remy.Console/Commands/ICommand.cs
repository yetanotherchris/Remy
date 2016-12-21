using Remy.Core.Tasks;
using Serilog;

namespace Remy.Console.Commands
{
	public interface ICommand
	{
		bool VerboseLogging { get; set; }
		ILogger Logger { get; set; }
		void Run(IServiceLocator serviceLocator);
	}
}