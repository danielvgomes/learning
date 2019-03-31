class Player

attr_accessor :x, :y

def initialize
    @image = Gosu::Image.new("media/object.bmp")
    @x = @y = @vel_x = @vel_y = @angle = 0.0
    @x2 = @y2 = 0.0
    @score = 0
    @milli = Gosu.milliseconds
  end

  def warp(x, y)
    @x, @y = x, y
  end
  
  def accelerate(angle, w)


	  p "HAHAHAHJAJS" + w.mouse_x.to_s
	#  p "angle = " + angle.to_s
	#  p "@angle = " + @angle.to_s
  if (@milli + 100) < Gosu.milliseconds

	  p "my current angle: " + @angle.to_s
	  p "my next angle: " + angle.to_s

if angle < @angle
	p "left, unless difference > 180"
  if (@angle - angle) > 180
	  p "TURNING RIGHT"
  else
	  p "TURNING LEFT"
  end
else
	p "right, unless difference > 180"
	if (angle - @angle) > 180
		p "TURNIN LEFTIE"
	else
		p "TURNIN RIGTHY"
	end
end

@milli = Gosu.milliseconds
  end




p "#####"

# p "i show you angels -> angle = " + angle.to_s + ", @angle = " + @angle.to_s + ", sub: " + (angle - @angle).to_s

# p "@a - a -> " + (@angle - angle).round(0).to_s
# p "a - @a -> " + (angle - @angle).round(0).to_s



	 @angle = angle

    ox = Gosu.offset_x(@angle, 1.1)
    oy = Gosu.offset_y(@angle, 1.1)

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
      
#    @vel_x += ox
#    @vel_y += oy
  end
  
  def move
    @x += @vel_x
    @y += @vel_y
    #@x %= 640
    #@y %= 480
    
    @vel_x *= 0.6
    @vel_y *= 0.6
  end

  def draw
    @image.draw_rot(@x, @y, 1, @angle)
  end
end
