require "rake"
require "./mongo"

task :default => :mongo

desc "Create root path"
mongo :mongo do |x|
	puts "Enter the mongo admin username: "
	username = gets.chomp
	puts "Enter the mongo admin password: "
	password = gets.chomp
    x.mongo_path = "E:/Software/MongoDB/bin"
	x.db_name = "admin"
	x.username = username
	x.password = password
	x.execute_script("mongo.js")
end