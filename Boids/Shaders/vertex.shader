#version 450 core

layout (location=0) in vec4 vPosition;
layout (location=1) in mat4 modelMatrix;

out vec4 position;

uniform mat4 view;
uniform mat4 projection;

void main()
{
	mat4 mvp = projection * view * modelMatrix;
	gl_Position = mvp * vPosition;

	position = vPosition;
}