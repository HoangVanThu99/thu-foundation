namespace Pancake
{
    /// <summary>
    /// Base class MonoBehavior
    /// </summary>
    public class BaseMono : Mono, ITickSystem, IFixedTickSystem, ILateTickSystem
    {
        /// <summary>
        /// Cached transform
        /// </summary>
        public UnityEngine.Transform CachedTransform { get; private set; }

        private void OnEnable()
        {
            if (CachedTransform == null) CachedTransform = transform;
            OnEnabled();
            Subscribe();
        }

        private void OnDisable()
        {
            Unsubscribe();
            OnDisabled();
        }

        private void Subscribe()
        {
            Runtime.AddTick(this);
            Runtime.AddFixedTick(this);
            Runtime.AddLateTick(this);
        }

        private void Unsubscribe()
        {
            Runtime.RemoveTick(this);
            Runtime.RemoveFixedTick(this);
            Runtime.RemoveLateTick(this);
        }


        void ITickSystem.OnTick() => Tick();

        void IFixedTickSystem.OnFixedTick() => FixedTick();

        void ILateTickSystem.OnLateTick() => LateTick();


        protected virtual void OnEnabled() { }
        protected virtual void OnDisabled() { }
        protected virtual void Tick() { }
        protected virtual void FixedTick() { }
        protected virtual void LateTick() { }
    }
}