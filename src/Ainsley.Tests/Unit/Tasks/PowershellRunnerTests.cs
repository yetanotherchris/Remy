using Ainsley.Core.Tasks;
using NUnit.Framework;

namespace Ainsley.Tests.Unit.Tasks
{
    [TestFixture]
	public class PowershellRunnerTests
    {
        [Test]
        public void should_capture_stderr_and_stdout()
        {
            // given
            var runner = new PowershellRunner();

            // when
            string output = runner.RunCommand("dir ~/");

            // then
            System.Console.WriteLine(output);
        }
    }
}