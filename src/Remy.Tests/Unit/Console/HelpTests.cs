using NUnit.Framework;
using Remy.Console;

namespace Remy.Tests.Unit.Console
{
	[TestFixture]
	public class HelpTests
	{
		[Test]
		public void GetHelpText_should_return_help_with_correct_version_number()
		{
			// given when
			string fileVersionInfo = Help.GetVersion();
			string helpText = Help.GetHelpText();

			// then
			Assert.That(helpText, Is.Not.Null.Or.Empty);
			Assert.That(helpText, Does.Contain("[Usage]"));
			Assert.That(helpText, Does.Contain($"Remy {fileVersionInfo}"));
		}
	}
}
