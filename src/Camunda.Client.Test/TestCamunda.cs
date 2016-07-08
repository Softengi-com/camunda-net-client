using System;
using NUnit.Framework;

namespace Camunda.Client.Test
{
	[TestFixture]
    public class TestCamunda
    {
		[Test]
		public void Authentication()
		{
			var cc = new CamundaConnection("http://localhost:8080/camunda/api/");
			cc.Authenticate("demo", "demo");

			var h = cc.GetTaskHistory(Guid.Parse("b7a9c075-429e-11e6-8781-902b34e38592"));
		}
	}
}
