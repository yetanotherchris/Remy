using CommandLine;

namespace Ainsley.Console
{
	class Program
	{
		static void Main(string[] args)
		{
            ParserResult<Options> result = CommandLine.Parser.Default.ParseArguments<Options>(args);
        }
	}
}
