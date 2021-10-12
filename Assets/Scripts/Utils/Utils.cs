using UnityEngine;

namespace Utils
{
    public static class Utils
    {
        public static void DestroyIfExist(this GameObject gameObject)
        {
            if (gameObject != null)
            {
                Object.Destroy(gameObject);
            }
        }
    }
}