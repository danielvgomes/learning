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

    # p "angle received: " + angle.to_s + ", @angle: " + @angle.to_s


    if (@angle > angle+15 || @angle < angle-15)
      turn_right
    else
      @vel_x += Gosu.offset_x(@angle, 0.5)
      @vel_y += Gosu.offset_y(@angle, 0.5)
    end
  end
  
  def move
    @x += @vel_x
    @y += @vel_y
    #@x %= 640
    #@y %= 480
    
    @vel_x *= 0.95
    @vel_y *= 0.95
    # p @x2.to_s + " " + @y2.to_s
  end

  def draw
    @image.draw_rot(@x, @y, 1, @angle)
  end
end
