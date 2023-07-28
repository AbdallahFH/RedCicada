using Silk.NET.OpenGL;
using System;

namespace RedCicada
{
    public class BufferObject<TDataType> : IDisposable where TDataType :unmanaged
    {
        uint ID;
        BufferTargetARB bufferTarget;
        GL gL;       
        public unsafe BufferObject(GL _gl,Span<TDataType> data,BufferTargetARB bufferType){
        
            gL = _gl;
            bufferTarget = bufferType;
            ID = gL.GenBuffer();
            Bind();
            fixed(void* d = data){
                gL.BufferData(bufferType,(nuint) (data.Length*sizeof(TDataType)),d,BufferUsageARB.StaticDraw);
            }
        }
        public void Bind(){
            gL.BindBuffer(bufferTarget,ID);
        }
        public void Dispose(){
            gL.DeleteBuffer(ID);
        }
    }
}