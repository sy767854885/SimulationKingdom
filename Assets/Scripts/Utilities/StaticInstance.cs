using UnityEngine;

/// <summary>
/// 一般单例，但是新创建的新例会覆盖旧有单例
/// </summary>
public abstract class StaticInstance<T> : MonoBehaviour where T : MonoBehaviour {
    public static T Instance { get; private set; }
    protected virtual void Awake()
    {
        Instance = this as T;
        OnAwake();
    }

    protected virtual void OnAwake(){}

    protected virtual void OnApplicationQuit() {
        Instance = null;
        Destroy(gameObject);
    }
}

/// <summary>
/// 此单例会在场景转换时被销毁
/// </summary>
public abstract class MonoSingleton<T> : StaticInstance<T> where T : MonoBehaviour {
    protected override void Awake() {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        base.Awake();
    }
}

/// <summary>
/// 持久性单例，场景切换时不会被销毁
/// </summary>
public abstract class PersistentSingleton<T> : MonoSingleton<T> where T : MonoBehaviour {
    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}