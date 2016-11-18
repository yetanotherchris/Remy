using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ainsley.Console
{
	class Program
	{
		static void Main(string[] args)
		{
            var options = new Options();

            var isValid = CommandLineParser.Parser.Default.ParseArgumentsStrict(args, options);
        }
	}
}
