require "yaml"
require "rexml/document"
include REXML

class XmlConfig

    attr_accessor :yamlFile, :yamlSection, :xmlFile, :xmlRoot, :yamlXmlMap
    
    def initialize()
        @yamlXmlMap = Array.new
    end
    
    def setAttribute(yamlKey, xpath)
        yamlXmlMap.push(YamlXmlMapping.new(yamlKey, xpath))
    end
    
    def run()
        yaml = YAML::load(File.open(yamlFile))
        document = Document.new File.new(xmlFile)
        document.context[:attribute_quote] = :quote
        yamlXmlMap.each { |mapping| 
                                xpath = xmlRoot + mapping.xpath
                                attribute = XPath.first(document, xpath)
                                if attribute == nil then raise "Could not find xpath '#{xpath}' in #{xmlFile}." end
                                attribute.element.attributes[attribute.name] = 
                                    yamlSection != nil ? yaml[yamlSection][mapping.yamlKey] : yaml[mapping.yamlKey]
                           }
        formatter = Formatters::Pretty.new
        File.open(xmlFile, 'w') do |result|
            formatter.write(document, result)
        end
    end
    
    class YamlXmlMapping
        attr_accessor :yamlKey, :xpath
        def initialize(yamlKey, xpath)
            @yamlKey = yamlKey
            @xpath = xpath
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
    
    