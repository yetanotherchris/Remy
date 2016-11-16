namespace Ainsley.Core.Config.Yaml
{
	public interface IYamlConfigParser
	{
		IConfiguration Parse(string filename);
	}
}