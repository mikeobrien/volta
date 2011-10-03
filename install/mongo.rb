def mongo(*args, &block)
    body = lambda { |*args|
        task = Tasks::Mongo::Task.new
        block.call(task)
        task.run }
    Rake::Task.define_task(*args, &body)
end

module Tasks
    module Mongo
       class Task
			attr_accessor :mongo_path, :db_name, :username, :password
			
            def initialize() 
                @scripts = Array.new
            end 

            def execute_script(*path)
                @scripts << File.join(*path)
            end

            def run()
                if @scripts.length > 0 then execute_scripts(@scripts) end
            end
            
            private
            
            def execute_scripts(scripts)
                puts
                puts "Executing mongo scripts..."
                scripts.each do |path|
					puts "#{mongo_path}/mongo -u #{username} -p #{password} #{db_name} #{path}"
                    system("#{mongo_path}/mongo -u #{username} -p #{password} #{db_name} #{path}")
                end
            end
        end
    end
end