using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Remy.Core.Tasks.Runners;

namespace Remy.Tests.Integration
{
	public class ScriptCsRunnerTests
	{
		[Test]
		public void should()
		{
			// Arrange
			var runner = new ScriptCsRunner(null);

			// Act
			runner.Run("test);");

			// Assert
		}
	}
}
