module ApplicationHelper
 
  require 'csv'
  require 'json'
 
class Dictionary
  def initialize
    @hash = {}
  end

  def add(defs)
    defs.each do |word, definition|
      @hash[word] = definition
    end
  end
    def entries
     @hash
  end

  def keywords
     @hash.keys
  end
end
  def hello_world
    init
    return "lol"
  end
 

def init
    @data1 = Dictionary.new
    @data2 = Dictionary.new
    @data3 = Dictionary.new
    @data4 = Dictionary.new
    @data5 = Dictionary.new
    @data6 = Dictionary.new
    @data_brasil = Dictionary.new

  Dir["../persefoni/data/F*.csv"].select { |f| File.file? f
                        data = File.open(f).read
                        data = CSV.parse(data, headers: true)

                        @data1.add((f.gsub(/[^0-9]/, '')) => data[0]["Branca"].to_i/1000)
                        @data2.add((f.gsub(/[^0-9]/, '')) => data[6]["Branca"].to_i/1000)
                        @data3.add((f.gsub(/[^0-9]/, '')) => data[0]["Parda"].to_i/1000)
                        @data4.add((f.gsub(/[^0-9]/, '')) => data[6]["Parda"].to_i/1000)
  }

  Dir["../persefoni/data/M*.csv"].select { |f| File.file? f
                        data = File.open(f).read
                        data = CSV.parse(data, headers: true)

                        @data5.add((f.gsub(/[^0-9]/, '')) => data[2]["Parda"].to_i/1000)
                        @data6.add((f.gsub(/[^0-9]/, '')) => data[6]["Parda"].to_i/1000)
  }
end

def br_diabetes
  return @data1.entries
end

def br_arma
  return @data2.entries
end

def pa_diabetes
  return @data3.entries
end

def pa_arma
  return @data4.entries
end

def pa_infarto
  return @data5.entries
end

def pa_disparo
  return @data6.entries
end

def brasil

# pop brasileira
# 2010 -> 196.8
# 2011 -> 198.6
# 2012 -> 200.5
# 2013 -> 202.4
# 2014 -> 204.2
# 2015 -> 205.9
# 2016 -> 207.6

  @data_brasil.add("2010" => 1967963.0/100000)
  @data_brasil.add("2011" => 1986867.0/100000)
  @data_brasil.add("2012" => 2005610.0/100000)
  @data_brasil.add("2013" => 2024086.0/100000)
  @data_brasil.add("2014" => 2042131.0/100000)
  @data_brasil.add("2015" => 2059621.0/100000)
  @data_brasil.add("2016" => 2076529.0/100000)

  return @data_brasil.entries

end
end
