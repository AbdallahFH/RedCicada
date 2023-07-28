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
        public float[] vertices={
                 0.5f,  0.5f, 0.0f,  0.0f, 0.0f,
                 0.5f, -0.5f, 0.0f,  0.0f, 1.0f,
                -0.5f, -0.5f, 0.0f,  1.0f, 1.0f,
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
        static Texture texture;
        
        public Vector3 cameraPosition = new Vector3(0,0,-3);
        static Transform transform = new Transform();
        public SpriteRenderer idle;
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
            OpenGL.gl = window.CreateOpenGL();
            ImageResult RawIcon = ImageResult.FromMemory(File.ReadAllBytes(appOptions.IconPath),ColorComponents.RedGreenBlueAlpha);
            RawImage icon = new RawImage(RawIcon.Width,RawIcon.Height,RawIcon.Data);
            window.SetWindowIcon(ref icon);
            //window.WindowBorder= WindowBorder.Fixed;
            Input = window.CreateInput();
            sceneSystem.LoadedScene.Start();
            image = FileManager.LoadFile<ImageFile>("Assets/Image/FEEcQGZXwAEP0kZ.img");
            ImageResult img = ImageResult.FromMemory(File.ReadAllBytes("Assets/Image/mainmenu.png"),ColorComponents.RedGreenBlueAlpha);
            ebo = new BufferObject<uint>(OpenGL.gl,indices,BufferTargetARB.ElementArrayBuffer);
            vbo = new BufferObject<float>(OpenGL.gl,vertices,BufferTargetARB.ArrayBuffer);
            vao = new VertexArrayObject<float, uint>(OpenGL.gl,vbo,ebo);
            vao.VertexAttributePointer(0,3,VertexAttribPointerType.Float,5,0);
            vao.VertexAttributePointer(1,2,VertexAttribPointerType.Float,5,3);
            
            material = new Material("Core/ShaderFiles/VertexShader.v","Core/ShaderFiles/FragmentShader.f");
            material.SetInt("Texture",0);
            ImageResult idle01 = ImageResult.FromMemory(File.ReadAllBytes("Assets/Idle/Idle01.png"),ColorComponents.RedGreenBlueAlpha);
            texture = new Texture(idle01, GLEnum.LinearMipmapLinear,GLEnum.Linear);
            texture.Bind();
            //Projection = Matrix4x4.CreateOrthographicOffCenter(0,appOptions.Resolution.x,appOptions.Resolution.y,0,-10.0f,10.0f);
            OpenGL.Projection = Matrix4x4.CreatePerspectiveFieldOfView(RedCicada.Math.DegreesToRadians(60),(float)appOptions.Resolution.x/(float)appOptions.Resolution.y,0.1f,1000);
            ImageFile f = new ImageFile();
            f.Data = idle01.Data;
            f.Width = idle01.Width;
            f.Height = idle01.Height;
            
            idle = new SpriteRenderer(idle01,material);
            transform.Position = new Vector3(1,0,1);
            transform.Scale = new Vector3(1);
            transform.Rotation = new Quaternion(0,0,0,0);
            transform.UpdateMatrix();
            idle.matrix = transform.Matrix;
            transform.Position = new Vector3(-1,0,1);
            transform.Scale = new Vector3(1);
            transform.Rotation = new Quaternion(0,0,0,0);
            transform.UpdateMatrix();
            OpenGL.gl.Enable(GLEnum.Blend);
            OpenGL.gl.BlendFunc(BlendingFactor.SrcAlpha,BlendingFactor.OneMinusSrcAlpha);
        }
        unsafe void OnRender(double deltaTime){
            OpenGL.gl.ClearColor(Color.SkyBlue);
            OpenGL.gl.Clear((uint)ClearBufferMask.ColorBufferBit);

            OpenGL.View = Matrix4x4.CreateLookAt(cameraPosition,cameraPosition+Vector3.UnitZ,Vector3.UnitY);
            SpriteSystem.Render((float)deltaTime);
            vao.Bind();
            material.Use();
            material.SetMatrix("Transform",transform.Matrix);
            material.SetMatrix("View",OpenGL.View);
            material.SetMatrix("Projection",OpenGL.Projection);
            material.SetInt("Texture",(int)texture.ID);
            texture.Bind();
            OpenGL.gl.DrawElements(PrimitiveType.Triangles,(uint)indices.Length,DrawElementsType.UnsignedInt,null);
        }
        void OnUpdate(double deltaTime){
            sceneSystem.LoadedScene.Update((float)deltaTime);
            
            //window.WindowState = WindowState.Normal;
        }
        void OnResize(Vector2D<int> Action){
            OpenGL.gl.Viewport(Action);
            appOptions.Resolution = new vector2Int(Action.X,Action.Y);
            window.Size = Action;
            OpenGL.Projection = Matrix4x4.CreatePerspectiveFieldOfView(RedCicada.Math.DegreesToRadians(60),(float)appOptions.Resolution.x/(float)appOptions.Resolution.y,0.1f,1000);
        } 
    }
}