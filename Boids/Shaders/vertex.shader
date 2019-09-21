#version 450 core

layout (location=0) in vec4 vPosition;

out vec4 color;

uniform mat4 MVP;

void main()
{
	gl_Position = MVP * vPosition;
	color = clamp(vPosition, 0.1f, 1.0f);
}