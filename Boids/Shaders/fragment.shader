#version 450 core

in vec4 position;

out vec4 fColor;

void main()
{
	float x = mix(0.5f, 1.0f, position.x);
	float y = mix(0.5f, 1.0f, position.y);
	float z = position.z;
	fColor = clamp(vec4(x,y,z,1.0f), 0.2f, 0.8f);
}