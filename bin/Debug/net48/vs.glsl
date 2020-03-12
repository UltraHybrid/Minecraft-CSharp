#version 450 core

out vec4 vs_color;

void main(void)
{
 gl_Position = vec4(0.25, -0.25, 0.5, 1.0);
 _color = vec4(1.0, 1.0, 0.0, 1.0)
}