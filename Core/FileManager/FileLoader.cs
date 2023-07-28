using StbImageSharp;


[Serializable]
public class Image{
    public int Width;
    public int Height;
    public byte[] Data;
    public int nrChannels;
    
}

namespace RedCicada
{
    class FileLoader
    {
        public T ImportFile<T>(string path) where T :Image{
            string extension = Path.GetExtension(path);
            if(extension==".png"||extension==".jpg"){
                return (T)LoadImage(path);
            }
            return null;
        }
        Image LoadImage(string path){
            ImageResult result = ImageResult.FromMemory(File.ReadAllBytes(path),ColorComponents.RedGreenBlueAlpha);
            Image image = new Image();
            image.Width = result.Width;
            image.Height = result.Height;
            image.Data = result.Data;
            if(result.Comp== StbImageSharp.ColorComponents.Grey){
                image.nrChannels = 0; 
            }
            if(result.Comp== StbImageSharp.ColorComponents.GreyAlpha){
                image.nrChannels = 1; 
            }
            if(result.Comp== StbImageSharp.ColorComponents.RedGreenBlue){
                image.nrChannels = 2; 
            }
            if(result.Comp== StbImageSharp.ColorComponents.RedGreenBlueAlpha){
                image.nrChannels = 3; 
            }
            return image;
        }
    }
}