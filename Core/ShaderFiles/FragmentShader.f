#version 330 core
in vec2 UV;
out vec4 Color;

uniform sampler2D Texture;

void main()
{
    vec4 tex = texture(Texture,UV);
    if(tex.a< 0.1){
        discard;
    }
    Color = tex;
}