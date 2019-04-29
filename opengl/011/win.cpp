#include "win.h"

win::win()
{
	width = 800;
	height = 600;

	for (size_t i = 0; i < 1024; i++)
	{
		keys[i] = 0;

	}
}

win::win(GLint window_width, GLint window_height)
{
	width = window_width;
	height = window_height;

	for (size_t i = 0; i < 1024; i++)
	{
		keys[i] = 0;

	}
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

	// handle key + mouse input
	create_callbacks();
	glfwSetInputMode(main_window, GLFW_CURSOR, GLFW_CURSOR_DISABLED);

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

	glfwSetWindowUserPointer(main_window, this);

	return 0;
}

void win::create_callbacks()
{
	glfwSetKeyCallback(main_window, handle_keys);
	glfwSetCursorPosCallback(main_window, handle_mouse);
}

GLfloat win::get_x_change()
{
	GLfloat the_change = x_change;
	x_change = 0.0f;
	return the_change;
}

GLfloat win::get_y_change()
{
	GLfloat the_change = y_change;
	y_change = 0.0f;
	return the_change;
}



void win::handle_keys(GLFWwindow* window, int key, int code, int action, int mode)
{
	win* the_window = static_cast<win*>(glfwGetWindowUserPointer(window));

	if(key == GLFW_KEY_ESCAPE && action == GLFW_PRESS)
	{
		glfwSetWindowShouldClose(window, GL_TRUE);
	}

	if(key >= 0 && key < 1024)
	{
		if(action == GLFW_PRESS)
		{
			the_window->keys[key] = true;
			printf("Pressed: %d\n", key);
		}
		else if (action == GLFW_RELEASE)
		{
			the_window->keys[key] = false;
			printf("Released: %d\n", key);
		}
	}



}

void win::handle_mouse(GLFWwindow* window, double x_pos, double y_pos)
	{
		win* the_window = static_cast<win*>(glfwGetWindowUserPointer(window));
		if (the_window->mouse_first_moved)
		{
			the_window->last_x = x_pos;
			the_window->last_y = y_pos;
			the_window->mouse_first_moved = false;
		}

		the_window->x_change = x_pos - the_window->last_x;
		the_window->y_change = the_window->last_y - y_pos;

		the_window->last_x = x_pos;
		the_window->last_y = y_pos;

		printf("x:%f, y:%f\n", the_window->x_change, the_window->y_change);
	}



win::~win()
{
		glfwDestroyWindow(main_window);
		glfwTerminate();
}
