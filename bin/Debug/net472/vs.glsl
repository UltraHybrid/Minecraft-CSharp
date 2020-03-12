#version 410 core

out vec4 vs_color;

void main(void)
{
 gl_Position = vec4(-0.25, -0.25, 0.5, 0.5);
 vs_color = vec4(0.5, 0.5, 0.0, 1.0);
}