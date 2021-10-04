using UnityEngine;

namespace UI
{
    public class UserInterface : MonoBehaviour
    {
        [SerializeField] private DeadScreen deadScreen;
        [SerializeField] private PauseScreen pauseScreen;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Time.timeScale == 0)
                    pauseScreen.OnExit();
                else
                    pauseScreen.OnEnter();
            }
        }
    }
}
