require 'csv'

data = File.open('FEM2010.csv').read
lol = CSV.parse(data, headers: true)



p lol[2]["Categoria CID-10"]

Dir["*.csv"].select { |f| File.file? f
  data = File.open(f).read
  data = CSV.parse(data, headers: true)
  p data
}


