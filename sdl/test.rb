#! /usr/bin/ruby -w

require 'sdl'
require 'time'

SDL.init SDL::INIT_VIDEO

def dro(s)
	a = b = 200
	s.fill_rect a, b, 10, 10, WHITEFOK
end


screen = SDL::set_video_mode 800, 600, 24, SDL::SWSURFACE
x = y = 0
r = 0
g = 0
b = 0


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
    end
  end
  screen.fill_rect 0, 0, 640, 480, BGCOLOR
  screen.draw_line x, 0, x, 479, linecolor
  screen.draw_line 0, y, 639, y, linecolor
  dro(screen)
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
    p counter
    counter = 0
    past_time = Time.now
  end
end

