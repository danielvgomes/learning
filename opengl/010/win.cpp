#include "win.h"

win::win()
{
	width = 800;
	height = 600;
}

win::win(GLint window_width, GLint window_height)
{
	width = window_width;
	height = window_height;
}

int win::initialize()
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

	main_window = glfwCreateWindow(width, height, "opengl test", NULL, NULL);

	if(!main_window)
	{
		printf("GLFW window creation failed!");
		glfwTerminate();
		return 1;
	}

	// get buffer size information
	glfwGetFramebufferSize(main_window, &buffer_width, &buffer_height);


	// set context for GLEW to use
	glfwMakeContextCurrent(main_window);

	// allow modern extension features
	glewExperimental = GL_TRUE;

	// GLenum error = glewInit();
	if(glewInit() != GLEW_OK)
	{
		printf("GLEW initialization failed!");
		glfwDestroyWindow(main_window);
		glfwTerminate();
		return 1;
	}

	glEnable(GL_DEPTH_TEST);

	// setup viewport size
	glViewport(0, 0, buffer_width, buffer_height);

	return 0;
}


win::~win()
{
		glfwDestroyWindow(main_window);
		glfwTerminate();
}
