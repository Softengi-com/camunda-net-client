using RestSharp.Deserializers;

namespace Camunda.Client.Types
{
	public class CamundaEngine
	{
		[DeserializeAs(Name = "name")]
		public string Name { get; set; }
	}
}