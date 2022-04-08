using CommandLine;
using CommandLine.Text;
using System;
using System.Threading.Tasks;

namespace ApiCheck.Console
{
  internal class Program
  {
    static async Task<int> Main(string[] args)
    {

      var parser = new Parser(with => with.HelpWriter = null);
      var parserResult = parser.ParseArguments<Options>(args);

      parserResult.WithNotParsed(errs => DisplayHelp(parserResult));
      return await parser.ParseArguments<Options>(args).MapResult(async (o) => await Run(o), errs => Task.FromResult(-1));

    }

    static void DisplayHelp<T>(ParserResult<T> result)
    {
      var helpText = HelpText.AutoBuild(result, h =>
      {
        h.AdditionalNewLineAfterOption = true;
        h.AddDashesToOption = true;
        h.AddPreOptionsLine("Usage: ApiCheck.Console.exe -r <reference assembly> -n <new assembly> [-x <xml report>] [-h <html report>] [-c <config file>] [-v]");
        h.Heading = "Myapp 2.0.0-beta"; //change header
        h.Copyright = "Copyright (c) 2019 Global.com"; //change copyright text
        return HelpText.DefaultParsingErrorsHandler(result, h);
      }, e => e);
      System.Console.WriteLine(helpText);
    }
    private static Task<int> Run(Options options)
    {
      int returnValue = -1;
      try
      {
        returnValue = new Check(options.ReferencePath, options.NewPath, options.HtmlPath, options.XmlPath, options.ConfigPath, options.Verbose).CheckAssemblies();
      }
      catch (Exception exception)
      {
        System.Console.WriteLine(exception.Message);
      }
      return Task.FromResult(returnValue);
    }
  }
}
