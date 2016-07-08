using System;

using CommandLine;

namespace CamundaCLI
{
	static internal class Program
	{
		static private void Main(string[] args)
		{
			var parser = new Parser(config => config.HelpWriter = Console.Out);
			parser.ParseArguments<DeployOptions>(args)
				.WithParsed(Deploy);
		}

		static private void Deploy(DeployOptions options)
		{
			var cn = new Camunda.Client.CamundaConnection(options.CamundaUri);
			cn.Authenticate(options.User, options.Password);

			foreach (var fileName in options.Files)
			{
				// TODO:
			}
		}
	}
}