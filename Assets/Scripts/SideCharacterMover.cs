using UnityEngine;

public class SideCharacterMover : MonoBehaviour
{
    [Header("Movimiento Horizontal")]
    public float moveSpeed = 2f;          // Velocidad del desplazamiento
    public float moveDistance = 3f;       // Qué tanto se aleja del punto inicial

    private Vector3 startPos;
    private int direction = 1;            // 1 = derecha, -1 = izquierda

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        transform.Translate(Vector3.right * direction * moveSpeed * Time.deltaTime);

        // Si se aleja demasiado del punto inicial, cambia dirección
        if (Mathf.Abs(transform.position.x - startPos.x) >= moveDistance)
        {
            direction *= -1;
            FlipSprite();
        }
    }

    void FlipSprite()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1; // Voltea el sprite horizontalmente
        transform.localScale = scale;
    }
}
