 using UnityEngine;


 [DisallowMultipleComponent]
 public abstract class SingletonBase<T> : MonoBehaviour where T: MonoBehaviour
 {
    [Header("Singleton")]
    [SerializeField] private bool m_DoNotDestroyOnLoad;

    public static T Instance { get; private set; }

    protected virtual void Awake()
    {
        if(Instance != null)
        {
            Debug.LogWarning("MonoSingleton: object of type already exists, instance well be destroyed = " + typeof(T).Name);
            Destroy(this);
            return;
        }

        Instance = this as T;


        if (m_DoNotDestroyOnLoad)
            DontDestroyOnLoad(gameObject);
    }
 }
