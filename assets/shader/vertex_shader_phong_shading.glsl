varying vec3 N; // Normal vector
varying vec3 p; // Surface point
uniform vec3 camera_position; // Set in Java application

/**
 * Vertex shader: Phong lighting model, Phong shading.
 */
void main(void)
{
    p  = vec3(gl_Vertex);
    N = gl_Normal;
    gl_Position = gl_ModelViewProjectionMatrix * gl_Vertex;
}