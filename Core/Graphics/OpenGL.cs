using Silk.NET.OpenGL;
using Silk.NET.Windowing;
using System.Numerics;

namespace RedCicada
{
    public static class OpenGL
    {
        public static GL gl;
        public static Matrix4x4 Projection;
        public static Matrix4x4 View;
        static IWindow window;
        public static void Init(IWindow _window){
            window = _window;
        }
        public static void OnLoad(){
            gl = window.CreateOpenGL();

        }
        public static void OnRender(){

        }
        public static void OnUpdate(){

        }
    }   
   
    public class Quad{
        Material material;
        float[] vertices={
                 0.5f,  0.5f, 0.0f,  0.0f, 0.0f,
                 0.5f, -0.5f, 0.0f,  0.0f, 1.0f,
                -0.5f, -0.5f, 0.0f,  1.0f, 1.0f,
                -0.5f,  0.5f, 0.0f,  1.0f, 0.0f
            };
        uint[] indices = {
            0, 1, 3,
            1, 2, 3
        };  
        static BufferObject<uint> ebo;
        static BufferObject<float> vbo;
        static VertexArrayObject<float , uint> vao;
        public Quad(Material _material){
            material = _material;
        }
        public void Init(){
            ebo = new BufferObject<uint>(OpenGL.gl,indices,BufferTargetARB.ElementArrayBuffer);
            vbo = new BufferObject<float>(OpenGL.gl,vertices,BufferTargetARB.ArrayBuffer);
            vao = new VertexArrayObject<float, uint>(OpenGL.gl,vbo,ebo);
            vao.VertexAttributePointer(0,3,VertexAttribPointerType.Float,5,0);
            vao.VertexAttributePointer(1,2,VertexAttribPointerType.Float,5,3);
        }
        public unsafe void Render(Texture texture){
            vao.Bind();
            material.Use();
            material.SetInt("Texture",(int)texture.ID);
            OpenGL.gl.DrawElements(PrimitiveType.Triangles,(uint)indices.Length,DrawElementsType.UnsignedInt,null);
        }
    }
        public class Texture : IDisposable
        {
            public uint ID;
            public unsafe Texture(StbImageSharp.ImageResult Image,GLEnum MagFilter,GLEnum MinFilter){
                ID = OpenGL.gl.GenTexture();
                OpenGL.gl.ActiveTexture(TextureUnit.Texture0);
                OpenGL.gl.BindTexture(TextureTarget.Texture2D,ID);
                fixed(byte* ptr = Image.Data){
                    OpenGL.gl.TexImage2D(TextureTarget.Texture2D,0,InternalFormat.Rgba,(uint)Image.Width,(uint)Image.Height,0,PixelFormat.Rgba,PixelType.UnsignedByte,ptr);
                }
                OpenGL.gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.Repeat);
                OpenGL.gl.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureWrapT,(int)TextureWrapMode.Repeat);
                OpenGL.gl.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureMinFilter,(int)MinFilter);
                OpenGL.gl.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureMagFilter,(int)MagFilter);
                OpenGL.gl.GenerateMipmap(TextureTarget.Texture2D);
                OpenGL.gl.BindTexture(TextureTarget.Texture2D,0);

            }
            public void Bind(){
                OpenGL.gl.ActiveTexture(TextureUnit.Texture0+(int)ID);
                OpenGL.gl.BindTexture(TextureTarget.Texture2D,ID);
            }
            public void Dispose(){
                OpenGL.gl.DeleteTexture(ID);
            }
        }
}