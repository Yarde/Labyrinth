using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Spinner : MonoBehaviour
{
    [SerializeField] private Image background;
    
    private void Start()
    {
        background.transform.DORotate(new Vector3(0f, 0f, 360f), 180f, RotateMode.FastBeyond360)
            .SetLoops(-1).SetEase(Ease.Linear).SetId("Spinner: background DORotate");
    }
}
