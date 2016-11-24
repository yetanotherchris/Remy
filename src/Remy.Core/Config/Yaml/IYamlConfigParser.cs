using System;
using System.Collections.Generic;
using Remy.Core.Tasks;

namespace Remy.Core.Config.Yaml
{
	public interface IYamlConfigParser
	{
        List<ITask> Parse(Uri uri);
	}
}