#version 330 core

out vec4 FragColor;
in vec2 texCoord;

uniform sampler2D texture0;
uniform sampler2D texture1;
uniform sampler2D noiseTexture;

uniform vec2 iResolution;
uniform float iTime;

float timeScale = 0.05;
float range = 0.1;
float threshold = 0.8;

void main()
{
    // Scroll the noise texture vertically
    vec4 noise = texture(noiseTexture,vec2(texCoord.x,texCoord.y-iTime*timeScale));

    // Follows tutorial control
    float t =  smoothstep(threshold-range,threshold+range,noise.r);

    // Pulsing effect
    float strength = sin(iTime)/2.0+0.5;

    // Random colors for water
    vec4 color1 = mix(vec4(0.,1.,1.,1.),vec4(0.,0.,1.,1.),noise.r);
    vec4 color2 = mix(color1,vec4(0.,0.,1.,1.),noise.r);

    FragColor = mix(color2,color1,t*strength);
}