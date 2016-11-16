namespace Ainsley.Core.Config
{
	public interface IConfigFileReader
	{
		string Read(string filename);
	}
}