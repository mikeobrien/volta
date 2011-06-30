class Gallio

    attr_accessor :verbosity, :noResults, :noProgress, :noLogo, :doNotRun,
                  :ignoreAnnotations, :runtimeLimit, :runnerType, :runnerExtensions,
                  :runnerProperties, :filter, :reportDirectory, :reportNameFormat, 
                  :reportArchive, :reportTypes, :reportFormatterProperties, :showReports,
                  :testAssemblies, :hintDirectories, :pluginDirectories, :applicationBaseDirectory,
                  :workingDirectory, :shadowCopy, :debug, :runtimeVersion, :echoCommandLine
    
    def initialize()
        @testAssemblies = Array.new
        @hintDirectories = Array.new
        @pluginDirectories = Array.new
        @runtimeLimit = -1
        @runnerExtensions = Array.new
        @runnerProperties = Array.new
        @reportTypes = Array.new
        @reportFormatterProperties = Array.new
    end
    
    def addTestAssembly(assembly)
        testAssemblies.push(assembly)
    end
    
    def addHintDirectory(directory)
        hintDirectories.push(directory)
    end
    
    def addPluginDirectory(directory)
        pluginDirectories.push(directory)
    end
    
    def addRunnerExtension(extension)
        runnerExtensions.push(extension)
    end
    
    def addRunnerProperty(property)
        runnerProperties.push(property)
    end
    
    def addReportType(type)
        reportTypes.push(type)
    end
    
    def addReportFormatterProperty(property)
        reportFormatterProperties.push(property)
    end
    
    def run()
        
        gallio = "gallio.echo "
        
        if testAssemblies.count > 0 then gallio += "#{testAssemblies.collect{|x| "\"#{x}\"" }.join(" ")} " end
        if hintDirectories.count > 0 then gallio += "#{hintDirectories.collect{|x| "\"/hd:#{x}\"" }.join(" ")} " end
        if pluginDirectories.count > 0 then gallio += "#{pluginDirectories.collect{|x| "\"/pd:#{x}\"" }.join(" ")} " end
        if applicationBaseDirectory != nil then gallio += "\"/abd:#{applicationBaseDirectory}\" " end
        if workingDirectory != nil then gallio += "\"/wd:#{workingDirectory}\" " end
        if shadowCopy == true then gallio += "/sc " end
        if debug == true then gallio += "/d " end
        if runtimeVersion != nil then gallio += "\"/rv:#{runtimeVersion}\" " end
        
        # Quiet, Normal, Verbose, Debug
        if verbosity != nil then gallio += "/v:#{verbosity} " end
        
        if noResults == true then gallio += "/ne " end
        if noProgress == true then gallio += "/np " end
        if noLogo == true then gallio += "/nl " end
        if doNotRun == true then gallio += "/dnr " end
        if ignoreAnnotations == true then gallio += "/ia " end
        if runtimeLimit > -1 then gallio += "/rtl:#{runtimeLimit} " end
        
        # IsolatedProcess, IsolatedAppDomain, Local
        if runnerType != nil then gallio += "/r:#{runnerType} " end
        
        if runnerExtensions.count > 0 then gallio += "#{runnerExtensions.collect{|x| "\"/re:#{x}\"" }.join(" ")} " end
        if runnerProperties.count > 0 then gallio += "#{runnerProperties.collect{|x| "\"/rp:#{x}\"" }.join(" ")} " end
        
        if filter != nil then gallio += "\"/f:#{filter}\" " end
        
        if reportDirectory != nil then gallio += "\"/rd:#{reportDirectory}\" " end
        if reportNameFormat != nil then gallio += "\"/rnf:#{reportNameFormat}\" " end
        if reportArchive != nil then gallio += "/ra:#{reportArchive} " end
        
        # Xml, Xml-Inline, Text, Text-Condensed, Html, Html-Condensed, XHtml, XHtml-Condensed, MHtml, MHtml-Condensed
        if reportTypes.count > 0 then gallio += "#{reportTypes.collect{|x| "/rt:#{x}" }.join(" ")} " end
        
        if reportFormatterProperties.count > 0 then gallio += "#{reportFormatterProperties.collect{|x| "\"/rfp:#{x}\"" }.join(" ")} " end
        if showReports == true then gallio += "/sr " end

        errorHandler = \
            lambda do |ok, res|
                       raise "Could not find gallio.echo.exe. " \
                             "Make sure it is added to your path." \
                       if res.exitstatus == 127
                       raise "Gallio failed with exit " \
                             "code #{res.exitstatus}." \
                       if res.exitstatus > 0
                   end

        if echoCommandLine = true then puts gallio end
        
        sh gallio, &errorHandler 
    end
    
end

def gallio(*args, &block)
    body = lambda { |*args|
        rc = Gallio.new
        block.call(rc)
        rc.run
    }
    Rake::Task.define_task(*args, &body)
end
    