using Silk.NET.OpenGL;
using Silk.NET.Windowing;

namespace RedCicada
{
    class OpenGL
    {
        GL gl;
        public void Init(IWindow window){
            gl = window.CreateOpenGL();
        }
        public void OnLoad(){

        }
        public void OnRender(){

        }
        public void OnUpdate(){

        }
    }   
}