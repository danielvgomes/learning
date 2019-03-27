
require 'gosu'
require_relative "player"

class Tutorial < Gosu::Window

module ZOrder
  BACKGROUND, STARS, PLAYER, UI = *0..3
end





  def initialize
    super 640, 480
    self.caption = "Tutorial Game"
    @font = Gosu::Font.new(20)

    @background_image = Gosu::Image.new("media/bg.bmp", :tileable => true)

    @player = Player.new
    @player.warp(320, 240)
  end

  def update
    if Gosu.button_down? Gosu::KB_LEFT or Gosu::button_down? Gosu::GP_LEFT
      @player.turn_left
    end
    if Gosu.button_down? Gosu::KB_RIGHT or Gosu::button_down? Gosu::GP_RIGHT
      @player.turn_right
    end
    if Gosu.button_down? Gosu::KB_UP or Gosu::button_down? Gosu::GP_BUTTON_0
      @player.accelerate
    end
    @player.move
  end

  def draw
    @player.draw
    @background_image.draw(0, 0, ZOrder::BACKGROUND)
    # @background_image.draw(0, 400, ZOrder::BACKGROUND)
    # @background_image.draw(400, 400, ZOrder::BACKGROUND)
    @background_image.draw(400, 0, ZOrder::BACKGROUND)
    @font.draw("FPS: #{Gosu.fps}", 10, 10, ZOrder::UI, 1.0, 1.0, Gosu::Color::YELLOW)
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
