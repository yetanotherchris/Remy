namespace Remy.Core.Tasks.Plugins.Powershell
{
	public interface IPowershellRunner
	{
		bool RunCommands(string[] commands);
		bool RunFile(string tempFilename);
	}
}