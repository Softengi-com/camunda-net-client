using System;
using System.Collections.Generic;

using RestSharp.Deserializers;

namespace Camunda.Client.Types
{
	public class DeploymentCreateResponse
	{
		[DeserializeAs(Name = "id")]
		public string ID { get; set; }

		[DeserializeAs(Name = "name")]
		public string Name { get; set; }

		[DeserializeAs(Name = "source")]
		public string Source { get; set; }

		[DeserializeAs(Name = "tenantId")]
		public string TenantID { get; set; }

		[DeserializeAs(Name = "deploymentTime")]
		public DateTime DeploymentTime { get; set; }

		[DeserializeAs(Name = "links")]
		public List<DeploymentLink> Links { get; set; }
	}
}