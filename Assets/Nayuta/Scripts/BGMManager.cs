using UnityEngine;

public class BGMManager : MonoBehaviour
{
    private static BGMManager instance;

    void Awake()
    {
        // ‚·‚Å‚É‘¶İ‚·‚éê‡‚ÍV‚µ‚¢‚à‚Ì‚ğ”jŠü
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject); // ƒV[ƒ“‘JˆÚ‚Å”jŠü‚³‚ê‚È‚¢
    }
}
