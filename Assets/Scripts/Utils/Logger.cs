using UnityEngine;

namespace Utils
{
    public class Logger
    {
        public static void Log(string s)
        {
            if (GameRoot.CheatsAllowed)
            {
                Debug.Log(s);
            }
        }
        public static void LogError(string s)
        {
            if (GameRoot.CheatsAllowed)
            {
                Debug.LogError(s);
            }
        }
    }
}
