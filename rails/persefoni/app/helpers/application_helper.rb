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

    def series_b
      lol = {"test" => []}
      lol["test"].push({"Ano" => "12", "moscas" => "1"})
      lol["test"].push({"Ano" => "15", "moscas" => "13"})
      
      lol2 = []
      lol2.push({"2001" => 1})
      lol2.push({"2002" => 7})

      za = {"test" => []}
      za["test"].push({"lol" => 23})
      za["test"].push({"lol1" => 223})

      ba = []
      ba.push("2001" => 1)
      ba.push("2007" => 15)

      be = { "2001" => []}
      be["2001"].push("test" => 14)
      be["2001"].push("gwlo" => 15)
      be["2001"].push("fef" => 17)

      car = {"2001" => 13, "2002" => 14}
      car["2005"] = 23
      return car
  

    end
end
