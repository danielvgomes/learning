class Player

attr_accessor :x, :y

def initialize
    @image = Gosu::Image.new("media/object.bmp")
    @x = @y = @vel_x = @vel_y = @angle = 0.0
    @x2 = @y2 = 0.0
    @score = 0
  end

  def warp(x, y)
    @x, @y = x, y
  end
  
  def turn_left
	  @angle -= 4.5
	  if @angle < 0
		  @angle = 360
	  end
  end
  
  def turn_right
	  @angle += 4.5
	  if @angle > 360
		  @angle = 0
	  end
  end
  
  def accelerate(angle)
	  p "angle = " + angle.to_s
	  p "@angle = " + @angle.to_s
      
	  # @angle = angle
    # p "angle received: " + angle.to_s + ", @angle: " + @angle.to_s


#    if (@angle > angle+15 || @angle < angle-15)
#      turn_right
#    else

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
	    
	    
#      z = Gosu.offset_x(@angle, 0.1)
#      @vel_x += z
#    else
#      @vel_x -= z
#    end
    
  
    
    
#      @vel_x += Gosu.offset_x(@angle, 0.1)
#      @vel_y += Gosu.offset_y(@angle, 0.1)
#    end
  end
  
  def move
    @x += @vel_x
    @y += @vel_y
    #@x %= 640
    #@y %= 480
    
    @vel_x *= 0.6
    @vel_y *= 0.6
    # p @x2.to_s + " " + @y2.to_s
  end

  def draw
    @image.draw_rot(@x, @y, 1, @angle)
  end
end
