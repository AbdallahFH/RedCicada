using StbImageSharp;

namespace RedCicada
{
    class AssetManager
    {
        //Load files when drop to window
        public static void Import(string path){
            string extension = Path.GetExtension(path);
            string name = Path.GetFileNameWithoutExtension(path);
            //Load Image
            if(extension ==".png"||extension ==".jpg"){
                int nrChannels = 0;
                ImageResult result = ImageResult.FromMemory(File.ReadAllBytes(path),ColorComponents.RedGreenBlueAlpha);
                if(result.Comp== StbImageSharp.ColorComponents.Grey){
                    nrChannels = 0; 
                }
                if(result.Comp== StbImageSharp.ColorComponents.GreyAlpha){
                    nrChannels = 1; 
                }
                if(result.Comp== StbImageSharp.ColorComponents.RedGreenBlue){
                    nrChannels = 2; 
                }
                if(result.Comp== StbImageSharp.ColorComponents.RedGreenBlueAlpha){
                    nrChannels = 3; 
                }
                ImageFile imageFile = new ImageFile();
                imageFile.Data = result.Data;
                imageFile.Width = result.Width;
                imageFile.Height = result.Height;
                imageFile.nrChannels = nrChannels;
                Directory.CreateDirectory("Assets/Image/");
                FileManager.SaveFile<ImageFile>("Assets/Image/"+name+".img",imageFile);
            }
        }

        //load files with import to folders 
        //save files to assets
        //Pack assets to package when build the game
    }
}