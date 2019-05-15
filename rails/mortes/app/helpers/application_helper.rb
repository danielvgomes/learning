module ApplicationHelper
 
  require 'csv'
  require 'json'
 
    def hello_world
     "Hello, World"
  end
 
    def series_a
      # data = File.open('../mortes/data/FEM2010.csv').read
      # CSV.parse(data).to_json
    end
 
    def fem_branca_e14
      {
        "2010": 13942,
        "2011": 14133,
        "2012": 13826,
        "2013": 13804,
        "2014": 13309,
        "2015": 13371,
        "2016": 13514
      }
    end
end
