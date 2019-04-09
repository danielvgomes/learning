class Player

attr_accessor :x, :y

def initialize(window)
	@image = Gosu::Image.new("app/media/object.bmp")
	@x = @y = @vel_x = @vel_y = @angle = 0.0
	@x2 = @y2 = 0.0
	@score = 0
	@milli = Gosu.milliseconds
	@w = window
	@diff = 0
	@nextX = @nextY = 0
	@ox = @oy = 0
	@mouse_angle = 0
end


def warp(x, y)
	@x, @y = x, y
end

def set_x_y(x, y)
	@mouse_angle = Gosu.angle(@x, @y, x, y)
	@diff = Gosu.angle_diff(@angle, @mouse_angle)
end



def turn
	# p @diff

	if @diff.abs < 50
		@angle = @mouse_angle
	end



	if @diff < 0

		@angle += (@diff/20).round(0).abs
	end

	if @diff > 0
		@angle -= (@diff/20).round(0).abs
	end


	@angle %= 360
end

def accelerate
	@ox = Gosu.offset_x(@angle, 1.1)
	@oy = Gosu.offset_y(@angle, 1.1)
	@milli = Gosu.milliseconds
	@vel_x += @ox
	@vel_y += @oy
	@x += @vel_x
	@y += @vel_y
	#@x %= 640
	#@y %= 480
	@vel_x = 0
	@vel_y = 0
end
  
def move # ou gira ou anda

	if @diff.round(0).abs > 0
		turn
	end

	if @diff == 0
		accelerate
	end

end

def draw
	@image.draw_rot(@x, @y, 1, @angle)
end
end
