using UnityEngine;

namespace UI.HUD
{
    public class ProgressPanel : MonoBehaviour
    {
        [SerializeField] private ObjectiveElement objectiveElementPrefab;
        [SerializeField] private Transform objectiveHolder;
        
        public void Setup(Player.Player player)
        {
            foreach (var playerObjective in player.Objectives)
            {
                var objective = Instantiate(objectiveElementPrefab, objectiveHolder);
                objective.Setup(playerObjective.Value);
            }
        }
    }
}