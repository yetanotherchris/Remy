using System;

namespace Remy.Core.Config
{
	public interface IConfigFileReader
	{
		string Read(Uri uri);
	}
}