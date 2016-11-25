using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Remy.Core.Tasks;

namespace Remy.Tests.Integration
{
	[TestFixture]
	public class PluginInstallerTests
	{
		[Test]
		public void should()
		{
			// given
			var pluginInstaller = new PluginInstaller();

			// when
			pluginInstaller.Run();

			// then
		}
	}
}
