using System.Collections.Generic;
using Ainsley.Core.Tasks;

namespace Ainsley.Core.Config.Yaml
{
	public interface IYamlConfigParser
	{
        List<ITask> Parse(string filename);
	}
}