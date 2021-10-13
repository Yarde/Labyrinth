using Cysharp.Threading.Tasks;
using Skills;
using UI.Windows;
using UnityEngine;

namespace UI
{
    public class UserInterface : MonoBehaviour
    {
        [SerializeField] private GameHud gameHud;
        [SerializeField] private QuestionScreenBase questionScreen;
        
        [SerializeField] private WindowState deadScreen;
        [SerializeField] private WindowState pauseScreen;

        #region Debug
        private void Update()
        {
            if (Time.timeScale == 0)
            {
                return;
            }
            
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (Time.timeScale == 0)
                    pauseScreen.OnExit();
                else
                    pauseScreen.OnEnter();
            }
        }
        #endregion
        
        public void Setup(Player player, Skill[] skills)
        {
            gameHud.Setup(player, skills);
        }

        public async UniTask OpenQuestion()
        {
            gameHud.Pause();
            
            questionScreen.gameObject.SetActive(true);
            await questionScreen.DisplayQuestion();
            questionScreen.gameObject.SetActive(false);
            
            gameHud.Resume();
        }
    }
}
