require "rexml/document"
include REXML

class XmlConfig

    attr_accessor :xmlFile, :xmlRoot, :settings
    
    def initialize()
        @settings = Array.new
    end
    
    def setAttribute(value, xpath)
        settings.push(Setting.new(value, xpath))
    end
    
    def run()
        document = Document.new File.new(xmlFile)
        document.context[:attribute_quote] = :quote
        settings.each { |setting| 
                                xpath = xmlRoot + setting.xpath
                                attribute = XPath.first(document, xpath)
                                if attribute == nil then raise "Could not find xpath '#{xpath}' in #{xmlFile}." end
                                attribute.element.attributes[attribute.name] = setting.value
                           }
        formatter = Formatters::Pretty.new
        File.open(xmlFile, 'w') do |result|
            formatter.write(document, result)
        end
    end
    
    class Setting
        attr_accessor :value, :xpath
        def initialize(value, xpath)
            @value = value
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
    
    