using RestSharp.Deserializers;

namespace Camunda.Client.Types
{
	public class CamundaUserProfile
	{
		[DeserializeAs(Name = "id")]
		public string Id { get; set; }

		[DeserializeAs(Name = "firstName")]
		public string FirstName { get; set; }

		[DeserializeAs(Name = "lastName")]
		public string LastName { get; set; }

		[DeserializeAs(Name = "email")]
		public string Email { get; set; }
	}
}