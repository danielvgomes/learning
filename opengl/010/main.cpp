#include <string.h>
#include <stdio.h>
#include <GL/glew.h>
#include <GLFW/glfw3.h>
#include <math.h>
#include <vector>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>
#include "win.h"
#include "mesh.h"
#include "shader.h"

win main_window;
std::vector<Mesh*> mesh_list;
std::vector<Shader> shader_list;

// vertex shader
static const char* v_shader = "shaders/shader.vert";


// fragment shader
static const char* f_shader = "shaders/shader.frag";

void create_objects()
{
	unsigned int indices[] = {
		0, 3, 1,
		1, 3, 2,
		2, 3, 0,
		0, 1, 2
	};

	GLfloat vertices[] = {
		-1.0f, -1.0f, 0.0f,
		0.0f, -1.0f, 1.0f,
		1.0f, -1.0f, 0.0f,
		0.0f, 1.0f, 0.0f
	};

	Mesh *obj1 = new Mesh();
	obj1->create_mesh(vertices, indices, 12, 12);
	mesh_list.push_back(obj1);
	Mesh *obj2 = new Mesh();
	obj2->create_mesh(vertices, indices, 12, 12);
	mesh_list.push_back(obj2);
}

void create_shaders()
{
	Shader *shader1 = new Shader();
	shader1->create_from_files(v_shader, f_shader);
	shader_list.push_back(*shader1);
}


int main()
{

	main_window = win(800, 600);
	main_window.initialize();
	create_objects();
	create_shaders();

	GLuint uniform_projection = 0, uniform_model = 0;

	glm::mat4 projection = glm::perspective(45.0f, (main_window.get_buffer_width()/main_window.get_buffer_height()), 0.1f, 100.0f);


	// loop until window closed
	while(!main_window.get_should_close())
	{

		// get + handle user input events
		glfwPollEvents();

		// clear window
		glClearColor(0.15f, 0.15f, 0.15f, 1.0f);
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

		shader_list[0].use_shader();
		uniform_model = shader_list[0].get_model_location();
		uniform_projection = shader_list[0].get_projection_location();

		glm::mat4 model;
		
		model = glm::translate(model, glm::vec3(0.0f, -2.5f, -1.5f));
		model = glm::scale(model, glm::vec3(0.3f, 0.3f, 1.0f));

		glUniformMatrix4fv(uniform_model, 1, GL_FALSE, glm::value_ptr(model));
		glUniformMatrix4fv(uniform_projection, 1, GL_FALSE, glm::value_ptr(projection));
		mesh_list[0]->render_mesh();		

		model = glm::mat4();
		model = glm::translate(model, glm::vec3(0.0f, 0.0f, -1.5f));
		model = glm::scale(model, glm::vec3(0.3f, 0.3f, 1.0f));
		glUniformMatrix4fv(uniform_model, 1, GL_FALSE, glm::value_ptr(model));
		mesh_list[1]->render_mesh();

		glUseProgram(0);

		main_window.swap_buffers();
	}

	return 0;
}
