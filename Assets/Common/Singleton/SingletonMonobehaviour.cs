namespace Common 
{
    using UnityEngine;

    public class SingletonMonobehaviour<T> : MonoBehaviour where T : MonoBehaviour
    {
        protected static T instance = null;
        private static readonly object lockObject = new object();

        public static T Instance 
        {
            get 
            {
                lock (lockObject)
                {
                    if (instance == null)
                    {
                        //Find existing instance in the scene
                        instance = FindObjectOfType<T>();

                        //Create a new instance if none exists
                        if (instance == null)
                        {
                            GameObject goSingleton = new GameObject(typeof(T).Name);
                            instance = goSingleton.AddComponent<T>();
                        }
                    }

                    return instance;
                }
            }
        }

        protected virtual void Awake()
        {
            if (instance != null && instance != this)
            {
                DestroyImmediate(this.gameObject);
                return;
            }

            instance = this as T;
        }

        protected virtual void OnDestroy()
        {
            if (instance == this)
                instance = null;
        }
    }
}

