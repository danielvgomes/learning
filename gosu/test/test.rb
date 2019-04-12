#require 'minitest/autorun'
require 'test/unit'
require 'gosu'

class TestFoo < Test::Unit::TestCase
#class TestFoo < Test::Minitest

	def test_is_cray
		assert_equal 1, 2/2
	end

	def test_doesnt_make_sense
		assert 4 > 2
	end

	def test_gosu
		assert_equal Gosu.angle_diff(0, 360), 0.0
	end

	def test_gosu2
		assert_equal Gosu.angle_diff(360, 0), 0.0
	end



end
