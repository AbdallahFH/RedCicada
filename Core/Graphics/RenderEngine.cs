using System.Numerics;

namespace RedCicada
{
    class SpriteRenderer : Component
    {
        Texture texture;
        Material material;
        public Matrix4x4 matrix;
        Quad sprite;
        public SpriteRenderer(StbImageSharp.ImageResult image,Material _material){
            texture = new Texture(image,Silk.NET.OpenGL.GLEnum.Linear,Silk.NET.OpenGL.GLEnum.Linear);
            material = _material;
            sprite = new Quad(material);
            texture.Bind();
            sprite.Init();
            SpriteSystem.Rigister(this);
        }
        public override void Start(){
           
        }
        public override void Render(float deltaTime){
            material.Use();
            material.SetMatrix("Transform",matrix);
            material.SetMatrix("View",OpenGL.View);
            material.SetMatrix("Projection",OpenGL.Projection);
            sprite.Render(texture);
        }
        public override void Update(float deltaTime){
            
        }

    }    
}