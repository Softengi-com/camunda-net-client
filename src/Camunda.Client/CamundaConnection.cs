using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;

using Camunda.Client.Types;

using RestSharp;
using RestSharp.Authenticators;

namespace Camunda.Client
{
	public class CamundaConnection
	{
		public CamundaConnection(string baseUri, string engineName = null)
		{
			_client = new RestClient(baseUri);
			_engineName = engineName;
		}

		public string TentantID { get; set; }

		public CamundaUserProfile Authenticate(string userName, string password)
		{

			//http://ubpm.qa.softengi.com:8080/camunda/api/engine/engine/default/deployment/76fa43f2-3ee3-11e6-958f-902b34e38592/resources

			var request = new RestRequest("/admin/auth/user/default/login/tasklist")
				.AddParameter("username", userName)
				.AddParameter("password", password);

			var authResponse = _client.Post<AuthenticationResponse>(request);
			CheckStatusOk("Authentication", authResponse);

			var cookie = authResponse.Cookies.FirstOrDefault(c => c.Name == "JSESSIONID");
			if (cookie == null)
				throw new ApplicationException("Expected 'JSESSIONID' cookie");
			_sessionID = cookie.Value;

			_client.Authenticator = new HttpBasicAuthenticator(userName, password);

			SetEngineName();
			return GetUserProfile().Data;
		}

		public void GetTask(Guid taskID)
		{
			var request = new RestRequest($"/engine/engine/{_engineName}/task/{taskID}");
			request.AddCookie("JSESSIONID", _sessionID);
			var resp = _client.Get(request);
			CheckStatusOk("Get task history", resp);
			// TODO:
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

		/// <summary>
		/// </summary>
		/// <param name="deploymentName">The name for the deployment to be created.</param>
		/// <param name="deploymentSource"></param>
		/// <param name="filePaths">List of files to deploy.</param>
		/// <param name="enableDuplicateFiltering">
		/// A flag indicating whether the process engine should perform duplicate checking for the deployment or not.
		/// This allows you to check if a deployment with the same name and the same resouces already exists and if true,
		/// not create a new deployment but instead return the existing deployment.
		/// The default value is false.
		/// </param>
		public DeploymentCreateResponse Deploy(string deploymentName, string deploymentSource, string[] filePaths,
			bool enableDuplicateFiltering = false)
		{
			var request = new RestRequest($"/engine/engine/{_engineName}/deployment/create")
				.AddParameter("deployment-name", deploymentName);
			if (deploymentSource != null)
				request.AddParameter("deployment-source", deploymentSource);
			if (enableDuplicateFiltering)
				request.AddParameter("enable-duplicate-filtering", true);

			foreach (var filePath in filePaths)
			{
				var fileBytes = File.ReadAllBytes(filePath);
				var fileName = Path.GetFileName(filePath);
				request.AddFile("data-" + filePath, fileBytes, fileName);
			}

			if (TentantID != null)
				request.AddParameter("tenant-id", TentantID);
			request.AddCookie("JSESSIONID", _sessionID);

			var response = _client.Post<DeploymentCreateResponse>(request);
			CheckStatusOk("Deploy", response);

			return response.Data;
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
			var request = new RestRequest($"/engine/engine/{_engineName}/user/demo/profile")
				.AddCookie("JSESSIONID", _sessionID);
			var response = _client.Get<CamundaUserProfile>(request);
			CheckStatusOk("User Profile", response);
			return response;
		}

		static private void CheckStatusOk(string requestName, IRestResponse response)
		{
			if (response.StatusCode != HttpStatusCode.OK)
				throw new ApplicationException($"{requestName} HTTP request failed, status code: {response.StatusCode}");
		}

		private readonly RestClient _client;
		private string _engineName;
		private string _sessionID;
	}
}