using RestSharp.Deserializers;

namespace Camunda.Client.Types
{
	public class DeploymentLink
	{
		[DeserializeAs(Name = "method")]
		public string Method { get; set; }

		[DeserializeAs(Name = "href")]
		public string Href { get; set; }

		[DeserializeAs(Name = "rel")]
		public string Rel { get; set; }
	}
}