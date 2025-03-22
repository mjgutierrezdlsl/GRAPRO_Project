#version 330 core

out vec4 FragColor;
in vec2 texCoord;

uniform sampler2D texture0;
uniform sampler2D texture1;
uniform sampler2D noiseTexture;

uniform vec2 iResolution;
uniform float iTime;

float timeScale = 0.1;
float range = 0.1;
float threshold = 0.5;

void main()
{
    vec4 noise = texture(noiseTexture,vec2(texCoord.x,texCoord.y+iTime*timeScale));
    float t =  smoothstep(threshold-range,threshold+range,noise.r);
    float strength = sin(iTime)/2.0+0.5;
    vec4 color1 = mix(vec4(0.,1.,1.,1.),vec4(0.,0.,1.,1.),noise.g);
    FragColor = mix(mix(color1,vec4(0.,0.,1.,1.),noise.b),color1,t*strength);
}