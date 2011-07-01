require "albacore"
require "release/robocopy"
require "release/gallio"
require "release/common"
require "release/xmlconfig"

task :default => [:acceptanceTests]

desc "Inits the build"
task :initBuild do
    Common.DeleteDirectory("reports")
	Common.EnsurePath("reports")
end

desc "Generate core assembly info."
assemblyinfo :coreAssemblyInfo => :initBuild do |asm|
    asm.version = ENV["GO_PIPELINE_LABEL"]
    asm.company_name = "Sadoway Group"
    asm.product_name = "Volta"
    asm.title = "Volta Core"
    asm.description = "Volta core library."
    asm.copyright = "Copyright (c) 2011 Sadoway Group"
    asm.output_file = "src/Volta.Core/Properties/AssemblyInfo.cs"
end

desc "Generate web assembly info."
assemblyinfo :webAssemblyInfo => :coreAssemblyInfo do |asm|
    asm.version = ENV["GO_PIPELINE_LABEL"]
    asm.company_name = "Sadoway Group"
    asm.product_name = "Volta"
    asm.title = "Volta Web"
    asm.description = "Volta website."
    asm.copyright = "Copyright (c) 2011 Sadoway Group"
    asm.output_file = "src/Volta.Web/Properties/AssemblyInfo.cs"
end

desc "Builds the library."
msbuild :buildLibrary => :webAssemblyInfo do |msb|
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

desc "Test config file replacements"
xmlConfig :testConfigReplacements => :buildTestProject do |o|
    o.yamlFile = "ci.yml"
    o.configFile = "Web.config"
    o.setValue("integration.test.connection.string", "connectionStrings/add[@name='VoltaIntegration']/@connectionString")
    o.setValue("acceptance.test.connection.string", "connectionStrings/add[@name='VoltaAcceptance']/@connectionString")
    o.setValue("volta.url", "appSettings/add[@key='VoltaUrl']/@value")
end

desc "Unit tests"
gallio :unitTests => :testConfigReplacements do |o|
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

desc "Website config file replacements"
xmlConfig :websiteConfigReplacements => :integrationTests do |o|
    o.yamlFile = "ci.yml"
    o.configFile = "Web.config"
    o.setValue("connection.string", "volta/@connectionString")
    o.setValue("log.file.path", "log4net/appender[@name='LogFileAppender']/file/@value")
    o.setValue("smtp.host", "log4net/appender[@name='EmailAppender']/smtpHost/@value")
end

desc "Deploys the site."
robocopy :deploy => :websiteConfigReplacements do |rc|
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
