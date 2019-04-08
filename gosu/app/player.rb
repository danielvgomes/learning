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
#	@mouse_angle = Gosu.angle(@x, @y, @w.mouse_x, @w.mouse_y)
	@nextX = @w.mouse_x
	@nextY = @w.mouse_y

	if (@milli + 100) < Gosu.milliseconds
#		p "my current angle: " + @angle.to_s
#		p "my next angle: " + @mouse_angle.to_s
		@diff = Gosu.angle_diff(@mouse_angle, @angle)

		@milli = Gosu.milliseconds
	end

	@ox = Gosu.offset_x(@angle, 1.1)
	@oy = Gosu.offset_y(@angle, 1.1)

	if (@angle >= 0 && @angle < 90) || (@angle > 270 && @angle <= 360)
		#     p "going up"
	else
		#     p "going down"
	end

	if @angle > 0 && @angle < 180
		#     p "going right"
	else
		#     p "going left"
	end
end
  
def move
	if @diff.round(0).abs > 0
		turn
	else
		if (@x.round(0) != @nextX.round(0) && (@y.round(0) != @nextY.round(0)))
			@vel_x += @ox
			@vel_y += @oy
		end

		@x += @vel_x
		@y += @vel_y
		#@x %= 640
		#@y %= 480

		@vel_x = 0
		@vel_y = 0
	end
end

def draw
	@image.draw_rot(@x, @y, 1, @angle)
end
end
