#include <GL/glew.h>
#include <GLFW/glfw3.h>
#include <stdio.h>

int main() {
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
		// wipe the drawing surface clear
		GLfloat red[] = { 1.0f, 0.0f, 0.0f, 1.0f };
		glClearBufferfv(GL_COLOR, 0, red);
		
		// glUseProgram(shader_programme);
		
		glfwPollEvents(); // se nao, nao da nem pra fechar a janela (input handler bitch), kkk

		// put the stuff we've been drawing onto the display
		glfwSwapBuffers(window); // se nao, nem aparece nada
	}

	glfwTerminate();
	return 0;
}
