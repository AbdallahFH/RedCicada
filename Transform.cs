using System.Numerics;

namespace RedCicada{

    class Transform : Component
    {
        public Transform (){
            TransformSystem.Rigister(this);
        }
        public Vector3 Position = Vector3.Zero;
        public Quaternion Rotation;
        public Vector3 Scale;
        public Matrix4x4 Matrix;
        public void UpdateMatrix(){
            var Mat = Matrix4x4.Identity * Matrix4x4.CreateFromQuaternion(Rotation)*Matrix4x4.CreateScale(Scale)*Matrix4x4.CreateTranslation(Position);
            Matrix = Mat;
        }
        public override void Update(float deltaTime){
        
        }
    }
}