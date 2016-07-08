using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Camunda.Client.Types;
using RestSharp;

namespace Camunda.Client
{
	public class CamundaConnection
	{
		private readonly RestClient _client;
		private string _engineName;
		private string _sessionID;

		public CamundaConnection(string baseUri, string engineName = null)
		{
			_client = new RestClient(baseUri);
			_engineName = engineName;
		}

		public CamundaUserProfile Authenticate(string userName, string password)
		{
			var request = new RestRequest("/admin/auth/user/default/login/tasklist");
			request.AddParameter("username", userName);
			request.AddParameter("password", password);

			var authResponse = _client.Post<AuthenticationResponse>(request);
			CheckStatusOk("Authentication", authResponse);

			var cookie = authResponse.Cookies.FirstOrDefault(c => c.Name == "JSESSIONID");
			if (cookie == null)
				throw new ApplicationException("Expected 'JSESSIONID' cookie");
			_sessionID = cookie.Value;

			SetEngineName();
			return GetUserProfile().Data;
		}

		public void GetTask(Guid taskID)
		{
			var request = new RestRequest($"/engine/engine/{_engineName}/task/{taskID}");
			request.AddCookie("JSESSIONID", _sessionID);
			var resp = _client.Get(request);
			CheckStatusOk("Get task history", resp);
		}

		public CamundaTaskHistory GetTaskHistory(Guid taskID)
		{
			var request = new RestRequest($"/engine/engine/{_engineName}/history/task?taskId={taskID}");
			request.AddCookie("JSESSIONID", _sessionID);

			var resp = _client.Get<List<CamundaTaskHistory>>(request);
			CheckStatusOk("Get task history", resp);

			var data = resp.Data;
			if (data.Count > 1)
				throw new ApplicationException("Expected only one task history instance");

			return data.Count == 0 ? null : data[0];
		}

		private void SetEngineName()
		{
			var response = _client.Get<List<CamundaEngine>>(new RestRequest("/engine/engine/"));
			CheckStatusOk("Engines list", response);

			if (string.IsNullOrEmpty(_engineName))
			{
				// Get the "default" or the first engine
				var engine = response.Data.FirstOrDefault(e => e.Name == "default") ?? response.Data[0];
				_engineName = engine.Name;
			}
			else
			{
				// Check that the specific engine exist
				var engine = response.Data.FirstOrDefault(e => e.Name == _engineName);
				if (engine == null)
					throw new ApplicationException("Engine '{0}' does not exist at Camunda server.");
			}

			var engines = response.Data;
			if (engines.Count == 0)
				throw new ApplicationException("Expected at least one Camunda engine available.");
		}

		private IRestResponse<CamundaUserProfile> GetUserProfile()
		{
			var request = new RestRequest($"/engine/engine/{_engineName}/user/demo/profile");
			request.AddCookie("JSESSIONID", _sessionID);
			var response = _client.Get<CamundaUserProfile>(request);
			CheckStatusOk("User Profile", response);
			return response;
		}

		private static void CheckStatusOk(string requestName, IRestResponse response)
		{
			if (response.StatusCode != HttpStatusCode.OK)
				throw new ApplicationException($"{requestName} HTTP request failed, status code: {response.StatusCode}");
		}
	}
}