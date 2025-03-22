#version 330 core

out vec4 FragColor;
in vec2 texCoord;

uniform sampler2D texture0;
uniform sampler2D texture1;
uniform sampler2D noiseTexture;

uniform vec2 iResolution;
uniform float iTime;
uniform float timeScale;

void main()
{
    vec4 noise = texture(noiseTexture,vec2(texCoord.x+iTime*timeScale,texCoord.y));
    FragColor = mix(vec4(0.,1.,0.,1.),vec4(0.,0.,1.,1.),noise.r);
}