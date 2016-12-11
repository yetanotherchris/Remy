using Remy.Core.Tasks.Plugins.Powershell;

namespace Remy.Tests.StubsAndMocks.Core.Tasks.Plugins.Powershell
{
	public class PowershellRunnerMock : IPowershellRunner
	{
		public string[] Commands { get; set; }
		public string ActualTempFilename { get; set; }

		public bool RunCommands(string[] commands)
		{
			Commands = commands;
			return true;
		}

		public bool RunFile(string tempFilename)
		{
			ActualTempFilename = tempFilename;
			return true;
		}
	}
}