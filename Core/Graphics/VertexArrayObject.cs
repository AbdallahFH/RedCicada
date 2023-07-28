using Silk.NET.OpenGL;
using System;

namespace RedCicada
{
    class VertexArrayObject<TVertexType,TIndexType> : IDisposable where TVertexType : unmanaged where TIndexType : unmanaged 
    {
        uint ID;
        GL gl;
        public VertexArrayObject(GL _gl,BufferObject<TVertexType> VBO,BufferObject<TIndexType> EBO){
            gl = _gl;
            ID = gl.GenVertexArray();
            Bind();
            VBO.Bind();
            EBO.Bind();
        }
        public unsafe void VertexAttributePointer(uint index, int count,VertexAttribPointerType type, uint vertexSize,int offset)
        {
            gl.VertexAttribPointer(index,count,type,false,vertexSize*(uint)sizeof(TVertexType),(void*) (offset*sizeof(TVertexType)));
            gl.EnableVertexAttribArray(index);
        }
        public void Bind(){
            gl.BindVertexArray(ID);
        }
        public void Dispose(){
            gl.DeleteVertexArray(ID);
        }
    }
}