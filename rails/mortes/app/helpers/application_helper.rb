module ApplicationHelper

  require 'csv'
  require 'json'

    def hello_world
     "Hello, World"
  end


    def series_a
      data = File.open('../mortes/data/FEM2010.csv').read
      CSV.parse(data).to_json
    end

    def series_b
      {
        "1993": 134,
        "2005": "42",
        "2006": "13"
      }
    end



end
