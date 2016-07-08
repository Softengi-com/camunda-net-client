using System;
using RestSharp.Deserializers;

namespace Camunda.Client.Types
{
	public class CamundaTaskHistory
	{
		[DeserializeAs(Name = "id")]
		public Guid ID { get; set; }

		[DeserializeAs(Name = "parentTaskId")]
		public Guid? ParentTaskID { get; set; }

		[DeserializeAs(Name = "processDefinitionKey")]
		public string ProcessDefinitionKey { get; set; }

		[DeserializeAs(Name = "processDefinitionId")]
		public string ProcessDefinitionID { get; set; }

		[DeserializeAs(Name = "instanceId")]
		public string ProcessInstanceID { get; set; }

		[DeserializeAs(Name = "executionId")]
		public string ProcessExecutionID { get; set; }

		[DeserializeAs(Name = "caseDefinitionKey")]
		public string CaseDefinitionID { get; set; }

		[DeserializeAs(Name = "caseDefinitionKey")]
		public string CaseDefinitionKey { get; set; }

		[DeserializeAs(Name = "caseInstanceId")]
		public string CaseInstanceID { get; set; }

		[DeserializeAs(Name = "activityInstanceId")]
		public string ActivityInstanceId { get; set; }

		[DeserializeAs(Name = "taskDefinitionKey")]
		public string TaskDefinitionKey { get; set; }

		[DeserializeAs(Name = "name")]
		public string Name { get; set; }

		[DeserializeAs(Name = "description")]
		public string Description { get; set; }

		[DeserializeAs(Name = "deleteReason")]
		public string DeleteReason { get; set; }

		[DeserializeAs(Name = "startTime")]
		public DateTime StartTime { get; set; }

		[DeserializeAs(Name = "endTime")]
		public DateTime? EndTime { get; set; }

		[DeserializeAs(Name = "due")]
		public DateTime? Due { get; set; }

		[DeserializeAs(Name = "followUp")]
		public DateTime? FollowUp { get; set; }

		[DeserializeAs(Name = "priority")]
		public int Priority { get; set; }

		[DeserializeAs(Name = "owner")]
		public string Owner { get; set; }

		[DeserializeAs(Name = "assignee")]
		public string Assignee { get; set; }

		[DeserializeAs(Name = "tenantId")]
		public string TenantID { get; set; }

		[DeserializeAs(Name = "duration")]
		public long? Duration { get; set; }
	}
}