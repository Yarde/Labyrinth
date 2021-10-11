using UnityEngine;

namespace UI
{
    public class UserInterface : MonoBehaviour
    {
        [SerializeField] private GameHud gameHud;
        [SerializeField] private DeadScreen deadScreen;
        [SerializeField] private PauseScreen pauseScreen;

        public void Setup(Player player, Skill[] skills)
        {
            gameHud.Setup(player, skills);
        }
        
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
