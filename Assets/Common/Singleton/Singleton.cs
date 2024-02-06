namespace Common 
{
    using UnityEngine;

    public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
    {
        public static T Instance { get; protected set; } = null;

        protected virtual void Awake()
        {
            if (Instance == null)
                Instance = this as T;
        }

        protected virtual void OnDestroy()
        {
            if (Instance == this)
                Instance = null;
        }
    }
}

