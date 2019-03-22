class Point  
  def initialize(size, x, y)  
    # Instance variables  
    @size = size
    @positionX = x
    @positionY = y
    @move_request_time = 0
  
  end  
  def move(move_time)
	  p "move called - time: " + move_time.to_s
	  if (move_time - @move_request_time).to_f > 0.05
	    @positionX += 1
	    @move_request_time = move_time
    end

  end

  def destino(dest_x, dest_y, t)
	  p "dest x " + dest_x.to_s
	  p "dest y " + dest_y.to_s
	  p "t " + t.to_s
	  @move_request_time = t
  end

  def getX
	  @positionX
  end

  def getY
	  @positionY
  end

  def getSize
	  @size
  end
end
