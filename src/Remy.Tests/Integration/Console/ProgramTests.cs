using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NUnit.Framework;
using Remy.Console;

namespace Remy.Tests.Integration.Console
{
	// Tests with actual Nuget downloads

	[TestFixture]
	public class ProgramTests
	{
		[Test]
		public void should_install_plugin_to_disk()
		{
			// Arrange
			string pluginsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "plugins");

			if (Directory.Exists(pluginsPath))
				Directory.Delete(pluginsPath, true);

			string[] args = { "plugins", "install", "NUnit" };

			// Act
			Program.Main(args);

			// Assert
			IEnumerable<string> files = Directory.EnumerateFiles(pluginsPath, "*.dll", SearchOption.AllDirectories);
			Assert.That(files.Count(), Is.GreaterThan(5));

			bool containsDll = false;
			foreach (string file in files)
			{
				if (file.EndsWith("nunit.framework.dll"))
				{
					containsDll = true;
				}
			}

			Assert.True(containsDll);
		}
	}
}