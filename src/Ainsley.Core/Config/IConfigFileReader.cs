using System;

namespace Ainsley.Core.Config
{
	public interface IConfigFileReader
	{
		string Read(Uri uri);
	}
}