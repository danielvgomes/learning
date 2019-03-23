# -*- coding: utf-8 -*-
require 'sdl2'

SDL2::MessageBox.show_simple_box(SDL2::MessageBox::ERROR, "Error",
                                 "This is the error message box", nil)

button = SDL2::MessageBox.show(flags: SDL2::MessageBox::WARNING,
                               window: nil,
                               title: "???????",
                               message: "??????????",
                               buttons: [ { # flags is ignored
                                           id: 0, 
                                           text: "???",
                                          }, 
                                         {flags: SDL2::MessageBox::BUTTON_RETURNKEY_DEFAULT,
                                          id: 1,
                                          text: "??",
                                         },
                                         {flags: SDL2::MessageBox::BUTTON_ESCAPEKEY_DEFAULT,
                                          id: 2,
                                          text: "?????",
                                         },
                                        ],
                               color_scheme: {
                                              bg: [255, 0, 0],
                                              text: [0, 255, 0],
                                              button_border: [255, 0, 0],
                                              button_bg: [0, 0, 255],
                                              button_selected: [255, 0, 0]
                                             }
                              )
p button
