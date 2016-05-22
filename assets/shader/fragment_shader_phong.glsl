varying vec3 N; // Normal vector
varying vec3 p; // Surface point
varying vec3 color; // Surface color
uniform vec3 camera_position; // Set in Java application
uniform sampler2D texture; // Texture object
varying vec2 texture_coordinate; // Texture coordinate
uniform vec3 useTexture; 

/**
 * Fragment shader: Phong shading with Phong lighting model.
 */
void main (void)
{
	vec3 surfaceColor = color;
	//vec3 surfaceColor = texture2D(texture, texture_coordinate).xyz;
	//if (useTexture[0] > 0.0 ){
	//	surfaceColor = texture2D(texture, texture_coordinate).xyz;
	//}

    // Set lights
    int numberOfLights = 2;
    vec3 lightPositions[2];
    lightPositions[0] = vec3(-2,5,3);
    lightPositions[1] = vec3(3,-2,-5);

    // Init result
    gl_FragColor = vec4(0,0,0,1);
   
   	// Ambient color
    gl_FragColor.xyz += vec3(0.2, 0.2, 0.2);
   
    // Add diffuse and specular for each light
    for ( int i = 0; i < numberOfLights; i++ ){

        // Point light, Spotlight
        vec3 L = normalize(lightPositions[i].xyz - p);
            
        // Diffuse
        vec3 diffuse = vec3(0,0,0);
        vec3 specular = vec3(0,0,0);
        if ( dot( N, L ) > 0.0 ){
            diffuse = surfaceColor * clamp( abs(dot( normalize(N), L )), 0.0, 1.0 );
            
            // Specular
            vec3 E = normalize( camera_position - p );
            vec3 R = normalize( reflect( L, N) );
            //specular = vec3(1,1,1) * pow(abs(dot(R,E)), 20.0);
        }

        gl_FragColor.xyz += diffuse + specular;
    }
    
    gl_FragColor = clamp( gl_FragColor, 0.0, 1.0 );
}