using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class ProgressPanel : MonoBehaviour
    {
        [SerializeField] private ObjectiveElement objectiveElementPrefab;
        [SerializeField] private Transform objectiveHolder;

        private List<ObjectiveElement> _objectives;
        
        public void Setup(Player player)
        {
            _objectives = new List<ObjectiveElement>();
            
            foreach (var playerObjective in player.Objectives)
            {
                var objective = Instantiate(objectiveElementPrefab, objectiveHolder);
                objective.Setup(playerObjective.Value);
                _objectives.Add(objective);
            }
        }
    }
}