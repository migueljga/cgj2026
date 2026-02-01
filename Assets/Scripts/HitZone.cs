using UnityEngine;
using System.Collections.Generic;

public class HitZone : MonoBehaviour
{
    public KeyCode key;
    public GameManager gameManager;
    private List<GameObject> notesInZone = new List<GameObject>();
    public GameObject hitParticlesPrefab;
    public Transform hitZone;

    public float pulseAmount = 1.2f;
    public float pulseSpeed = 8f;

    void Update()
    {
        float beatPulse = Mathf.Sin(Time.time * pulseSpeed) * 0.1f + 1f;
        hitZone.localScale = Vector3.one * beatPulse;

        if (Input.GetKeyDown(key) && notesInZone.Count > 0)
        {

            GameObject note = notesInZone[0];
            NoteObject noteScript = note.GetComponent<NoteObject>();
            noteScript.wasHit = true; //Marcar como acertada
            JudgeHit(note);
            notesInZone.RemoveAt(0);
            SpawnHitEffect(hitZone.position);
            Destroy(note);            
        }
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Note"))
            notesInZone.Add(col.gameObject);
    }


    //No funciona. Al destruir el objeto presionando su tecla, lo cuenta como fallido
    //se procede a marcar las notas acertadas de aquellas que se dejaron pasar
    void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Note"))
        {
            
            NoteObject note = col.GetComponent<NoteObject>();
            // Si la nota ya fue acertada, no contar como fallo
            if (note != null && note.wasHit)
                return;
            // Si la nota sale sin ser presionada = MISS
            notesInZone.Remove(col.gameObject);
            gameManager.RegisterMiss();
            Destroy(col.gameObject);
        }
    }

    void JudgeHit(GameObject note)
    {
        float distance = Mathf.Abs(note.transform.position.y - transform.position.y);

        if (distance < 0.2f)
            gameManager.AddScore("Perfect");
        else if (distance < 0.5f)
            gameManager.AddScore("Good");
        else
            gameManager.AddScore("Bad");
    }

    void SpawnHitEffect(Vector3 position)
    {
        Instantiate(hitParticlesPrefab, position, Quaternion.identity);
    }

}
