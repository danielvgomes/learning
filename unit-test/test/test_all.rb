require 'test/unit'
require 'app/double.rb'

class TestAll < Test::Unit::TestCase
	def test_double
		d = Double.new
		assert_equal 18, d.double(9)
		assert_equal 200, d.double(100)
		assert_equal 122, d.double(61)
		assert_equal 300, d.double(150)
	end
end
