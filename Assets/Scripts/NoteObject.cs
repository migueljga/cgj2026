using UnityEngine;

public class NoteObject : MonoBehaviour
{
    public float speed = 5f;
    [HideInInspector] public bool wasHit = false;

        void Update()
        {
            transform.Translate(Vector3.down * speed * Time.deltaTime);
        }
}
