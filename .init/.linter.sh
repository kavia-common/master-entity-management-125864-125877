#!/bin/bash
cd /home/kavia/workspace/code-generation/master-entity-management-125864-125877/backend_api
dotnet build --no-restore -v quiet -nologo -consoleloggerparameters:NoSummary /p:TreatWarningsAsErrors=false
LINT_EXIT_CODE=$?
if [ $LINT_EXIT_CODE -ne 0 ]; then
  exit 1
fi

