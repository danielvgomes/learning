require 'gosu'
require_relative 'player'

class Tutorial < Gosu::Window

module ZOrder
  BACKGROUND, STARS, PLAYER, UI = *0..3
end

  def initialize
    super 640, 480
    self.caption = "Tutorial Game"
    @cursor = Gosu::Image.new('media/cursor.png')
    @font = Gosu::Font.new(20)
    @background_image = Gosu::Image.new("media/bg.bmp", :tileable => true)

    @player = Player.new self
    @player.warp(320, 240)
  end

  def update

    @player.move
    
    if Gosu.button_down? Gosu::MS_LEFT
      @player.accelerate
    end
  end

  def draw
    @cursor.draw mouse_x, mouse_y, 2, 0.5, 0.5
    @player.draw
    @background_image.draw(0, 0, ZOrder::BACKGROUND)
    @background_image.draw(400, 0, ZOrder::BACKGROUND)
    @font.draw("FPS: #{Gosu.fps}", 10, 10, ZOrder::UI, 1.0, 1.0, Gosu::Color::YELLOW)
    @font.draw("Mouse: #{mouse_x}, #{mouse_y}", 10, 30, ZOrder::UI, 1.0, 1.0, Gosu::Color::YELLOW)
    @font.draw("Wilber: #{'%.1f' % @player.x}, #{'%.1f' % @player.y}", 10, 50, ZOrder::UI, 1.0, 1.0, Gosu::Color::YELLOW)
    #@font.draw("Angle: #{'%.1f' % @angle}", 10, 70, ZOrder::UI, 1.0, 1.0, Gosu::Color::YELLOW)
    @font.draw_text("Milli: #{Gosu.milliseconds}", 10, 70, 4, 1.0, 1.0, Gosu::Color::YELLOW)

  end

  def button_down(id)
    if id == Gosu::KB_ESCAPE
      close
    else
      super
    end
  end
end

Tutorial.new.show
