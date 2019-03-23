#! /usr/bin/ruby -w

require 'sdl2'
require 'time'
require_relative 'point'

SDL.init SDL::INIT_VIDEO

def dro(s, o)
	s.fill_rect o.getX, o.getY, o.getSize, o.getSize, WHITEFOK
end






screen = SDL::set_video_mode 800, 600, 24, SDL::SWSURFACE
x = y = 0
r = 0
g = 0
b = 0



blah = Point.new(10, 400, 300)


BGCOLOR = screen.format.mapRGB 0, 0, 0
WHITEFOK = screen.format.mapRGB 255, 255, 255


linecolor = screen.format.mapRGB r, g, b

running = true
past_time = Time.now
counter = 0


while running
  while event = SDL::Event2.poll
    case event
      when SDL::Event2::Quit
        running = false
      when SDL::Event2::MouseMotion
        x = event.x
        y = event.y
	blah.destino(x, y, Time.now)
    end
  end
  screen.fill_rect 0, 0, 800, 600, BGCOLOR
  screen.draw_line x, 0, x, 599, linecolor
  screen.draw_line 0, y, 799, y, linecolor

  dro(screen, blah)
  
  blah.move(Time.now)

  screen.flip
  counter += 1
  r += 1
  g += 2
  b += 4
  if r >= 255
    r = 0
  end
  if g >= 255
    g = 0
  end
  if b >= 255
    b = 0
  end
linecolor = screen.format.mapRGB r, g, b
  if (Time.now - past_time) >= 1
    # p counter
    counter = 0
    past_time = Time.now
  end
end

