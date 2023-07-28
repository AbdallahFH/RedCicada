using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;

namespace RedCicada;

public class Program{
   static Application Game = new Application();
   static ImGuiController controller = null;
   static string rf ="";
   
   static Entity player = new Entity("Player");
   static Entity ground = new Entity();
   static string ConsoleLoger ="";
   
   
   public static void Main(){
      Game.appOptions.SetOptions("Red Cicada",new vector2Int(1280,720),"RedCicada.png",60);
      Game.Init();
      Game.window.Load+=OnLoad;
      Game.window.Render+=OnRender;
      Game.window.Update+=OnUpdate;
      Game.window.FileDrop+=OnDropFile;
      Game.Run();
      
   }
   static void OnLoad(){
      controller = new ImGuiController(OpenGL.gl = Game.window.CreateOpenGL(),Game.window,Game.Input);
      ImGuiNET.ImGui.GetIO().ConfigFlags = ImGuiNET.ImGuiConfigFlags.DockingEnable;
      for (int i = 0; i < Game.Input.Keyboards.Count; i++)
      {
         Game.Input.Keyboards[i].KeyDown+=KeyDown;
         Game.Input.Keyboards[i].KeyUp+=KeyUp;
      }
      PlayerMovment move = new PlayerMovment();
      player.AddComponent(move);
      Game.sceneSystem.LoadedScene.entities.Add(player);
   }
   static void OnRender(double deltaTime){
      controller.Update((float)deltaTime);
      //Editor
      ImGuiNET.ImGui.Begin("App");
      ImGuiNET.ImGui.End();
      ConsoleLoger = rf;
      ImGuiNET.ImGui.End();
      ImGuiNET.ImGui.Begin("Console");
      ImGuiNET.ImGui.Text(ConsoleLoger);
      ImGuiNET.ImGui.End();
      ImGuiNET.ImGui.Begin("Project");
      if(ImGuiNET.ImGui.Button("Import")){
         AssetManager.Import(rf);
      }
      ImGuiNET.ImGui.End();
      ImGuiNET.ImGui.Begin("Inspector");
      ImGuiNET.ImGui.InputFloat3("Camera",ref Game.cameraPosition);
      ImGuiNET.ImGui.End();
      ImGuiNET.ImGui.Begin("Scene List");
      ImGuiNET.ImGui.End();
      controller.Render();
   }
   static void OnUpdate(double deltaTime){
   }
   static void OnDropFile(string[] path){
      for (int i = 0; i < path.Count(); i++)
      {
         AssetManager.Import(path[i]);
         rf+=path[i];
      }
      
   }
   static void KeyDown(IKeyboard keyboard,Key key,int keyCode){
      if(key== Key.Escape){
         Game.window.Close();
      }
   }
   static void KeyUp(IKeyboard keyboard,Key key,int keyCode){
      if(key== Key.F11){
      }
   }
   void Build(string project){
      string command = "";
      System.Diagnostics.Process.Start("CMD.exe",project);
   }
}