require 'csv'

data = File.open('FEM2010.csv').read
lol = CSV.parse(data, headers: true)

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

p lol[0]["Categoria CID-10"] # diabetes
p lol[1]["Categoria CID-10"] # parkinson
p lol[2]["Categoria CID-10"] # infarto
p lol[3]["Categoria CID-10"] # estrangulamento
p lol[4]["Categoria CID-10"] # arma de fogo
p lol[5]["Categoria CID-10"] # calibre maior
p lol[6]["Categoria CID-10"] # disparo
p lol[7]["Categoria CID-10"] # chama
p lol[8]["Categoria CID-10"] # cortante
p lol[9]["Categoria CID-10"] # contundente
p lol[10]["Categoria CID-10"] # forca corporal
p lol[11]["Categoria CID-10"] # nao especificado
p lol[12]["Categoria CID-10"] # intervencao legal
#p lol[13]["Categoria CID-10"] # Total
#p lol[14]["Categoria CID-10"]
#p lol[15]["Categoria CID-10"]

p "##"

p lol[0]["Parda"]
# p lol[1]["Parda"]
p lol[2]["Parda"]
#p lol[3]["Parda"]
#p lol[4]["Parda"]
#p lol[5]["Parda"]
p lol[6]["Parda"]
#p lol[7]["Parda"]
p lol[8]["Parda"]
#p lol[9]["Parda"]
#p lol[10]["Parda"]
#p lol[11]["Parda"]
#p lol[12]["Parda"]



data1 = Dictionary.new

Dir["*.csv"].select { |f| File.file? f
  data = File.open(f).read
  data = CSV.parse(data, headers: true)

#  data1 = 
# data1 = [f.gsub(/[^0-9]/, '')] = data[0]["Branca"]
  data1.add((f.gsub(/[^0-9]/, '')) => data[0]["Branca"].to_i)


}

p "eita porra duvido"
p "#################"
p data1.entries


car = {}

car["2001"] = 20
car["2002"] = 199

p car
