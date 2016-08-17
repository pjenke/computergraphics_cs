varying vec3 N; // Normal vector
varying vec3 p; // Surface point
varying vec4 color; // Surface color
uniform sampler2D texture; // Texture object
varying vec2 texture_coordinate; // Texture coordinate

uniform vec3 camera_position;
uniform int shaderMode; 
uniform vec3 lightPosition;

/**
 * Fragment shader: Phong shading with Phong lighting model.
 */
void main (void)
{
	// Init result
    gl_FragColor = vec4(0,0,0,color.a);
    float ambientFactor = 0.25;
    
	vec3 surfaceColor = color.xyz;
	if ( shaderMode == 0 ){
		// Phong shading
		surfaceColor = color.xyz;
	}
	if (shaderMode == 1 ){
		// Texture
		surfaceColor = texture2D(texture, texture_coordinate).xyz;
	}
	if (shaderMode == 2 ){
		// No lighting
    	gl_FragColor = color;
    	return;
	}
	if (shaderMode == 3 ){
		// Ambient only
    	gl_FragColor.xyz = color.xyz * ambientFactor;
    	return;
	}

    // Set lights
    int numberOfLights = 1;
    vec3 lightPositions[2];
    lightPositions[0] = lightPosition;
    lightPositions[1] = vec3(-2,1,0);

   	// Ambient color
    gl_FragColor.xyz += color.xyz * ambientFactor;
   
    // Add diffuse and specular for each light
    for ( int i = 0; i < numberOfLights; i++ ){

        // Point light, Spotlight
        vec3 L = normalize(lightPositions[i].xyz - p);
            
        // Diffuse
        vec3 diffuse = vec3(0,0,0);
        vec3 specular = vec3(0,0,0);
        float diffuseReflection = 0.9;
        float speculatReflection = 0.9;
        if ( dot( N, L ) > 0.0 ){
            diffuse = surfaceColor * clamp( dot( normalize(N), L ), 0.0, 1.0 ) * diffuseReflection;
            
            // Specular
            vec3 E = normalize( camera_position - p );
            vec3 R = normalize( reflect( L, N) );
            if ( dot(R,E) < 0.0 ){
            	specular = vec3(1,1,1) * pow(abs(dot(R,E)), 20.0);
            }
        }

        gl_FragColor.xyz += diffuse + specular;
    }

    gl_FragColor = clamp( gl_FragColor, 0.0, 1.0 );
}