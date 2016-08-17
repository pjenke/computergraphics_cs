varying vec3 N; // Normal vector
varying vec3 p; // Surface point
varying vec4 color; // Surface color
varying vec2 texture_coordinate; // Texture coordinate

attribute vec3 inVertex;
attribute vec3 inNormal;
attribute vec4 inColor;

uniform vec3 camera_position;
uniform int shaderMode; 
uniform mat4 modelMatrix; 
uniform mat4 viewMatrix; 

/**
 * Vertex shader: Phong lighting model, Phong shading.
 */
void main(void)
{
	vec4 p4 = modelMatrix * vec4(inVertex.x, inVertex.y, inVertex.z, 1) ; 
	//vec4 p4 = modelMatrix * gl_Vertex; 
	p4 = p4 / p4.w;
    p  = p4.xyz;
    N = (modelMatrix * vec4( inNormal, 0.0)).xyz;
    //N = (modelMatrix * vec4( gl_Normal, 0.0)).xyz;
    color = inColor;
    texture_coordinate = vec2(gl_MultiTexCoord0);
    gl_Position = gl_ProjectionMatrix * ( viewMatrix * (modelMatrix * vec4(inVertex.x, inVertex.y, inVertex.z, 1)));
    //gl_Position = gl_ProjectionMatrix * ( viewMatrix * (modelMatrix * gl_Vertex));
}