#include <string.h>
#include <stdio.h>
#include <GL/glew.h>
#include <GLFW/glfw3.h>
#include <math.h>
#include <vector>
#include <glm/glm.hpp>
#include <glm/gtc/matrix_transform.hpp>
#include <glm/gtc/type_ptr.hpp>
#include "mesh.h"
#include "shader.h"

// window dimensions
const GLint width = 800, height = 600;

std::vector<Mesh*> mesh_list;
std::vector<Shader> shader_list;

bool direction = true;
float trioffset = 0.0f;
float trimaxoffset = 0.7f;
float triincrement = 0.005f;
const float to_radians = 3.14159265f / 180.0f;

float curangle = 0.0f;

float cursize = 0.4f;
float maxsize = 0.8f;
float minsize = 0.1f;

bool size_direction = true;

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
	// initialize GLFW
	if (!glfwInit())
	{
		printf("GLFW initialization failed!");
		glfwTerminate();
		return 1;
	}

	// setup GLFW window propeties
	glfwWindowHint(GLFW_CONTEXT_VERSION_MAJOR, 3);
	glfwWindowHint(GLFW_CONTEXT_VERSION_MINOR, 2);
	glfwWindowHint(GLFW_OPENGL_PROFILE, GLFW_OPENGL_CORE_PROFILE);
	glfwWindowHint(GLFW_OPENGL_FORWARD_COMPAT, GL_TRUE);

	GLFWwindow *mainWindow = glfwCreateWindow(width, height, "Test Window", NULL, NULL);

	if(!mainWindow)
	{
		printf("GLFW window creation failed!");
		glfwTerminate();
		return 1;
	}

	// get buffer size information
	int buffer_width, buffer_height;
	glfwGetFramebufferSize(mainWindow, &buffer_width, &buffer_height);


	// set context for GLEW to use
	glfwMakeContextCurrent(mainWindow);

	// allow modern extension features
	glewExperimental = GL_TRUE;

	// GLenum error = glewInit();
	if(glewInit() != GLEW_OK)
	{
		printf("GLEW initialization failed!");
		glfwDestroyWindow(mainWindow);
		glfwTerminate();
		return 1;
	}

	glEnable(GL_DEPTH_TEST);

	// setup viewport size
	glViewport(0, 0, buffer_width, buffer_height);


	create_objects();
	create_shaders();

	GLuint uniform_projection = 0, uniform_model = 0;

	glm::mat4 projection = glm::perspective(45.0f, (GLfloat)buffer_width/(GLfloat)buffer_height, 0.1f, 100.0f);


	// loop until window closed
	while(!glfwWindowShouldClose(mainWindow))
	{

		// get + handle user input events
		glfwPollEvents();

		if (direction)
		{
			trioffset += triincrement;
		}
		else
		{
			trioffset -= triincrement;
		}

		if (abs(trioffset) >= trimaxoffset)
		{
			direction = !direction;
		}

		curangle += 1.0f;
		if (curangle >= 360)
		{
			curangle = 0.00f;
		}

		if (size_direction)
		{
			cursize += 0.01f;
		}
		else
		{
			cursize -= 0.01f;
		}

		if (cursize >= maxsize || cursize <= minsize)
		{
			size_direction = !size_direction;
		}

		// clear window
		glClearColor(0.15f, 0.15f, 0.15f, 1.0f);
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

		shader_list[0].use_shader();
		uniform_model = shader_list[0].get_model_location();
		uniform_projection = shader_list[0].get_projection_location();

		glm::mat4 model;
		
		model = glm::translate(model, glm::vec3(trioffset, 0.0f, -1.5f));
		model = glm::scale(model, glm::vec3(0.3f, 0.3f, 1.0f));

		glUniformMatrix4fv(uniform_model, 1, GL_FALSE, glm::value_ptr(model));
		glUniformMatrix4fv(uniform_projection, 1, GL_FALSE, glm::value_ptr(projection));
		mesh_list[0]->render_mesh();		

		model = glm::mat4();
		model = glm::translate(model, glm::vec3(-trioffset, 0.0f, -1.5f));
		model = glm::scale(model, glm::vec3(0.3f, 0.3f, 1.0f));
		glUniformMatrix4fv(uniform_model, 1, GL_FALSE, glm::value_ptr(model));
		mesh_list[1]->render_mesh();

		glUseProgram(0);

		glfwSwapBuffers(mainWindow);
	}

	return 0;
}
