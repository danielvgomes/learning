class Player
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
  end
  
  def turn_right
    @angle += 4.5
  end
  
  def accelerate
    @vel_x += Gosu.offset_x(@angle, 0.5)
    @vel_y += Gosu.offset_y(@angle, 0.5)
  end
  
  def move
    @x2 += @vel_x
    @y2 += @vel_y
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
