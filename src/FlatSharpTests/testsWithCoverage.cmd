dotnet test /p:AltCover=true /p:AltCoverAttributeFilter=ExcludeFromCodeCoverage
dotnet %UserProfile%\.nuget\packages\reportgenerator\4.1.2\tools\netcoreapp2.1\ReportGenerator.dll -reports:coverage.netcoreapp2.1.xml -targetdir:.coverage\