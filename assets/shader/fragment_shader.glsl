varying vec3 N; // Normal vector
varying vec3 p; // Surface point
varying vec4 color; // Surface color
varying vec2 texture_coordinate; // Texture coordinate

uniform sampler2D texture; // Texture object
uniform int shaderMode; 
uniform vec3 camera_position;
uniform vec3 lightPosition;

/**
 * Fragment shader: Phong shading with Phong lighting model.
 */
void main (void)
{
	float ambientFactor = 0.25;
    float diffuseReflection = 0.9;
	float specularReflection = 0.3;

	// Init result
    gl_FragColor = vec4(0,0,0,color.a);
    
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
	if (shaderMode == 4 ){
		// Texture w/o lighting
    	gl_FragColor.xyz = texture2D(texture, texture_coordinate).xyz;
    	gl_FragColor.a = 1.0;
    	return;
	}
	if (shaderMode == 5 ){
		// Fullscreen texture: Texture w/o lighting
    	gl_FragColor.xyz = texture2D(texture, texture_coordinate).xyz;
    	gl_FragColor.a = 1.0;
    	return;
	}

   	// Ambient color
    gl_FragColor.xyz = color.xyz * ambientFactor;
   
    // Light direction
	vec3 L = normalize(lightPosition-p);
            
	// Phong
	if ( dot( N, L ) > 0.0 ){
		gl_FragColor.xyz += surfaceColor * dot( N, L ) * diffuseReflection;
            
		// Specular
		vec3 E = normalize(-p );
		vec3 R = normalize( reflect( L, N) );
		if ( dot(R,E) < 0.0 ){
			gl_FragColor.xyz += vec3(1,1,1) * pow(abs(dot(R,E)), 20.0) * specularReflection;
		}
	}
    gl_FragColor = clamp( gl_FragColor, 0.0, 1.0 );
}