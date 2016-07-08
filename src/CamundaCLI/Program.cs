using System;
using System.Collections.Generic;

using CommandLine;
using CommandLine.Text;

namespace CamundaCLI
{
	public class Options
	{
		public DeployOptions DeployVerb { get; set; } = new DeployOptions();
	}

	[Verb("deploy", HelpText = "Deploy files.")]
	public class DeployOptions
	{
		[Option('f', "files", Required = true, HelpText = "Files to deploy.")]
		public IEnumerable<string> Files { get; set; }

		[Option('c', "camundaUri", Required = true, HelpText = "Camunda URI.")]
		public string CamundaUri { get; set; }

		[Option('u', "user", Required = true, HelpText = "User name.")]
		public string User { get; set; }

		[Option('p', "password", Required = true, HelpText = "User password.")]
		public string Password { get; set; }

		[Option(Default = true, HelpText = "Prints all messages to standard output.")]
		public bool Verbose { get; set; }
	}

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