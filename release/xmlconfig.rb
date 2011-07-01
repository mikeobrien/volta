require "yaml"
require "rexml/document"
include REXML

class XmlConfig

    attr_accessor :yamlFile, :configFile, :yamlConfigMap
    
    def initialize()
        @yamlConfigMap = Hash.new
    end
    
    def setValue(key, xpath)
        yamlConfigMap[key] = xpath
    end
    
    def run()
        yaml = YAML::load(File.open(yamlFile))
        xml = Document.new File.new(configFile)
        xml.context[:attribute_quote] = :quote
        yamlConfigMap.each { |key, value| 
                                xpath = "/configuration/" + value
                                attribute = XPath.first(xml, xpath)
                                if attribute == nil then raise "Could not find xpath '#{xpath}' in #{configFile}." end
                                attribute.element.attributes[attribute.name] = yaml[key]
                           }
        formatter = Formatters::Pretty.new
        File.open(configFile, 'w') do |result|
            formatter.write(xml, result)
        end
    end
    
end

def xmlConfig(*args, &block)
    body = lambda { |*args|
        xmlConfig = XmlConfig.new
        block.call(xmlConfig)
        xmlConfig.run
    }
    Rake::Task.define_task(*args, &body)
end
    