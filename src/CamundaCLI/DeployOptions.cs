using System.Collections.Generic;

using CommandLine;

namespace CamundaCLI
{
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
}