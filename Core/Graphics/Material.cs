using Silk.NET.OpenGL;
using System.Numerics;

namespace RedCicada
{
    public class Material
    {
        static uint ID;
        public Material(string VertexPath,string FragmentPath){
            string VertexCode = File.ReadAllText(VertexPath);
            string fragmentCode = File.ReadAllText(FragmentPath);
            uint vertexShader = OpenGL.gl.CreateShader(ShaderType.VertexShader);
            OpenGL.gl.ShaderSource(vertexShader,VertexCode);
            OpenGL.gl.CompileShader(vertexShader);
            OpenGL.gl.GetShader(vertexShader,ShaderParameterName.CompileStatus,out int vStatus);
            if(vStatus !=(int)GLEnum.True){
                throw new Exception("VertexShader fail:"+OpenGL.gl.GetShaderInfoLog(vertexShader));
            }
            uint framgentShader = OpenGL.gl.CreateShader(ShaderType.FragmentShader);
            OpenGL.gl.ShaderSource(framgentShader,fragmentCode);
            OpenGL.gl.CompileShader(framgentShader);
            OpenGL.gl.GetShader(framgentShader,ShaderParameterName.CompileStatus,out int fStatus);
            if(fStatus !=(int)GLEnum.True){
                throw new Exception("fragmentShader fail:"+OpenGL.gl.GetShaderInfoLog(framgentShader));
            }
            ID = OpenGL.gl.CreateProgram();
            OpenGL.gl.AttachShader(ID,vertexShader);
            OpenGL.gl.AttachShader(ID,framgentShader);
            OpenGL.gl.LinkProgram(ID);
            OpenGL.gl.GetProgram(ID,ProgramPropertyARB.LinkStatus,out int pStatus);
            if(pStatus !=(int)GLEnum.True){
                throw new Exception("Shader Program fail:"+OpenGL.gl.GetProgramInfoLog(ID));
            }
            OpenGL.gl.DetachShader(ID,vertexShader);
            OpenGL.gl.DetachShader(ID,framgentShader);
            OpenGL.gl.DeleteShader(vertexShader);
            OpenGL.gl.DeleteShader(framgentShader);
        }
        public void Use(){
            OpenGL.gl.UseProgram(ID);
        }
        public void SetInt(string name,int value){
            int location = OpenGL.gl.GetUniformLocation(ID,name);
            OpenGL.gl.Uniform1(location,value);
        }
        public void SetFloat(string name,float value){
            int location = OpenGL.gl.GetUniformLocation(ID,name);
            OpenGL.gl.Uniform1(location,value);
        }
        public unsafe void SetMatrix(string name,Matrix4x4 value){
            int location = OpenGL.gl.GetUniformLocation(ID,name);
            OpenGL.gl.UniformMatrix4(location,1,false,(float*) &value);
        }
    }
}