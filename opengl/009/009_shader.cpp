#include "009_shader.h"
#include <cstring>

Shader::Shader()
{
	shader_id = 0;
	uniform_model = 0;
	uniform_projection = 0;
}

void Shader::create_from_string(const char* vertex_code, const char* fragment_code)
{
	compile_shader(vertex_code, fragment_code);
}

void Shader::compile_shader(const char* vertex_code, const char* fragment_code)
{
	shader_id = glCreateProgram();
	if(!shader_id) {
		printf("Error creating shader program!\n");
		return;
	}

	add_shader(shader_id, vertex_code, GL_VERTEX_SHADER);
	add_shader(shader_id, fragment_code, GL_FRAGMENT_SHADER);

	GLint result = 0;
	GLchar elog[1024] = {0};

	glLinkProgram(shader_id);

	glGetProgramiv(shader_id, GL_LINK_STATUS, &result);
	if(!result)
	{
		glGetProgramInfoLog(shader_id, sizeof(elog), NULL, elog);
		printf("Error linking program: '%s'\n", elog);
		return;
	}

	glValidateProgram(shader_id);
	glGetProgramiv(shader_id, GL_VALIDATE_STATUS, &result);
	if(!result)
	{
		glGetProgramInfoLog(shader_id, sizeof(elog), NULL, elog);
		printf("Error validating program: '%s'\n", elog);
		return;
	}

	uniform_model = glGetUniformLocation(shader_id, "model");
	uniform_projection = glGetUniformLocation(shader_id, "projection");

}

GLuint Shader::get_projection_location()
{
	return uniform_projection;
}

GLuint Shader::get_model_location()
{
	return uniform_model;
}

void Shader::use_shader()
{
	glUseProgram(shader_id);
}

void Shader::clear_shader()
{
	if(shader_id != 0)
	{
		glDeleteProgram(shader_id);
		shader_id = 0;
	}

	uniform_model = 0;
	uniform_projection = 0;
}

void Shader::add_shader(GLuint the_program, const char* shader_code, GLenum shader_type)
{
GLuint the_shader = glCreateShader(shader_type);

	const GLchar* the_code[1];
	the_code[0] = shader_code;

	GLint code_length[1];
	code_length[0] = strlen(shader_code);

	glShaderSource(the_shader, 1, the_code, code_length);

	glCompileShader(the_shader);
	
	GLint result = 0;
	GLchar elog[1024] = {0};

	glGetShaderiv(the_shader, GL_COMPILE_STATUS, &result);
	if(!result)
	{
		glGetShaderInfoLog(the_shader, sizeof(elog), NULL, elog);
		printf("Error compiling the %d shader: '%s'\n", shader_type, elog);
		return;
	}

	glAttachShader(the_program, the_shader);

}

Shader::~Shader()
{
	clear_shader();
}
