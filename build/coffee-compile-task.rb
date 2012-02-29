require "fileutils"

def compile_coffee(*args, &block)
	body = lambda { |*args|
		task = CoffeeCompiler.new
		block.call(task)
		task.run
	}
	Rake::Task.define_task(*args, &body)
end
	
class CoffeeCompiler

    attr_accessor :node_path, :coffee_path, :path, :report_path
    
    def run()
    
        node_path = File.join @node_path, 'node.exe'
        coffee_path = File.join @coffee_path, 'coffee'
        errors = false
        
        Dir.glob(File.join(@path, '**/*.coffee')) do |path|
            puts "Compiling coffee script #{path}"
            result = system("\"#{node_path}\" \"#{coffee_path}\" -b -c \"#{File.expand_path(path)}\"")
            puts "Coffeescript compiler failed: #{$?}." unless result
            errors = true unless result
        end
    
		fail "Coffeescript compiliation failed." unless !errors
    end
	
end
