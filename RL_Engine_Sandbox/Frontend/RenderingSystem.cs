using RL_Engine_Sandbox.Backend.ECS.Component;
using RL_Engine_Sandbox.Backend.ECS.Entity;
using RL_Engine_Sandbox.Backend.ECS.Interface;

namespace RL_Engine_Sandbox.Frontend
{
    public class RenderingSystem : IRenderingSystem {
        private readonly IEntityManager _entityManager;
        private readonly IComponentManager _componentManager;
        private readonly Console _renderingConsole;
        public List<Entity> EntitiesToRender { get; set; }
        
        public RenderingSystem(IEntityManager entityManager, IComponentManager componentManager, Console renderingConsole) {
            _entityManager = entityManager;
            _componentManager = componentManager;
            _renderingConsole = renderingConsole;
            EntitiesToRender = new List<Entity>();
        }
        
        public void Update() {
            _renderingConsole.Clear(); 
            foreach (var entity in EntitiesToRender) {
                var position = _componentManager.GetComponent<PositionComponent>(entity.Id);
                var render = _componentManager.GetComponent<RenderingComponent>(entity.Id);
                
                if (position == null || render == null) continue;
                
                _renderingConsole.SetGlyph(
                    position.X,
                    position.Y,
                    render.Glyph.Glyph,
                    render.Glyph.Foreground,
                    render.Glyph.Background);
            }
            _renderingConsole.IsDirty = true; 
        }

    }
}