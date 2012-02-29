require "fileutils"

def compile_coffeescript(*args, &block)
	body = lambda { |*args|
		task = CoffeeCompiler.new
		block.call(task)
		task.run
	}
	Rake::Task.define_task(*args, &body)
end
	
class CoffeeCompiler

    attr_accessor :node_path, :coffee_path, :path
    
    def run()
    
        node_path = 'node' #File.join @node_path, 'node.exe'
        coffee_path = File.join @coffee_path, 'coffee'
        errors = false
        
        Dir.glob(File.join(@path, '**/*.coffee')) do |path|
            puts "Compiling coffee script #{path}..."
            command = "\"#{node_path}\" \"#{coffee_path}\" -b -c \"#{File.expand_path(path)}\""
            result = `#{command} 2>&1`
            puts result
            if $? != 0 then
                puts command
                puts "Coffeescript compiler failed: #{$?}."
                errors = true
            else
                puts "Success."
            end
        end
    
		fail "Coffeescript compiliation failed." unless !errors
    end
	
end
