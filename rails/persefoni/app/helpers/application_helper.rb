module ApplicationHelper
 
  require 'csv'
  require 'json'
 
    def hello_world
     "Hello, World"
  end
 
    def series_a
#      data = File.open('../persefoni/data/FEM2010.csv').read
#      CSV.parse(data).to_json
    end
 
#    def series_b
#      {
#        "1993": 134,
#        "2005": "42",
#        "2006": "13"
#      }
#    end

#    def series_b
#      lol = {"test" => []}
#      lol["test"].push({"Ano" => "12", "moscas" => "1"})
#      lol["test"].push({"Ano" => "15", "moscas" => "13"})
#      
#      lol2 = []
#      lol2.push({"2001" => 1})
#      lol2.push({"2002" => 7})
#
#      za = {"test" => []}
#      za["test"].push({"lol" => 23})
#      za["test"].push({"lol1" => 223})
#
#      ba = []
#      ba.push("2001" => 1)
#      ba.push("2007" => 15)
#
#      be = { "2001" => []}
#      be["2001"].push("test" => 14)
#      be["2001"].push("gwlo" => 15)
#      be["2001"].push("fef" => 17)
#
#      # car = {"2001" => 13, "2002" => 14}
#      car = {}
##      car["2001"] = 20
#      car["2002"] = 120
#      car["2003"] = 90
#      car["2004"] = 80
#      car["2005"] = 112
#      car["2006"] = 415
#      car["2007"] = 300
#
#      return car
#    end

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

def series_b
  data1 = Dictionary.new
  Dir["../persefoni/data/*.csv"].select { |f| File.file? f
                        data = File.open(f).read
                        data = CSV.parse(data, headers: true)

                        data1.add((f.gsub(/[^0-9]/, '')) => data[0]["Branca"].to_i)
  }

  return data1.entries

end

    

end
