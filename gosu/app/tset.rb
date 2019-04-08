require 'gosu'

class HUD
    attr_writer :pressed_once
    def initialize(x, y, draws, buttons_down, id)
        @x = x
        @y = y
        @draws = draws
        @buttons_down = buttons_down
        @id = id
        @font = nil
        @pressed_once = 0
    end
    
    def main
        if Gosu.button_down?(Gosu::KbE) && @font == nil && @pressed_once == 0
            @font = Gosu::Font.new(20)
            @pressed_once += 1
        elsif Gosu.button_down?(Gosu::KbE) && @font != true && @pressed_once == 0
            @font = nil
            @pressed_once += 1
        end
    end
    
    def write(var,var2,var3,var4,var5)
        if @font != nil
            @font.draw("X = #{var}, Y = #{var2}, Draws = #{var3}, Buttons down = #{var4}, id = #{var5}, FPS = #{Gosu.fps}",
            10, 10, 1, 1.0, 1.0, Gosu::Color::YELLOW)
        end
    end
end

class GameWindow < Gosu::Window
    def initialize(width = 1000, height = 1000, fullscreen = false)
        super
        self.caption = 'hello world'
        @x = @y = 180
        @buttons_down = 0
        @draws = 0
        @font = Gosu::Font.new(20)
        @ka = 0
        @my_hud = HUD.new(@x,@y,@draws,@buttons_down,@ka)
    end
    
    def update
        Thread.new{@my_hud.main}.join
        @x += 10 if button_down?(Gosu::KbD)
        @x -= 10 if button_down?(Gosu::KbA)
        @y += 10 if button_down?(Gosu::KbS)
        @y -= 10 if button_down?(Gosu::KbW)
    end
    
    def button_down(id)
        close if id == Gosu::KbEscape
        @buttons_down += 1
        @ka = id
    end
    
    def button_up(id)
        @my_hud.pressed_once = 0 if id == Gosu::KbE
        @buttons_down -= 1
    end
    
   # def needs_redraw?
   #     @draws == 0 || @buttons_down > 0
   # end
    
    def draw
        @draws += 1
        @my_hud.write(@x,@y,@draws,@buttons_down,@ka)
        @cha = Gosu::Image.from_text(self, mensage, Gosu.default_font_name, 30)
        @cha.draw(@x, @y, 0)
    end
    
    private
    
    def mensage
        "(#{@x}, #{@y}, #{@draws}, #{@buttons_down}, #{@ka})"
    end
end

window = GameWindow.new
window.show
