require "albacore"
require "release/robocopy"
require "release/gallio"
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

desc "Unit tests"
gallio :unitTests => :buildTestProject do |o|
    o.echoCommandLine = true
    o.workingDirectory = Dir.getwd
    o.addTestAssembly("src/Volta.Tests/bin/Release/Volta.Tests.dll")
    o.verbosity = "Normal"
    o.filter = "Namespace: /Volta.Tests.Unit.*/"
    o.reportDirectory = "reports"
    o.reportNameFormat = "gallio-unit"
    o.addReportType("Html")
end

desc "Integration tests"
gallio :integrationTests => :unitTests do |o|
    o.echoCommandLine = true
    o.workingDirectory = Dir.getwd
    o.addTestAssembly("src/Volta.Tests/bin/Release/Volta.Tests.dll")
    o.verbosity = "Normal"
    o.filter = "Namespace: /Volta.Tests.Integration.*/"
    o.reportDirectory = "reports"
    o.reportNameFormat = "gallio-integration"
    o.addReportType("Html")
end

desc "Deploys the site."
robocopy :deploy => :integrationTests do |rc|
    rc.source = "src/Volta.Web"
    rc.target = "D:/Websites/volta.groupsadoway.org/wwwroot"
    rc.excludeDirs = "obj"
    rc.includeFiles = "*.dll *.config *.spark *.cshtml *.htm *.html *.txt *.css *.asax " \
                      "*.gif *.jpg *.jpeg *.png *.xml *.js *.ico *.xsl"
    rc.logPath = "reports/deploy.log"
end

desc "Acceptance tests"
gallio :acceptanceTests => :deploy do |o|
    o.echoCommandLine = true
    o.workingDirectory = Dir.getwd
    o.addTestAssembly("src/Volta.Tests/bin/Release/Volta.Tests.dll")
    o.verbosity = "Normal"
    o.filter = "Namespace: /Volta.Tests.Acceptance.*/"
    o.reportDirectory = "reports"
    o.reportNameFormat = "gallio-acceptance"
    o.addReportType("Html")
end
