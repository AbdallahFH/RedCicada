using Silk.NET.OpenGL;
using System.Numerics;

namespace RedCicada
{
    class Material
    {
        static uint ID;
        public GL gl;
        public Material(string VertexPath,string FragmentPath,GL _gl){
            gl = _gl;
            string VertexCode = File.ReadAllText(VertexPath);
            string fragmentCode = File.ReadAllText(FragmentPath);
            uint vertexShader = gl.CreateShader(ShaderType.VertexShader);
            gl.ShaderSource(vertexShader,VertexCode);
            gl.CompileShader(vertexShader);
            gl.GetShader(vertexShader,ShaderParameterName.CompileStatus,out int vStatus);
            if(vStatus !=(int)GLEnum.True){
                throw new Exception("VertexShader fail:"+gl.GetShaderInfoLog(vertexShader));
            }
            uint framgentShader = gl.CreateShader(ShaderType.FragmentShader);
            gl.ShaderSource(framgentShader,fragmentCode);
            gl.CompileShader(framgentShader);
            gl.GetShader(framgentShader,ShaderParameterName.CompileStatus,out int fStatus);
            if(fStatus !=(int)GLEnum.True){
                throw new Exception("fragmentShader fail:"+gl.GetShaderInfoLog(framgentShader));
            }
            ID = gl.CreateProgram();
            gl.AttachShader(ID,vertexShader);
            gl.AttachShader(ID,framgentShader);
            gl.LinkProgram(ID);
            gl.GetProgram(ID,ProgramPropertyARB.LinkStatus,out int pStatus);
            if(pStatus !=(int)GLEnum.True){
                throw new Exception("Shader Program fail:"+gl.GetProgramInfoLog(ID));
            }
            gl.DetachShader(ID,vertexShader);
            gl.DetachShader(ID,framgentShader);
            gl.DeleteShader(vertexShader);
            gl.DeleteShader(framgentShader);
        }
        public void Use(){
            gl.UseProgram(ID);
        }
        public void SetInt(string name,int value){
            int location = gl.GetUniformLocation(ID,name);
            gl.Uniform1(location,value);
        }
        public void SetFloat(string name,float value){
            int location = gl.GetUniformLocation(ID,name);
            gl.Uniform1(location,value);
        }
        public unsafe void SetMatrix(string name,Matrix4x4 value){
            int location = gl.GetUniformLocation(ID,name);
            gl.UniformMatrix4(location,1,false,(float*) &value);
        }
    }
}