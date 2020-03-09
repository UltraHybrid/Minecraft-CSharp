#version 330
 
in vec3 vPosition;
in  vec3 vColor;
out vec4 color;
uniform mat4 model;
uniform mat4 view;
uniform mat4 projection;
 
void
main()
{
    gl_Position = projection * view * model * vec4(vPosition, 1.0);
 
    color = vec4( vColor, 1.0);
}