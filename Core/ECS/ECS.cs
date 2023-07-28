namespace RedCicada
{
    class Component
    {
        public Entity entity;
        public virtual void Start(){}
        public virtual void Update(float deltaTime){}
    }
    class Entity : Component
    {
        public string Name = "Entity";
        public Transform transform = new Transform();
        public Entity(){    
        }
        public Entity(string name){
            Name = name;
        }
        List<Component> components = new List<Component>();
        public void AddComponent(Component component){
            components.Add(component);
            component.entity = this;
        }
        public T GetComponent<T>() where T :Component{
            foreach (Component component in components)
            {
                if(component.GetType().Equals(typeof(T))){
                    return (T)component;
                }
            }
            return null;
        }
    }
    class BaseSystem<T> where T : Component 
    {
        protected static List<T> components = new List<T>();
        public static void Rigister(T component){
            components.Add(component);
        }
        public static void Start(){
            foreach (T component in components)
            {
                component.Start();
            }
        }
        public static void Update(float deltaTime){
            foreach (T component in components)
            {
                component.Update(deltaTime);
            }
        }
    }
    class TransformSystem : BaseSystem <Transform>{}
    class ScriptSystem : BaseSystem<CicadaScript>{}
    class Scene
    {
        public string Name = "New Scene";
        public List<Entity> entities = new List<Entity>();
        public Scene(){}
        public Scene(string name){
            Name = name;
        }
        public void Start(){
            ScriptSystem.Start();
            TransformSystem.Start();
        }
        public void Update(float deltaTime){
            ScriptSystem.Update(deltaTime);
            TransformSystem.Update(deltaTime);
        }
    }
    class SceneSystem 
    {
        public List<Scene> scenes = new List<Scene>();
        public Scene LoadedScene = new Scene();
        public SceneSystem(){
            Scene scene = new Scene();
            LoadedScene = scene;
            scenes.Add(scene);
        }
        public void LoadScene(Scene scene){
            LoadedScene = scene;
        }
    }
}