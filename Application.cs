using System.Numerics;
using System.Drawing;
using Silk.NET.Windowing;
using Silk.NET.Maths;
using Silk.NET.Input;
using Silk.NET.Core;
using Silk.NET.OpenGL;
using StbImageSharp;

namespace RedCicada{
    public struct AppOptions
    {
        public string Title;
        public vector2Int Resolution;
        public string IconPath;
        public int FPS;  
        public void SetOptions(string title,vector2Int resolution,string iconPath,int fps){
            Title = title;
            Resolution = resolution;
            IconPath = iconPath;
            FPS = fps;
        }
    };
    class Application
    {
        public AppOptions appOptions;
        public IWindow window;
        public IInputContext Input;
        static WindowOptions windowOptions;
        public SceneSystem sceneSystem = new SceneSystem();
        public GL gl;
        public float[] vertices={
                0.5f,  0.5f, 0.0f,  1.0f, 1.0f,
                 0.5f, -0.5f, 0.0f,  0.0f, 1.0f,
                -0.5f, -0.5f, 0.0f,  0.0f, 0.0f,
                -0.5f,  0.5f, 0.0f,  1.0f, 0.0f
            };
        public uint[] indices = {
            0, 1, 3,
            1, 2, 3
        };  
        static BufferObject<uint> ebo;
        static BufferObject<float> vbo;
        static VertexArrayObject<float , uint> vao;
        static Material material;
        static ImageFile image;
        static uint texture;
        static Matrix4x4 Projection;
        static Matrix4x4 View;
        public Vector3 cameraPosition = new Vector3(0,0,-3);
        static Transform transform = new Transform();

        public void Init(){
            windowOptions = WindowOptions.Default with{
                Size = new Vector2D<int>(appOptions.Resolution.x,appOptions.Resolution.y),
                Title = appOptions.Title,
                FramesPerSecond = appOptions.FPS,
            };
            window = Window.Create(windowOptions);
            //window.WindowState = WindowState.Fullscreen;
            window.Load+=OnLoad;
            window.Render+=OnRender;
            window.Update+=OnUpdate;
            window.Resize+=OnResize;
        }
        public void Run(){
            window.Run();
        }
        unsafe void OnLoad(){     
            gl = window.CreateOpenGL();
            ImageResult RawIcon = ImageResult.FromMemory(File.ReadAllBytes(appOptions.IconPath),ColorComponents.RedGreenBlueAlpha);
            RawImage icon = new RawImage(RawIcon.Width,RawIcon.Height,RawIcon.Data);
            window.SetWindowIcon(ref icon);
            //window.WindowBorder= WindowBorder.Fixed;
            Input = window.CreateInput();
            sceneSystem.LoadedScene.Start();
            image = FileManager.LoadFile<ImageFile>("Assets/Image/FEEcQGZXwAEP0kZ.img");
            ImageResult img = ImageResult.FromMemory(File.ReadAllBytes("Assets/Image/mainmenu.png"),ColorComponents.RedGreenBlueAlpha);
            ebo = new BufferObject<uint>(gl,indices,BufferTargetARB.ElementArrayBuffer);
            vbo = new BufferObject<float>(gl,vertices,BufferTargetARB.ArrayBuffer);
            vao = new VertexArrayObject<float, uint>(gl,vbo,ebo);
            vao.VertexAttributePointer(0,3,VertexAttribPointerType.Float,5,0);
            vao.VertexAttributePointer(1,2,VertexAttribPointerType.Float,5,3);
            
            material = new Material("Core/ShaderFiles/VertexShader.v","Core/ShaderFiles/FragmentShader.f",gl);
            texture = gl.GenTexture();
            material.SetInt("Texture",0);
            gl.ActiveTexture(TextureUnit.Texture0);
            gl.BindTexture(TextureTarget.Texture2D,texture);
            fixed(byte* ptr = img.Data){
                gl.TexImage2D(TextureTarget.Texture2D,0,InternalFormat.Rgba,(uint)img.Width,(uint)img.Height,0,PixelFormat.Rgba,PixelType.UnsignedByte,ptr);
            }
            gl.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int) TextureWrapMode.Repeat);
            gl.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureWrapT,(int)TextureWrapMode.Repeat);
            gl.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureMinFilter,(int)TextureMinFilter.Linear);
            gl.TexParameter(TextureTarget.Texture2D,TextureParameterName.TextureMagFilter,(int)TextureMagFilter.Linear);
            gl.GenerateMipmap(TextureTarget.Texture2D);
            gl.BindTexture(TextureTarget.Texture2D,0);



            //Projection = Matrix4x4.CreateOrthographicOffCenter(0,appOptions.Resolution.x,appOptions.Resolution.y,0,-10.0f,10.0f);
            Projection = Matrix4x4.CreatePerspectiveFieldOfView(RedCicada.Math.DegreesToRadians(60),appOptions.Resolution.x/appOptions.Resolution.y,0.1f,1000);
            //material.SetMatrix("View",View);

            transform.Position = new Vector3(0,0,1);
            transform.Scale = new Vector3(1);
            transform.Rotation = new Quaternion(0,0,0,0);
            transform.UpdateMatrix();
            gl.Enable(GLEnum.Blend);
            gl.BlendFunc(BlendingFactor.SrcAlpha,BlendingFactor.OneMinusSrcAlpha);
        }
        unsafe void OnRender(double deltaTime){
        }
        public unsafe void Render(){
            gl.ClearColor(Color.SkyBlue);
            gl.Clear((uint)ClearBufferMask.ColorBufferBit);
            vao.Bind();
            View = Matrix4x4.CreateLookAt(cameraPosition,cameraPosition+Vector3.UnitZ,Vector3.UnitY);
            material.Use();
            material.SetMatrix("Transform",transform.Matrix);
            material.SetMatrix("View",View);
            material.SetMatrix("Projection",Projection);
            
            
            gl.ActiveTexture(TextureUnit.Texture0);
            gl.BindTexture(TextureTarget.Texture2D,texture);
            gl.DrawElements(PrimitiveType.Triangles,(uint)indices.Length,DrawElementsType.UnsignedInt,null);
        }
        void OnUpdate(double deltaTime){
            sceneSystem.LoadedScene.Update((float)deltaTime);
            
            //window.WindowState = WindowState.Normal;
        }
        void OnResize(Vector2D<int> Action){
            gl.Viewport(Action);
            appOptions.Resolution = new vector2Int(Action.X,Action.Y);
            window.Size = Action;
        } 
    }
}