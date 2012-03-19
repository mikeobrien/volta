window.error = (message) ->
    console.log message
    phantom.exit 1
    
#window.onerror = (message) ->
#    console.log message
#    phantom.exit 1

Array.prototype.contains = (item) -> @.indexOf(item) > -1
Date.prototype.elapsed = -> (new Date() - @) / 1000;
String.prototype.left = (length) -> @substr 0, length
String.prototype.right = (length) -> @substr @length - length
String.prototype.startsWith = (text) -> @left(text.length).toLowerCase() == text.toLowerCase()
String.prototype.endsWith = (text) -> @right(text.length).toLowerCase() == text.toLowerCase()
String.prototype.count = (text) -> @split(text).length - 1
String.prototype.repeat = (count) -> Array(count + 1).join @
String.prototype.urlEncode = -> @.replace(/\&/g, "&amp;").replace(/</g, "&lt;").replace(/\>/g, "&gt;").replace(/\"/g, "&quot;").replace(/\'/g, "&apos;")

class Path
    @tempFilename: (extension) -> "~#{Math.random().toString().substr(2)}.#{extension}"
    @normalizePath: (path) -> path.replace(/\\/g, '/')
    @join: (paths...) -> @normalizePath paths.join('/').replace(/\/\//g, '/')
    @normalizeDirectory: (path) ->
        path = @normalizePath(path)
        if path.endsWith('/') and path.count('/') > 1 then path.left(path.length - 1) else path
    @getDirectory: (path) ->
        path = @normalizePath(path)
        path.left path.lastIndexOf('/')
    @getFilename: (path) -> 
        path = @normalizePath(path)
        path.substr path.lastIndexOf('/') + 1
    @getFilenameWithoutExt: (filename) ->
        if filename.indexOf('.') > 0 then filename.left(filename.lastIndexOf('.')) else filename
    @getPathWithoutFileExt: (path) -> 
        directory = @getDirectory(path)
        filename = @getFilenameWithoutExt(@getFilename(path))
        if directory then @join directory, filename else filename
    @getExtension: (path) ->
        filename = @getFilename path
        if filename.indexOf('.') > 0 then filename.substr(filename.lastIndexOf('.')) else ''
    @getRootPath: (paths...) ->
        if paths.length == 0 then return ''
        if paths.length == 1 then return paths[0]
        path = paths[0]
        while (p for p in paths when p.startsWith(path)).length < paths.length
            path = path.left(path.lastIndexOf('/'))
        @normalizeDirectory path
    @excludeDirectories: (paths, exclusions) ->
        paths.filter((path) -> !exclusions.some((directory) -> path.startsWith("#{directory}/")))
    @getAbsolutePath: (path) -> require('fs').absolute(path)
    @getRelativePath: (root, path) ->
        if !path 
            path = root
            root = require('fs').workingDirectory
        root = @normalizeDirectory(root)
        path = @getAbsolutePath(@normalizeDirectory(path))
        if path.left(root.length) == root then return path.substr root.length + 1
        rootPath = @getRootPath root, path
        if rootPath.length == 0 then error "Path '#{path}' is not relative to '#{root}'."
        @join('../'.repeat(root.count('/') - rootPath.count('/')), path.substr(rootPath.length + 1))
    @toRegex: (object) ->
        if object instanceof RegExp then return object
        if !arguments.callee.reserved
            reserved = ['/', '.', '+', '|', '(', ')', '[', ']', '{', '}', '\\']
            arguments.callee.reserved = new RegExp('(\\' + reserved.join('|\\') + ')', 'g')
        new RegExp("^#{object.replace(arguments.callee.reserved, '\\$1').replace('*', '.*').replace('?', '.')}$")
    @search: (path, filter) ->
        path = @normalizePath(path)
        filter = @toRegex filter
        fs = require 'fs'
        results = ({ name: object, path: @join(path, object) } for object in fs.list(path))
        dirs = (object for object in results when fs.isDirectory(object.path))
        files = (object.path for object in results when fs.isFile(object.path) and object.name.match(filter))
        files = files.concat(@search(dir.path, filter)) for dir in dirs when dir.name != '.' and dir.name != '..'
        files

class Config
    constructor: (args) ->
        fs = require('fs')
        @run = false
        @autorun = false
        @autorunFrequency = 2000
        @createRunner = false
        @runnerFilename = 'SpecRunner.html'
        @path = fs.workingDirectory
        @appFilter = 'main.js'
        @testFilter = '*.specs.js'
        @requirePath = 'require.js'
        @jasminePath = 'jasmine.js'
        @outputPath = fs.workingDirectory
        @outputFilename = 'results'
        @output = []
        @scriptPaths = []
        @modulePaths = []
        name = null
        for arg in args
            if arg.startsWith '--' 
                switch arg
                    when '--run' then @run = true
                    when '--auto-run' then @autorun = true
                    when '--create-runner' then @createRunner = true
                    else name = arg.substr(2)
            else
                if name == null then @path = arg
                else
                    switch name
                        when 'app-filter' then @appFilter = arg
                        when 'specs-path' then @testsPath = arg
                        when 'spec-filter' then @testFilter = arg
                        when 'require-path' then @requirePath = arg
                        when 'jasmine-path' then @jasminePath = arg
                        when 'output-path' then @outputPath = arg
                        when 'xml-output-path' then @xmlOutputPath = arg
                        when 'html-output-path' then @htmlOutputPath = arg
                        when 'output-filename' then @outputFilename = arg
                        when 'xml-output-filename' then @xmlOutputFilename = arg
                        when 'html-output-filename' then @htmlOutputFilename = arg
                        when 'runner-filename' then @runnerFilename = arg
                        when 'output' then @output.push arg
                        when 'script-path' then @scriptPaths.push arg
                        when 'module-path' then @modulePaths.push arg
                        when 'auto-run-frequency' then @autorunFrequency = parseInt(arg) * 1000
                        else continue
                name = null
        @testsPath = @testsPath ? @path
        
    printSummary: ->
        printOption = (text, value) -> console.log "    #{text}: #{value}"
        for option, value of @ when typeof value != 'function'
            printOption option, value
        console.log ''
        console.log '----------------------------------------------------------------------------'
        
    @printOptions: ->
        console.log 'Usage: bambino path/to/app/root [options]'
        console.log ''
        printOption = (option, description) -> console.log "  --#{option}#{' '.repeat(22 - option.length)}#{description}"
        printOption 'run', 'run all tests'
        printOption 'auto-run', 'auto run all tests'
        printOption 'auto-run-frequency', 'frequency (seconds) to auto run tests, defaults to 2'
        printOption 'create-runner', 'generate standalone test runner(s)'
        printOption 'runner-filename', 'runner name, defaults to "SpecRunner.html"'
        printOption 'app-filter', 'app file pattern, defaults to "main.js"'
        printOption 'specs-path', 'path that contains specs, defaults to app root'
        printOption 'spec-filter', 'spec file pattern, defaults to "*.specs.js"'
        printOption 'require-path', 'path to require.js files'
        printOption 'jasmine-path', 'path to jasmine.js files'
        printOption 'output-path', 'path for report output'
        printOption 'xml-output-path', 'path for xml report output, defaults to output-path'
        printOption 'html-output-path', 'path for html report output, defaults to output-path'
        printOption 'output-filename', 'filename of report, defaults to "results"'
        printOption 'xml-output-filename', 'filename of xml report, defaults to output-filename'
        printOption 'html-output-filename', 'filename of html report, defaults to output-filename'
        printOption 'output*', 'output type: xml, html'
        printOption 'script-path*', 'path to a script'
        printOption 'module-path*', 'path to a require.js module'
        console.log ''
        console.log '  * multiple allowed'

class Page
    constructor: ->
        @page = require('webpage').create()
        @page.settings.localToRemoteUrlAccessEnabled = true
        @page.onResourceRequested = (request) => @handleEvent { message: { message: "Loading #{request.url}"} }
        @page.onConsoleMessage = (message, line, source) => @handleEvent { message: { message: message, line: line, source: source} }
        
    handleEvent: (event) -> 
        console.log JSON.stringify(event)
        return true

    monitorEvents: ->
        result = true
        while event = @page.evaluate(-> window.events.shift()) 
            try
                result = @handleEvent(event)
            catch error
                result = @handleEvent({ error: { message: error } })
        @eventMonitor = setTimeout (=> @monitorEvents()), 100 unless !result
 
    stopEventMonitor: ->
        if !@eventMonitor then return
        clearTimeout(@eventMonitor)
        @eventMonitor = null

    apply: (args, func) ->
        @page.evaluate "function() { (#{func.toString()}(#{args.map((x) -> JSON.stringify(x)).join(', ')})); }"

    loadScripts: (scripts, onComplete) ->
        if scripts.length == 0 then onComplete() else @page.includeJs(scripts.shift(), => @loadScripts(scripts, onComplete))
        
    load: (scripts, onComplete) ->
        @stopEventMonitor()
        fs = require 'fs'
        templatePath = Path.getAbsolutePath(Path.tempFilename('html'))
        fs.write(templatePath, '<html><body></body></html>', 'w')
        runnerUrl = "file://#{templatePath.substr(2)}"
        @page.open runnerUrl, (status) =>
            fs.remove templatePath
            @page.evaluate ->
                window.events = []
                window.onerror = (message, source, line) ->
                    window.events.push({error: {message: message, source: source, line: line}})
            @loadScripts scripts.slice(), =>
                onComplete()
                @monitorEvents()
  
    close: -> 
        @stopEventMonitor()
        @page.release()

class JasmineTestRunner extends Page
    constructor: -> super()
        
    handleEvent: (event) ->
        if event == null then return true
        if event.error
            console.log "An error occured at #{event.error.source}:#{event.error.line}: #{event.error.message}"
            phantom.exit 1
        if event.message then console.log event.message.message
        if event.jasmine
            jasmine = event.jasmine
            if jasmine.spec
                console.log "Test '#{jasmine.spec.fullName}' #{if !jasmine.spec.failed then 'SUCCEEDED' else 'FAILED'}."
            if jasmine.summary
                succeded = (suite for suite in jasmine.summary when suite.failed != 0).length == 0
                console.log "Completed #{if succeded then 'successfully' else 'with failure(s).'}."
                @onComplete(@, jasmine.summary)
                return false
        return true
    
    save: (appPath, testsPath, runnerFilename, scripts, jasminePath, requirePaths, modules) -> 
        runner = """
            <!DOCTYPE HTML PUBLIC "-//W3C//DTD HTML 4.01 Transitional//EN"
              "http://www.w3.org/TR/html4/loose.dtd">
            <!-- Generated by bambino (https://github.com/mikeobrien/bambino) on #{new Date()} -->
            <html>
                <head>
                    <title>Jasmine Spec Runner</title>
                    <link rel="shortcut icon" type="image/png" href="#{Path.join(jasminePath, 'jasmine_favicon.png')}">
                    <link rel="stylesheet" type="text/css" href="#{Path.join(jasminePath, 'jasmine.css')}">
                    #{scripts.map((script) -> "<script type=\"text/javascript\" src=\"#{script}\"></script>").join("\r")}
                    <script type="text/javascript" src="#{Path.join(jasminePath, 'jasmine-html.js')}"></script>
                    <script type="text/javascript">
                        require.config({
                            baseUrl: #{JSON.stringify appPath},
                            paths: #{JSON.stringify requirePaths, null, 4},
                            urlArgs: "x=" + (new Date()).getTime()
                        });
                        require(#{JSON.stringify modules, null, 4}, function() {
                            jasmineEnv = jasmine.getEnv();
                            jasmineEnv.updateInterval = 1000;
                            jasmineEnv.addReporter(new jasmine.TrivialReporter());
                            jasmineEnv.execute();
                        });
                    </script>
                </head>
                <body></body>
            </html>        
        """
        require('fs').write Path.join(testsPath, runnerFilename), runner, 'w'
        
    run: (appPath, scripts, requirePaths, modules, onComplete) -> 
        @onComplete = onComplete
        @load scripts, =>
            @page.evaluate ->
                define 'JasmineReporter', ->
                    class JasmineReporter 
                        elapsed: (startTime, endTime) -> (endTime - startTime) / 1000;
                        
                        reportSpecStarting: (spec) ->
                            spec.summary = spec.summary ? { start: new Date() }
                            spec.suite.summary = spec.suite.summary ? { start: new Date() }

                        reportSpecResults: (spec) ->
                            results = spec.results()
                            spec.summary.failed = !results.passed()
                            spec.summary.fullName = spec.getFullName()
                            spec.summary.name = spec.description
                            spec.summary.end = new Date()
                            spec.summary.duration = @elapsed(spec.summary.start, new Date())
                            results = results.getItems()
                            spec.summary.messages = ({ log: result.toString() } for result in results when result.type == 'log')
                            failures = (result for result in results when result.type == 'expect' and result.passed and !result.passed())
                            spec.summary.messages = spec.summary.messages.concat({ fail: failure.message } for failure in failures when failure.message)
                            spec.summary.messages = spec.summary.messages.concat({ stack: failure.trace.stack } for failure in failures when failure.trace.stack)
                            window.events.push { jasmine: { spec: spec.summary }}

                        reportSuiteResults: (suite) ->
                            specs = suite.specs()
                            suite.summary.name = suite.getFullName()
                            suite.summary.end = new Date()
                            suite.summary.duration = @elapsed(suite.summary.start, new Date())
                            suite.summary.specs = specs.map (x) -> x.summary
                            suite.summary.failed = (spec for spec in specs when spec.summary.failed).length
                            window.events.push { jasmine: { suite: suite.summary }}

                        reportRunnerResults: (runner) ->
                            window.events.push { jasmine: { summary: runner.suites().map((x) -> x.summary) }}
                            
            @apply [appPath, requirePaths, modules], (appPath, requirePaths, modules) -> 
                console.log 'Starting Jasmine test runner...'
                
                require.config
                    baseUrl: appPath
                    paths: requirePaths
                    urlArgs: "x=" + (new Date()).getTime()

                require ['JasmineReporter'].concat(modules), (JasmineReporter) ->
                    console.log 'Running Jasmine tests...'
                    jasmineEnv = jasmine.getEnv()
                    jasmineEnv.updateInterval = 1000
                    jasmineEnv.addReporter new JasmineReporter()
                    jasmineEnv.execute()

class TestRunner
    constructor: (basePath, @appFilter, testsPath, @testFilter, @requirePath, @jasminePath, @scriptPaths, @modulePaths, @resultWriters) ->
        @basePath = Path.getAbsolutePath basePath
        @testsPath = if !testsPath then @basePath else Path.getAbsolutePath testsPath
        @scripts = [Path.join(@requirePath, 'require.js'), Path.join(@jasminePath, 'jasmine.js')].concat(@scriptPaths)
    
    findApps: ->
        console.log "Searching for #{@appFilter} under #{@basePath}..."
        apps = []
        paths = Path.search(@basePath, @appFilter).sort (a, b) -> b.length - a.length
        for path in paths
            app = { tests: {}, require: {}}
            app.path = Path.getDirectory(path)
            
            app.tests.path = Path.join(@testsPath, Path.getRelativePath(@basePath, app.path))
            testsPaths = Path.search(app.tests.path, @testFilter).map((x) -> Path.getPathWithoutFileExt(Path.getRelativePath(app.path, x)))
            app.tests.paths = Path.excludeDirectories(testsPaths, apps.map((x) => Path.getRelativePath(app.path, x.tests.path)))
            
            app.modules = @modulePaths.map((x) -> Path.getPathWithoutFileExt(Path.getRelativePath(app.path, x)))
            
            requirePaths = /paths\s*:\s*(\{\s*[\s\S]*?\s*\})/m.exec(require('fs').read(path))
            app.require.paths = if requirePaths.length > 1 then JSON.parse(requirePaths[1]) else {} 
            
            apps.push app
        apps

    save: (runnerFilename) ->
        console.log 'Creating test runners...'
        apps = @findApps()
        for app in apps
            appPath = Path.getRelativePath(app.tests.path, app.path)
            scripts = @scripts.map (script) -> Path.getRelativePath(app.tests.path, script)
            jasminePath = Path.getRelativePath(app.tests.path, @jasminePath)
            new JasmineTestRunner().save(appPath, app.tests.path, runnerFilename, scripts, jasminePath, app.require.paths, app.tests.paths.concat(app.modules))
        console.log '----------------------------------------------------------------------------'
    
    run: (onComplete) ->
        console.log 'Starting test runner...'
        summary = { start: new Date(), suites: [] }
        appQueue = @findApps()
        if appQueue.length == 0 then @runComplete(summary, onComplete)
        else @runNext(appQueue, summary, onComplete)
        
    runNext: (appQueue, summary, onComplete) ->
        if appQueue.length == 0 then return @runComplete(summary, onComplete)
        app = appQueue.pop()
        
        console.log '----------------------------------------------------------------------------'
        console.log "Running tests for #{app.path}"
        
        if app.tests.paths.length == 0
            console.log "No tests found."
            return @runNext(appQueue, summary, onComplete)
        
        console.log("Found test suite #{suite}") for suite in app.tests.paths

        onNext = (runner, suites) =>
            suites = suites.slice()
            suitePath = Path.getRelativePath(app.tests.path) || '/'
            suites.forEach((x) => x.path = suitePath)
            summary.suites = summary.suites.concat(suites)
            runner.close()
            @runNext(appQueue, summary, onComplete)
        
        appPath = Path.getRelativePath(app.path)
        new JasmineTestRunner().run(appPath, @scripts, app.require.paths, app.tests.paths.concat(app.modules), onNext)

    runComplete: (summary, onComplete) ->
        console.log '----------------------------------------------------------------------------'
        summary.end = new Date()
        summary.duration = summary.start.elapsed()
        if summary.suites.length == 0
            summary.specs = 0
            summary.failed = 0
            console.log 'Test runner did not find any tests!'
        else
            summary.specs = (suite.specs.length for suite in summary.suites).reduce (t, s) -> t + s
            summary.failed = (suite.failed for suite in summary.suites).reduce (t, s) -> t + s
            console.log "Test runner completed #{if summary.failed == 0 then 'successfully' else 'with failures'} yo."
        console.log '----------------------------------------------------------------------------'
        @resultWriters.forEach (x) => x.writeSummary summary
        onComplete summary

class ResultWriter
    constructor: (path, filename) -> 
        @path = Path.getAbsolutePath(path)
        @filename = filename
        @data = ''
    write: (data) -> @data += data
    writeln: (data) -> @write data + "\r"
    save: (path, filename) -> 
        data = @data
        @data = ''
        require('fs').write Path.join(path ? @path, filename ? @filename), data, 'w'
        
class XmlWriter extends ResultWriter
    constructor: (path, filename) -> 
        if !Path.getExtension(filename) then filename += '.xml'
        super(path, filename)
    writeSummary: (summary) ->
        @write """<testsuites tests="#{summary.specs}" failures="#{summary.failed}" disabled="0" """
        @writeln """errors="0" time="#{summary.duration}" timestamp="#{summary.end.toISOString()}">"""
        
        for suite in summary.suites
            @write """\t<testsuite name="#{suite.name} (#{suite.path.urlEncode()})" tests="#{suite.specs.length}" failures="#{suite.failed}" """
            @writeln """disabled="0" errors="0" time="#{suite.duration}">"""
            
            for spec in suite.specs    
                @writeln """\t\t<testcase name="#{spec.name}" status="run" time="#{spec.duration}" classname="#{spec.name}">"""
                
                for message in spec.messages when message.fail or message.stack
                    @write """\t\t\t<failure message="#{(message.log ? message.fail ? '').urlEncode()}" type="">"""
                    @writeln """<![CDATA[#{message.stack ? message.fail ? ''}]]></failure>"""
                    
                @writeln '\t\t</testcase>'

            @writeln '\t</testsuite>'
                                  
        @writeln '</testsuites>'
        @save()

class HtmlWriter extends ResultWriter
    constructor: (path, filename, @autoReload, @autoReloadInterval, @linkToStandAloneRunner, @runnerFilename) -> 
        if !Path.getExtension(filename) then filename += '.html'
        super(path, filename)
    writeSummary: (summary) ->
        @writeln """
            <html><head><title>Jasmine Spec Runner</title>
            <style>
                body { font-family: "Helvetica Neue Light", "Lucida Grande", "Calibri", "Arial", sans-serif; }
                .banner { color: #303; background-color: #fef; padding: 5px; }
                .logo { float: left; font-size: 1.1em; padding-left: 5px; }
                .options { text-align: right; font-size: .8em; }
                .suite { border: 1px outset gray; margin: 5px 0; padding-left: 1em; } .suite .suite { margin: 5px; }
                .suite.passed { background-color: #dfd; } .suite.failed { background-color: #fdd; }
                .spec { margin: 5px; padding-left: 1em; clear: both; }
                .spec.failed, .spec.passed, .spec.skipped { padding-bottom: 5px; border: 1px solid gray; }
                .spec.failed { background-color: #fbb; border-color: red; }
                .spec.passed { background-color: #bfb; border-color: green; }
                .finished-at { padding-left: 1em; font-size: .6em; }
                .messages { border-left: 1px dashed gray; padding-left: 1em; padding-right: 1em; }
                .resultMessage span.result { display: block; line-height: 2em; color: black; }
                .resultMessage .mismatch { color: black; }
                .stackTrace { white-space: pre; font-size: .8em; margin-left: 10px; max-height: 5em; 
                              overflow: auto; border: 1px inset red; padding: 1em; background: #eef; }            
                .passed { background-color: #cfc; display: none; } .failed { background-color: #fbb; }
                .show-passed .passed, .show-skipped .skipped { display: block; }
                .runner { border: 1px solid gray; display: block; margin: 5px 0; padding: 2px 0 2px 10px; }
            </style>
            <script>
                #{if @autoReload then "window.setInterval(function() {location.reload();}, #{@autoReloadInterval});" else ''}
                function toggleTestVisibility(enabled) {
                    reporter = document.getElementById('reporter');
                    if (enabled) { reporter.className += ' show-passed'; } 
                    else { reporter.className = reporter.className.replace(/ show-passed/, ''); }
                }   
            </script>
            </head>
            <body>
                <div class="jasmine_reporter" id="reporter">
                    <div class="banner">
                        <div class="logo"><span class="title">Jasmine</span></div>
                        <div class="options">
                            <input id="showPassed" type="checkbox" onclick="toggleTestVisibility(this.checked)">
                            <label for="showPassed"> passed </label>
                        </div>
                    </div>

                    <div class="runner #{if summary.failed == 0 then 'passed' else 'failed'}">
                        <span>#{summary.specs} spec#{if summary.specs != 1 then 's' else ''}, 
                              #{summary.failed} failure#{if summary.failed != 1 then 's' else ''} in #{summary.duration}s</span>
                        <span class="finished-at">Finished at #{summary.end}</span>
                    </div>
        """
        
        for suite in summary.suites
            @writeln """<div class="suite #{if suite.failed == 0 then 'passed' else 'failed'}"><span>
                        #{suite.name} (<a #{if @linkToStandAloneRunner then "href=\"#{Path.join(suite.path, @runnerFilename)}\"" else ''} target="_blank">#{suite.path}</a>)</span>"""
            
            for spec in suite.specs    
                @writeln """<div class="spec #{if spec.failed then 'failed' else 'passed'}"><span>#{spec.name}</span><div class="messages">"""
                
                for message in spec.messages    
                    type = if message.stack then 'stackTrace' else 'resultMessage ' + (if message.log then 'log' else 'fail')
                    @writeln """<div class="#{type}">#{(message.log ? message.stack ? message.fail).urlEncode()}</div>"""
                    
                @writeln '</div></div>'

            @writeln "</div>"

        @writeln "</div></body></html>"
        @save()

console.log 'Bambino Test Runner'
console.log ''

config = new Config(phantom.args)

if !config.run and !config.autorun and !config.createRunner
    Config.printOptions()
    phantom.exit()
 
resultWriters = []
if config.output.contains('xml') 
    resultWriters.push new XmlWriter(config.xmlOutputPath ? config.outputPath, config.xmlOutputFilename ? config.outputFilename)
if config.output.contains('html') 
    resultWriters.push new HtmlWriter(config.htmlOutputPath ? config.outputPath, config.htmlOutputFilename ? config.outputFilename, 
                                      config.autorun, config.autorunFrequency, config.createRunner, config.runnerFilename)

runner = new TestRunner(config.path, config.appFilter, config.testsPath, config.testFilter, config.requirePath, 
                        config.jasminePath, config.scriptPaths, config.modulePaths, resultWriters)

config.printSummary()    
                    
if config.autorun
    runTests = -> 
        if config.createRunner then runner.save(config.runnerFilename)
        runner.run((summary) -> window.setTimeout(runTests, config.autorunFrequency))
    runTests()
else
    if config.createRunner then runner.save(config.runnerFilename)
    if config.run then runner.run((summary) -> phantom.exit if summary.failed == 0 then 0 else 1)
    else phantom.exit()

