# Summary

Project is basically a playgroung to study Camunda and its REST API, but I do not deny that I could add here a few useful things like CLI to deploy workflows.

Anyway, it contains useful code snippets for anyone who wants to integrate with Camunda from .Net applications.

# Command Line Interface to Camunda

The following is an example of how to create a deployment at Camunda server:
```
CamundaCLI.exe deploy -n "Sample deployment" -c http://camunda.deployment.yours/api/ -u demo -p demo -f test.bpmn startForm.html
CamundaCLI.exe deploy -n "Sample deployment" -s "My deployment source" -c http://camunda.deployment.yours/api/ -u demo -p demo -f directoryWithFiles
```
