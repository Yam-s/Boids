#version 450 core

layout (location=0) in vec4 vPosition;
layout (location=1) in mat4 modelMatrix;

out vec4 color;

uniform mat4 view;
uniform mat4 projection;

void main()
{
	mat4 mvp = projection * view * modelMatrix;
	gl_Position = mvp * vPosition;
	float x = mix(0, 1, vPosition.x);
	float y = mix(0, 1, vPosition.y);
	float z = mix(0, 1, vPosition.z);
	color = clamp(vec4(x,y,z,1.0f), 0.2f, 0.8f);
}