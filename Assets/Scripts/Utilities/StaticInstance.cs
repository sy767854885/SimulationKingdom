using UnityEngine;

/// <summary>
/// һ�㵥���������´����������Ḳ�Ǿ��е���
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
/// �˵������ڳ���ת��ʱ������
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
/// �־��Ե����������л�ʱ���ᱻ����
/// </summary>
public abstract class PersistentSingleton<T> : MonoSingleton<T> where T : MonoBehaviour {
    protected override void Awake() {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }
}