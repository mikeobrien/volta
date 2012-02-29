require "albacore"
require_relative "robocopy-task"
require_relative "gallio-task"
require_relative "common"
require_relative "xml-config-task"
require_relative "dynamic-tasks"
require_relative "coffee-compile-task"

reportsPath = "reports"
version = ENV["BUILD_NUMBER"]
projects = ['Volta.Core', 'Volta.Web', 'Volta.Tests']

task :build_and_deploy => :acceptance_tests

task :initialize do
    Common.DeleteDirectory(reportsPath)
	Common.EnsurePath(reportsPath)
end

tasks :assembly_info, projects do |task, project|
    assemblyinfo task => :initialize do |options|
        options.version = version
        options.company_name = "Sadoway Group"
        options.product_name = "Volta"
        options.title = project.gsub('.', '')
        options.description = project.gsub('.', '')
        options.copyright = "Copyright (c) #{Time.new().year} Sadoway Group"
        options.output_file = "src/#{project}/Properties/AssemblyInfo.cs"
    end
end

tasks :compile, projects do |task, project|
    msbuild task => :assembly_info do |options|
        options.properties :configuration => :Release
        options.targets :Clean, :Build
        options.solution = "src/#{project}/#{project}.csproj"
    end
end

compile_coffee :compile_coffee => :compile do |options|
    options.path = '.'
    options.report_path = './artifacts'
    options.node_path = 'D:/Software/node.js/node.exe'
    options.coffee_path = 'D:/Software/coffeescript/coffee'
end

xml_config :test_config_settings => :compile_coffee do |options|
    options.xml_file = "src/Volta.Tests/bin/Release/Volta.Tests.dll.config"
    options.xml_root = "configuration"
    options.set_node("connectionStrings/add[@name='VoltaIntegration']/@connectionString", ENV["VOLTA_TEST_CONN_STRING"])
    options.set_node("connectionStrings/add[@name='VoltaAcceptance']/@connectionString", ENV["VOLTA_PROD_CONN_STRING"])
    options.set_node("appSettings/add[@key='VoltaUrl']/@value", ENV["VOLTA_URL"])
end

gallio :unit_tests => :test_config_settings do |options|
    options.echo_command_line = true
    options.working_directory = Dir.getwd
    options.add_test_assembly("src/Volta.Tests/bin/Release/Volta.Tests.dll")
    options.verbosity = "Normal"
    options.filter = "Namespace: /Volta.Tests.Unit.*/"
    options.report_directory = File.join(reportsPath, "gallio")
    options.report_name_format = "gallio-unit"
    options.add_report_type("Html")
end

gallio :integration_tests => :unit_tests do |options|
    options.echo_command_line = true
    options.working_directory = Dir.getwd
    options.add_test_assembly("src/Volta.Tests/bin/Release/Volta.Tests.dll")
    options.verbosity = "Normal"
    options.filter = "Namespace: /Volta.Tests.Integration.*/"
    options.report_directory = File.join(reportsPath, "gallio")
    options.report_name_format = "gallio-integration"
    options.add_report_type("Html")
end

xml_config :website_config_settings => :integration_tests do |options|
    options.xml_file = "src/Volta.Web/Web.config"
    options.xml_root = "/configuration/"
    options.set_node("volta/@connectionString", ENV["VOLTA_PROD_CONN_STRING"])
    options.set_node("log4net/appender[@name='LogFileAppender']/file/@value", ENV["VOLTA_LOG_FILE_PATH"])
    options.set_node("log4net/appender[@name='EmailAppender']/smtpHost/@value", ENV["VOLTA_SMTP_HOST"])
end

robocopy :deploy => :website_config_settings do |options|
    options.source = "src/Volta.Web"
    options.target = "D:/Websites/volta.groupsadoway.org/wwwroot"
    options.exclude_dirs("obj")
    options.include_files("*.dll *.config *.spark *.cshtml *.htm *.html *.txt *.css *.asax " \
                          "*.gif *.jpg *.jpeg *.png *.xml *.js *.ico *.xsl")
    options.log_path = File.join(reportsPath, "deploy.log")
end

gallio :acceptance_tests => :deploy do |options|
    options.echo_command_line = true
    options.working_directory = Dir.getwd
    options.add_test_assembly("src/Volta.Tests/bin/Release/Volta.Tests.dll")
    options.verbosity = "Normal"
    options.filter = "Namespace: /Volta.Tests.Acceptance.*/"
    options.report_directory = File.join(reportsPath, "gallio")
    options.report_name_format = "gallio-acceptance"
    options.add_report_type("Html")
end
