using System.Collections.Generic;
using RestSharp.Deserializers;

namespace Camunda.Client.Types
{
	public class AuthenticationResponse
	{
		[DeserializeAs(Name = "userId")]
		public string UserId { get; set; }

		[DeserializeAs(Name = "authorizedApps")]
		public List<string> AuthorizedApps { get; set; }
	}
}