using DG.Tweening;
using UnityEngine;

public class ScaleJumpyAnimation : MonoBehaviour
{
    [SerializeField] private Vector3 maxValue = new Vector3(1.1f,1.1f,1.1f);
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private LoopType loop = LoopType.Yoyo;
    [SerializeField] private Ease easing = Ease.InOutQuad;


    private void Start()
    {
        transform.DOScale(maxValue, speed)
         .SetLoops(-1, loop)
         .SetEase(easing);
    }

}
