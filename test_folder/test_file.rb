require "benchmark"

n = 100000
Benchmark.bmbm do |x|
  x.report("<<") do
    foo = ""
    n.times do
      foo << 'foobar'
    end
  end

  x.report("+=") do
    foo = ""
    n.times do
      foo += 'foobar'
    end
  end
end
