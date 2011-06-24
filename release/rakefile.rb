require "albacore"
require "release/robocopy"
require "release/common"

task :default => [:acceptanceTests]

desc "Inits the build"
task :initBuild do
	Common.EnsurePath("reports")
end

desc "Builds the library."
msbuild :buildLibrary => :initBuild do |msb|
    msb.properties :configuration => :Release
    msb.targets :Clean, :Build
    msb.solution = "src/Volta.Core/Volta.Core.csproj"
end

desc "Builds the website."
msbuild :buildWebsite => :buildLibrary do |msb|
    msb.properties :configuration => :Release
    msb.targets :Clean, :Build
    msb.solution = "src/Volta.Web/Volta.Web.csproj"
end

desc "Builds the test project."
msbuild :buildTestProject => :buildWebsite do |msb|
    msb.properties :configuration => :Release
    msb.targets :Clean, :Build
    msb.solution = "src/Volta.Tests/Volta.Tests.csproj"
end

desc "Unit and integration tests"
nunit :unitTests => :buildTestProject do |nunit|
	nunit.command = "src/packages/NUnit.2.5.10.11092/tools/nunit-console.exe"
	nunit.assemblies "src/Volta.Tests/bin/Release/Volta.Tests.dll"
	nunit.options "/xml=reports/TestResult.xml", "/run:Volta.Tests.Unit,Volta.Tests.Integration"
end

desc "Deploys the site."
robocopy :deploy => :unitTests do |rc|
    rc.source = "src/Volta.Web"
    rc.target = "D:/Websites/volta.groupsadoway.org/wwwroot"
    rc.excludeDirs = "obj"
    rc.includeFiles = "*.dll *.config *.spark *.cshtml *.htm *.html *.txt *.css *.asax " \
                      "*.gif *.jpg *.jpeg *.png *.xml *.js *.ico *.xsl"
    rc.logPath = "volta.groupsadoway.org.log"
end

desc "Acceptance tests"
nunit :acceptanceTests => :deploy do |nunit|
	nunit.command = "src/packages/NUnit.2.5.10.11092/tools/nunit-console.exe"
	nunit.assemblies "src/Volta.Tests/bin/Release/Volta.Tests.dll"
	nunit.options "/xml=reports/TestResult.xml", "/run:Volta.Tests.Acceptance"
end
