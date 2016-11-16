namespace Ainsley.Core.Yaml
{
	public interface IYamlConfigParser
	{
		IConfiguration Parse(string filename);
	}
}