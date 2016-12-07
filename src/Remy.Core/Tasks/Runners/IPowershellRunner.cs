namespace Remy.Core.Tasks.Runners
{
	public interface IPowershellRunner
	{
		bool RunCommands(string[] commands);
		bool RunFile(string tempFilename);
	}
}