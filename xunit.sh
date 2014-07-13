#!/bin/sh -x

mono --runtime=v4.0 .nuget/NuGet.exe install xunit.runners -Version 2.0.0-beta-build2700 -o packages
mono --runtime=v4.0 .nuget/NuGet.exe install xunit.core -Version 2.0.0-beta-build2700 -o packages

runTest(){
   mkdir build
   cp packages/xunit.runners.2.0.0-beta-build2700/tools/* ./build
   cp packages/xunit.core.2.0.0-beta-build2700/lib/portable-net45+win+wpa81+wp80+monotouch+monoandroid/* ./build
   cp $1* ./build
   
   mono --runtime=v4.0 xunit.console.exe ./build/$2
   if [ $? -ne 0 ]
   then   
     exit 1
   fi
}

runTest test/FeatureFlipper.Tests/bin/Debug/ FeatureFlipper.Tests.dll
runTest test/FeatureFlipper.Unity.Tests/bin/Debug/ FeatureFlipper.Unity.Tests.dll
runTest test/FeatureFlipper.WebApi.Tests/bin/Debug/ FeatureFlipper.WebApi.Tests.dll

exit 