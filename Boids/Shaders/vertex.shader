#version 450 core

layout (location=0) in vec4 vPosition;

out vec4 color;

uniform mat4 MVP;

void main()
{
	gl_Position = MVP * vPosition;

	vec4 testtwo = vPosition * MVP;
	float x = mix(0, 1, testtwo.x);
	float y = mix(0, 1, testtwo.y);
	float z = mix(0, 1, testtwo.z);
	color = clamp(vec4(x,y,z,1.0f), 0.2f, 0.8f);
}