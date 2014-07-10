#!/bin/sh -x

mono --runtime=v4.0 .nuget/NuGet.exe install xunit.runners -Version 2.0.0-beta-build2700 -o packages
mono --runtime=v4.0 .nuget/NuGet.exe install xunit.core -Version 2.0.0-beta-build2700 -o packages

runTest(){
   mv packages/xunit.core.2.0.0-beta-build2700/tools/*.dll packages/xunit.runners.2.0.0-beta-build2700/tools/
   mono --runtime=v4.0 packages/xunit.runners.2.0.0-beta-build2700/tools/xunit.console.exe $@ -xml coverage.xml
   if [ $? -ne 0 ]
   then   
     exit 1
   fi
}

runTest $1

exit $?