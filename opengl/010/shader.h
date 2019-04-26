

#include <stdio.h>
#include <string>
#include <iostream>
#include <fstream>

#include <GL/glew.h>

class Shader
{
	public:
		Shader();

		void create_from_string(const char* vertex_code, const char* fragment_code);

		GLuint get_projection_location();
		GLuint get_model_location();

		void use_shader();
		void clear_shader();
		
		~Shader();

	private:
		GLuint shader_id, uniform_projection, uniform_model;

		void compile_shader(const char* vertex_code, const char* fragment_code);
		void add_shader(GLuint the_program, const char* shader_code, GLenum shader_type);

};
