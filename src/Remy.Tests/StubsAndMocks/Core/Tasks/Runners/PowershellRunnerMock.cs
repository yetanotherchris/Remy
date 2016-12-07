using Remy.Core.Tasks.Runners;

namespace Remy.Tests.StubsAndMocks.Core.Tasks.Runners
{
	public class PowershellRunnerMock : IPowershellRunner
	{
		public string[] Commands { get; set; }
		public string TempFilename { get; set; }

		public bool RunCommands(string[] commands)
		{
			Commands = commands;
			return true;
		}

		public bool RunFile(string tempFilename)
		{
			TempFilename = tempFilename;
			return false;
		}
	}
}