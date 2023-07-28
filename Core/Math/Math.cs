namespace RedCicada
{
        public struct vector2
        {
            public float x;
            public float y;
            public vector2(float X,float Y){
                x = X;
                y = Y;
            }
        }
        public struct vector2Int
        {
        public int x;
        public int y;
            public vector2Int(int X,int Y){
                x = X;
                y = Y;
            }
        }
        public struct vector3
        {
            public float x;
            public float y;   
            public float z;
            public vector3(float X,float Y,float Z){
                x = X;
                y = Y;
                z = Z;
            }
        }
        public struct vector3Int
        {
            public int x;
            public int y;   
            public int z;
            public vector3Int(int X,int Y,int Z){
                x = X;
                y = Y;
                z = Z;
            }
        }
        public struct vector4
        {
            public float x;
            public float y;   
            public float z;
            public float w;
            public vector4(float X,float Y,float Z,float W){
                x = X;
                y = Y;
                z = Z;
                w = W;
            }
        }
        public struct vector4Int
        {
            public int x;
            public int y;   
            public int z;
            public int w;
            public vector4Int(int X,int Y,int Z,int W){
                x = X;
                y = Y;
                z = Z;
                w = W;
            }
        }
        public static class Math
        {
            public static float DegreesToRadians(float Angle){
                return Angle*(float)System.Math.PI/180;
            }
        }
}