using System;
using System.IO;
using System.Linq;

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
			if (!string.IsNullOrEmpty(options.TenantID))
				cn.TentantID = options.TenantID;
			cn.Authenticate(options.User, options.Password);

			cn.GetTask(Guid.Parse("dde93ad5-4518-11e6-a4a0-00155dc89912"));

			var allFiles = options
				.Files
				.SelectMany(f => Directory.Exists(f) ? Directory.EnumerateFiles(f) : new[] {f})
				.ToArray();

			var result = cn.Deploy(options.Name, options.Source, allFiles);
			Console.WriteLine($@"
Deployed:
  ID             : {result.ID}
  Name           : {result.Name}
  Source         : {result.Source}
  Deployment time: {result.DeploymentTime}
  Tentant ID     : {result.TenantID}
");
			if (result.Links != null)
			{
				Console.WriteLine("Links:");
				foreach (var link in result.Links)
					Console.WriteLine($"  Method: {link.Method}; Rel: {link.Rel}; Href: {link.Href};");
			}
			else
				Console.WriteLine("Links: none.");
		}
	}
}