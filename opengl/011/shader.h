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
		void create_from_files(const char* vertex_location, const char* fragment_location);

		std::string read_file(const char* file_location);

		GLuint get_projection_location();
		GLuint get_model_location();
		GLuint get_view_location();

		void use_shader();
		void clear_shader();
		
		~Shader();

	private:
		GLuint shader_id, uniform_projection, uniform_model, uniform_view;

		void compile_shader(const char* vertex_code, const char* fragment_code);
		void add_shader(GLuint the_program, const char* shader_code, GLenum shader_type);

};
