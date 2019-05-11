#include <GL/glew.h>
#include <GLFW/glfw3.h>
#include <stdio.h>
#include <string.h>
#include <time.h>
#include <math.h>
#define PI 3.14159265

int frames = 0;
int counter = 0;
int fps;
time_t timer;
long past_time;

GLuint rendering_program;
GLuint program;
GLuint vertex_array_object;
GLuint vertex_shader;
GLuint fragment_shader;

void compile_shaders(void);
GLenum err;
void startup(void);
void shutdown(void);

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
	compile_shaders();

	const GLubyte* renderer = glGetString(GL_RENDERER);
	const GLubyte* version = glGetString(GL_VERSION);
	printf("Renderer: %s\n", renderer);
	printf("OpenGL version supported %s\n", version);

	while(!glfwWindowShouldClose(window)) {

		frames++;
		counter++;

		if (time(&timer) > past_time)
		{
			printf("%dFPS\n", frames);
			frames = 0;
			past_time = time(&timer);
		}

		// wipe the drawing surface clear
		GLfloat color[] = { (float)sin(counter*PI/180) * 0.5f + 0.5f,
			            (float)cos(counter*PI/180) * 0.5f + 0.5f,
				    0.0f, 1.0f };
		glClearBufferfv(GL_COLOR, 0, color);

		// use program
		glUseProgram(program);

		// draw one thing
		glDrawArrays(GL_POINTS, 0, 1);
		glPointSize(abs(100*(float)sin(counter*PI/180)));
		
		glfwPollEvents(); // se nao, nao da nem pra fechar a janela (input handler bitch), kkk

		// put the stuff we've been drawing onto the display
		glfwSwapBuffers(window); // se nao, nem aparece nada

		while((err = glGetError()) != GL_NO_ERROR)
		{
			if (err == GL_INVALID_ENUM) { printf("INVALID_ENUM\n"); }
			if (err == GL_INVALID_VALUE) { printf("INVALID_VALUE\n"); }
			if (err == GL_INVALID_OPERATION) { printf("INVALID_OPERATION\n"); }
			if (err == GL_INVALID_FRAMEBUFFER_OPERATION) { printf("INVALID_FRAMEBUFFER_OPERATION\n"); }
			if (err == GL_OUT_OF_MEMORY) { printf("GL_OUT_OF_MEMORY\n"); }
			if (err == GL_STACK_UNDERFLOW) { printf("GL_STACK_UNDERFLOW\n"); }
			if (err == GL_STACK_OVERFLOW) { printf("GL_STACK_OVERFLOW\n"); }
		}
	}

	glfwTerminate();
	return 0;
}

void compile_shaders(void)
{
	printf("compile shaders function\n");

	// source code for vertex shader
	static const char * vertex_shader_source = "						\n\
		#version 430 core								\n\
												\n\
		void main()									\n\
		{										\n\
			gl_Position = vec4(0.0, 0.0, 0.5, 1.0);					\n\
		}";

	// source code for fragment shader
	static const char * fragment_shader_source = "						\n\
		#version 430 core								\n\
												\n\
		out vec4 color;									\n\
												\n\
		void main()									\n\
		{										\n\
			color = vec4(0.0, 0.8, 1.0, 1.0);					\n\
		}";

	const GLchar* vs_code[1];
	const GLchar* fs_code[1];

	vs_code[0] = vertex_shader_source;
	fs_code[0] = fragment_shader_source;

	// create and compile vertex shader
	vertex_shader = glCreateShader(GL_VERTEX_SHADER);
	glShaderSource(vertex_shader, 1, vs_code, NULL);
	glCompileShader(vertex_shader);

	// create and compile fragment shader
	fragment_shader = glCreateShader(GL_FRAGMENT_SHADER);
	glShaderSource(fragment_shader, 1, fs_code, NULL);
	glCompileShader(fragment_shader);

	// create program, attach shaders to it, and link it
	program = glCreateProgram();

	// tratamento de erro do outro curso
	if (!program)
	{
		printf(" deu ruim: shader\n");
	}

	glAttachShader(program, vertex_shader);
	glAttachShader(program, fragment_shader);
	glLinkProgram(program);

	GLint is_linked;
	glGetProgramiv(program, GL_LINK_STATUS, (GLint *)&is_linked);
	if (is_linked == GL_FALSE)
	{
		printf("deu ruim: linkar\n");

		GLint max_length;
		glGetProgramiv(program, GL_INFO_LOG_LENGTH, &max_length);
		if (max_length > 0)
		{
			char *p_link_info_log = new char[max_length];
			glGetProgramInfoLog(program, max_length, &max_length, p_link_info_log);
			printf("deu ruim, log: %s\n", p_link_info_log);
			delete [] p_link_info_log;
		}
	}

	// delete the shaders as the program has them now
	glDeleteShader(vertex_shader);
	glDeleteShader(fragment_shader);

}

void startup()
{
	compile_shaders();
	glGenVertexArrays(1, &vertex_array_object);
	glBindVertexArray(vertex_array_object);
}

void shutdown()
{
	glDeleteVertexArrays(1, &vertex_array_object);
	glDeleteProgram(rendering_program);
	glDeleteVertexArrays(1, &vertex_array_object);
}