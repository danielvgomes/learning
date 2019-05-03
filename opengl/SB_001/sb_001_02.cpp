#include <GL/glew.h>
#include <GLFW/glfw3.h>
#include <stdio.h>
#include <string.h>
#include <time.h>
#include <math.h>

int frames = 0;
int fps;
time_t timer;
long past_time;

int main() {

	past_time = time(&timer);

	if (!glfwInit()) {
		fprintf(stderr, "ERROR: could not start GLFW3\n");
		return 1;
	}

	GLFWwindow* window = glfwCreateWindow(640, 480, "Hello Book", NULL, NULL);

	if(!window) {
		fprintf(stderr, "ERROR: could not open window with GFLW3\n");
		glfwTerminate();
		return 1;
	}

	glfwMakeContextCurrent(window);
	glewExperimental = GL_TRUE;
	glewInit();

	const GLubyte* renderer = glGetString(GL_RENDERER);
	const GLubyte* version = glGetString(GL_VERSION);
	printf("Renderer: %s\n", renderer);
	printf("OpenGL version supported %s\n", version);

	while(!glfwWindowShouldClose(window)) {

		frames++;
		// printf("frames: %ld\n", frames);

		if (time(&timer) > past_time)
		{
			printf("%dFPS\n", frames);
			frames = 0;
			past_time = time(&timer);
		}

		// wipe the drawing surface clear
		GLfloat color[] = { (float)sin(frames) * 0.05f + 0.5f,
			            (float)cos(frames) * 0.05f + 0.5f,
				    0.0f, 1.0f };
		glClearBufferfv(GL_COLOR, 0, color);
		
		// glUseProgram(shader_programme);
		
		glfwPollEvents(); // se nao, nao da nem pra fechar a janela (input handler bitch), kkk

		// put the stuff we've been drawing onto the display
		glfwSwapBuffers(window); // se nao, nem aparece nada
	}

	glfwTerminate();
	return 0;
}
