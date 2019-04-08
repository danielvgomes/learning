require 'gosu'

class GameWindow < Gosu::Window
    def initialize(width = 1000, height = 1000, fullscreen = false)
        super
        self.caption = 'hello world'
        @x = @y = 180
        @buttons_down = 0
        @draws = 0
        @font = Gosu::Font.new(20)
        @ka = 0
        @hud = nil
    end
    
    def update
        #@my_hud.main
        #Thread.new{@my_hud.main}.join
        @x += 10 if button_down?(Gosu::KbD)
        @x -= 10 if button_down?(Gosu::KbA)
        @y += 10 if button_down?(Gosu::KbS)
        @y -= 10 if button_down?(Gosu::KbW)
    end
    
    def button_down(id)
        close if id == Gosu::KbEscape
        @hud ||= Gosu::Font.new(20) if id == Gosu::KbE
        @buttons_down += 1
        @ka = id
    end
    
    def button_up(id)
        @hud = nil if id == Gosu::KbE
        @buttons_down -= 1
    end
    
    # def needs_redraw?
    #     @draws == 0 || @buttons_down > 0
    # end
    
    def draw
        @draws += 1
        if @hud != nil
            @hud.draw("X = #{@x}, Y = #{@y}, Draws = #{@draws}, Buttons down = #{@buttons_down}, id = #{@ka}, FPS = #{Gosu.fps}",
            10, 10, 1, 1.0, 1.0, Gosu::Color::YELLOW)
        end
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
