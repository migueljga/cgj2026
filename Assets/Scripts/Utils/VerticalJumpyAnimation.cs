using DG.Tweening;
using UnityEngine;

public class VerticalJumpyAnimation : MonoBehaviour
{
    [SerializeField] private Orientation orientation = Orientation.Y;
    [SerializeField] private float maxValue = 1.3f;
    [SerializeField] private float speed = 0.5f;
    [SerializeField] private LoopType loop = LoopType.Yoyo;
    [SerializeField] private Ease easing = Ease.InOutQuad;

    public enum Orientation { X, Y }

    private void Start()
    {
        switch (orientation)
        {
            case Orientation.X:
                transform.DOLocalMoveX(maxValue, speed)
                 .SetLoops(-1, loop)
                 .SetEase(easing);
                break;
            case Orientation.Y:
            default:
                transform.DOLocalMoveY(maxValue, speed)
                 .SetLoops(-1, loop)
                 .SetEase(easing);
                break;
        }
            
    }
}
