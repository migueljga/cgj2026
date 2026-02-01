using DG.Tweening;
using UnityEngine;

public class VerticalJumpyAnimation : MonoBehaviour
{
    [SerializeField] private float maxValue = 1.3f;
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private LoopType loop = LoopType.Yoyo;
    [SerializeField] private Ease easing = Ease.InOutQuad;

    private void Start()
    {
        transform.DOLocalMoveY(maxValue, speed)
         .SetLoops(-1, loop)
         .SetEase(easing);
    }
}
