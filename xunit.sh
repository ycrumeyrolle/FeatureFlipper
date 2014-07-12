#!/bin/sh -x

mono --runtime=v4.0 .nuget/NuGet.exe install xunit.runners -Version 2.0.0-beta-build2700 -o packages
mono --runtime=v4.0 .nuget/NuGet.exe install xunit.core -Version 2.0.0-beta-build2700 -o packages

runTest(){
   cp packages/xunit.core.2.0.0-beta-build2700/lib/portable-net45+win+wpa81+wp80+monotouch+monoandroid/* ./
   cp $@ ./
   
   mono --runtime=v4.0 packages/xunit.runners.2.0.0-beta-build2700/tools/xunit.console.exe ./FeatureFlipper.Tests.dll -xml coverage.xml
   if [ $? -ne 0 ]
   then   
     exit 1
   fi
}

runTest $1

exit $?