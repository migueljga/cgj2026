using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


[System.Serializable]
public class NoteBeat
{
    public float beat;
    public int lane; // 0=izq, 1=abajo, 2=arriba, 3=derecha
}

public class GameManager : MonoBehaviour
{

    [Header("Debug")]
    public float startBeat = 0f; // Empezar la canción adelantada para pruebas

    public AudioSource musicSource;
    public float songBPM = 120f;

    private float songStartTime;

    public float songPosition => Time.time - songStartTime;
    public float secPerBeat => 60f / songBPM;

    public GameObject[] laneNotePrefabs;
    public Transform[] laneSpawnPoints;
    //public List<NoteBeat> notes;
    private List<NoteBeat> notes = new List<NoteBeat>();

    private int nextNoteIndex = 0;

    public int score;
    public int combo;

    //para ccalcular tiempo de viaje de las notas y no aparezcan juntas en los primeros beats
    public Transform hitZone;        // zona donde el jugador presiona
    public Transform noteSpawnPoint; // punto donde nacen las notas
    public float noteSpeed = 5f;     // debe coincidir con NoteObject

    private float noteTravelTime;

    public float fadeOutDuration = 3f; // segundos que tarda en apagarse la música

    private bool isFadingOut = false;
    private float originalVolume;

    [Header("Resultados UI")]
    public GameObject resultsPanel;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI maxComboText;

    public Image flashImage;
    public int comboToFlash;

    private int maxCombo = 0;

    void Start()
    {
        LoadMediumLevel();
        notes.Sort((a, b) => a.beat.CompareTo(b.beat));

        float startTimeInSong = startBeat * secPerBeat;

        songStartTime = Time.time  - startTimeInSong;
        musicSource.time = startTimeInSong; //saltar en audio
        musicSource.Play();

        originalVolume = musicSource.volume;

        float distance = Vector3.Distance(noteSpawnPoint.position, hitZone.position);
        noteTravelTime = distance / noteSpeed;    

        SkipNotesBeforeStart();    
    }

    void Update()
    {
        //Versión que intentaba corregir aparición simultáneas de notas en los primeros beats
        //No funcionó
        //se ejecuta en base a la distancia de las notas
        float songTime = songPosition;

        while (nextNoteIndex < notes.Count)
        {
            float noteBeatTime = notes[nextNoteIndex].beat * secPerBeat;
            float spawnTime = noteBeatTime - noteTravelTime;

            if (songTime >= spawnTime)
            {
                SpawnNote(notes[nextNoteIndex]);
                nextNoteIndex++;
            }
            else
            {
                break;
            }
        }

        if (!isFadingOut && nextNoteIndex >= notes.Count)
        {
            StartCoroutine(FadeOutMusic());
            isFadingOut = true;
        }



        //versión inicial de instanciamiento de notas
        //en los primeros beats aparecían incorrectamente 2 notas simultáneas.
        //se ejecuta en base al tiempo de las notas

        /*float currentBeat = songPosition / secPerBeat;

        if (nextNoteIndex < notes.Count && currentBeat >= notes[nextNoteIndex].beat - 2f) 
        {
            SpawnNote(notes[nextNoteIndex]);
            nextNoteIndex++;
        }*/
    }

    void SpawnNote(NoteBeat note)
    {
        int lane = note.lane;

        if (lane < 0 || lane >= laneNotePrefabs.Length)
        {
            Debug.LogWarning("Carril inválido: " + lane);
            return;
        }

        Instantiate(laneNotePrefabs[lane], laneSpawnPoints[lane].position, Quaternion.identity);

        
        //Instantiate(notePrefab, laneSpawnPoints[note.lane].position, Quaternion.identity);
    }



    public void AddScore(string rating)
    {
        switch (rating)
        {
            case "Perfect":
                score += 1000;
                combo++;
                break;

            case "Good":
                score += 500;
                combo++;
                break;

            case "Bad":
                score += 100;
                combo = 0;
                break;
        }

        Debug.Log(rating + " | Score: " + score + " | Combo: " + combo);

        if (combo > maxCombo)
            maxCombo = combo;
        if (combo > comboToFlash)
            Flash();
    }

    public void RegisterMiss()
    {
        combo = 0;
        Debug.Log("Miss!");
    }

    void ShowResults()
    {
        Time.timeScale = 0f; // Pausa el juego

        resultsPanel.SetActive(true);

        scoreText.text = "Score: " + score;
        maxComboText.text = "Max Combo: " + maxCombo;
    }

    void SkipNotesBeforeStart()
    {
        while (nextNoteIndex < notes.Count &&
               notes[nextNoteIndex].beat * secPerBeat < musicSource.time)
        {
            nextNoteIndex++;
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void ExitGame()
    {
        Debug.Log("Saliendo del juego..."); // Para probar en el editor
        Application.Quit(); // Cierra la aplicación compilada
    }

    void LoadMediumLevel()
    {
        notes = new List<NoteBeat>()
        {

            /*
          // --- MINUTO 1 ---
new NoteBeat { beat = 1f, lane = 1 },
new NoteBeat { beat = 2f, lane = 0 },
new NoteBeat { beat = 3f, lane = 2 },
new NoteBeat { beat = 4f, lane = 3 },
new NoteBeat { beat = 5f, lane = 1 },
new NoteBeat { beat = 6f, lane = 2 },
new NoteBeat { beat = 7f, lane = 0 },
new NoteBeat { beat = 8f, lane = 3 },

new NoteBeat { beat = 9f, lane = 1 },
new NoteBeat { beat = 9.5f, lane = 3 },
new NoteBeat { beat = 10f, lane = 1 },
new NoteBeat { beat = 11f, lane = 0 },
new NoteBeat { beat = 12f, lane = 2 },
new NoteBeat { beat = 13f, lane = 1 },
new NoteBeat { beat = 14f, lane = 3 },
new NoteBeat { beat = 15f, lane = 2 },
new NoteBeat { beat = 16f, lane = 0 },

new NoteBeat { beat = 17f, lane = 1 },
new NoteBeat { beat = 18f, lane = 2 },
new NoteBeat { beat = 19f, lane = 3 },
new NoteBeat { beat = 20f, lane = 0 },
new NoteBeat { beat = 21f, lane = 2 },
new NoteBeat { beat = 22f, lane = 1 },
new NoteBeat { beat = 23f, lane = 3 },
new NoteBeat { beat = 24f, lane = 2 },

new NoteBeat { beat = 25f, lane = 0 },
new NoteBeat { beat = 25.5f, lane = 1 },
new NoteBeat { beat = 26f, lane = 2 },
new NoteBeat { beat = 27f, lane = 3 },
new NoteBeat { beat = 28f, lane = 1 },
new NoteBeat { beat = 29f, lane = 2 },
new NoteBeat { beat = 30f, lane = 3 },
new NoteBeat { beat = 31f, lane = 1 },
new NoteBeat { beat = 32f, lane = 2 },

new NoteBeat { beat = 33f, lane = 1 },
new NoteBeat { beat = 34f, lane = 1 },
new NoteBeat { beat = 35f, lane = 3 },
new NoteBeat { beat = 36f, lane = 0 },
new NoteBeat { beat = 37f, lane = 2 },
new NoteBeat { beat = 38f, lane = 1 },
new NoteBeat { beat = 39f, lane = 3 },
new NoteBeat { beat = 40f, lane = 2 },

new NoteBeat { beat = 41f, lane = 0 },
new NoteBeat { beat = 41.5f, lane = 2 },
new NoteBeat { beat = 42f, lane = 1 },
new NoteBeat { beat = 43f, lane = 3 },
new NoteBeat { beat = 44f, lane = 2 },
new NoteBeat { beat = 45f, lane = 0 },
new NoteBeat { beat = 46f, lane = 3 },
new NoteBeat { beat = 47f, lane = 1 },
new NoteBeat { beat = 48f, lane = 2 },

new NoteBeat { beat = 49f, lane = 1 },
new NoteBeat { beat = 50f, lane = 2 },
new NoteBeat { beat = 51f, lane = 0 },
new NoteBeat { beat = 52f, lane = 3 },
new NoteBeat { beat = 53f, lane = 1 },
new NoteBeat { beat = 54f, lane = 0 },
new NoteBeat { beat = 55f, lane = 2 },
new NoteBeat { beat = 56f, lane = 3 },

new NoteBeat { beat = 57f, lane = 1 },
new NoteBeat { beat = 58f, lane = 0 },
new NoteBeat { beat = 59f, lane = 2 },
new NoteBeat { beat = 60f, lane = 3 },
new NoteBeat { beat = 61f, lane = 1 },
new NoteBeat { beat = 62f, lane = 2 },
new NoteBeat { beat = 63f, lane = 0 },
new NoteBeat { beat = 64f, lane = 3 },
new NoteBeat { beat = 65f, lane = 1 },
new NoteBeat { beat = 66f, lane = 2 },
new NoteBeat { beat = 67f, lane = 3 },
new NoteBeat { beat = 68f, lane = 1 },
new NoteBeat { beat = 69f, lane = 0 },*/

// --- MINUTO 2 (difícil) ---

new NoteBeat { beat = 70f, lane = 1 },
new NoteBeat { beat = 70.25f, lane = 2 },
new NoteBeat { beat = 70.5f, lane = 3 },
new NoteBeat { beat = 71f, lane = 0 },
new NoteBeat { beat = 71f, lane = 2 },

new NoteBeat { beat = 72f, lane = 1 },
new NoteBeat { beat = 72.5f, lane = 3 },
new NoteBeat { beat = 73f, lane = 2 },
new NoteBeat { beat = 73.25f, lane = 1 },
new NoteBeat { beat = 73.5f, lane = 0 },

new NoteBeat { beat = 74f, lane = 3 },
new NoteBeat { beat = 74f, lane = 1 },
new NoteBeat { beat = 74.75f, lane = 2 },
new NoteBeat { beat = 75f, lane = 0 },
new NoteBeat { beat = 75.5f, lane = 3 },

new NoteBeat { beat = 76f, lane = 2 },
new NoteBeat { beat = 76.25f, lane = 1 },
new NoteBeat { beat = 76.5f, lane = 0 },
new NoteBeat { beat = 77f, lane = 3 },
new NoteBeat { beat = 77f, lane = 2 },

new NoteBeat { beat = 78f, lane = 1 },
new NoteBeat { beat = 78.5f, lane = 0 },
new NoteBeat { beat = 79f, lane = 3 },
new NoteBeat { beat = 79.25f, lane = 2 },
new NoteBeat { beat = 79.5f, lane = 1 },

new NoteBeat { beat = 80f, lane = 0 },
new NoteBeat { beat = 80f, lane = 3 },
new NoteBeat { beat = 80.75f, lane = 2 },
new NoteBeat { beat = 81f, lane = 1 },
new NoteBeat { beat = 81.5f, lane = 3 },

new NoteBeat { beat = 82f, lane = 0 },
new NoteBeat { beat = 82.25f, lane = 2 },
new NoteBeat { beat = 82.5f, lane = 1 },
new NoteBeat { beat = 83f, lane = 3 },
new NoteBeat { beat = 83f, lane = 0 },

new NoteBeat { beat = 84f, lane = 2 },
new NoteBeat { beat = 84.5f, lane = 1 },
new NoteBeat { beat = 85f, lane = 3 },
new NoteBeat { beat = 85.25f, lane = 0 },
new NoteBeat { beat = 85.5f, lane = 2 },

new NoteBeat { beat = 86f, lane = 1 },
new NoteBeat { beat = 86f, lane = 3 },
new NoteBeat { beat = 86.75f, lane = 0 },
new NoteBeat { beat = 87f, lane = 2 },
new NoteBeat { beat = 87.5f, lane = 1 },

new NoteBeat { beat = 88f, lane = 3 },
new NoteBeat { beat = 88.25f, lane = 2 },
new NoteBeat { beat = 88.5f, lane = 1 },
new NoteBeat { beat = 89f, lane = 0 },
new NoteBeat { beat = 89f, lane = 2 },

new NoteBeat { beat = 90f, lane = 3 },
new NoteBeat { beat = 90.5f, lane = 1 },
new NoteBeat { beat = 91f, lane = 2 },
new NoteBeat { beat = 91.25f, lane = 0 },
new NoteBeat { beat = 91.5f, lane = 3 },

new NoteBeat { beat = 92f, lane = 1 },
new NoteBeat { beat = 92f, lane = 2 },
new NoteBeat { beat = 92.75f, lane = 3 },
new NoteBeat { beat = 93f, lane = 0 },
new NoteBeat { beat = 93.5f, lane = 1 },

new NoteBeat { beat = 94f, lane = 2 },
new NoteBeat { beat = 94.25f, lane = 3 },
new NoteBeat { beat = 94.5f, lane = 0 },
new NoteBeat { beat = 95f, lane = 1 },
new NoteBeat { beat = 95f, lane = 3 },

new NoteBeat { beat = 96f, lane = 2 },
new NoteBeat { beat = 96.5f, lane = 1 },
new NoteBeat { beat = 97f, lane = 0 },
new NoteBeat { beat = 97.25f, lane = 2 },
new NoteBeat { beat = 97.5f, lane = 3 },

new NoteBeat { beat = 98f, lane = 1 },
new NoteBeat { beat = 98f, lane = 0 },
new NoteBeat { beat = 98.75f, lane = 2 },
new NoteBeat { beat = 99f, lane = 3 },
new NoteBeat { beat = 99.5f, lane = 1 },

new NoteBeat { beat = 100f, lane = 2 },
new NoteBeat { beat = 100.25f, lane = 3 },
new NoteBeat { beat = 100.5f, lane = 0 },
new NoteBeat { beat = 101f, lane = 1 },
new NoteBeat { beat = 101f, lane = 2 },

new NoteBeat { beat = 102f, lane = 3 },
new NoteBeat { beat = 102.5f, lane = 1 },
new NoteBeat { beat = 103f, lane = 0 },
new NoteBeat { beat = 103.25f, lane = 2 },
new NoteBeat { beat = 103.5f, lane = 3 },

new NoteBeat { beat = 104f, lane = 1 },
new NoteBeat { beat = 104f, lane = 3 },
new NoteBeat { beat = 104.75f, lane = 0 },
new NoteBeat { beat = 105f, lane = 2 },
new NoteBeat { beat = 105.5f, lane = 1 },

new NoteBeat { beat = 106f, lane = 3 },
new NoteBeat { beat = 106.25f, lane = 2 },
new NoteBeat { beat = 106.5f, lane = 0 },
new NoteBeat { beat = 107f, lane = 1 },
new NoteBeat { beat = 107f, lane = 3 },

new NoteBeat { beat = 108f, lane = 2 },
new NoteBeat { beat = 108.5f, lane = 0 },
new NoteBeat { beat = 109f, lane = 1 },
new NoteBeat { beat = 109.25f, lane = 2 },
new NoteBeat { beat = 109.5f, lane = 3 },

new NoteBeat { beat = 110f, lane = 0 },
new NoteBeat { beat = 110f, lane = 2 },
new NoteBeat { beat = 110.75f, lane = 1 },
new NoteBeat { beat = 111f, lane = 3 },
new NoteBeat { beat = 111.5f, lane = 0 },

new NoteBeat { beat = 112f, lane = 2 },
new NoteBeat { beat = 112.25f, lane = 1 },
new NoteBeat { beat = 112.5f, lane = 3 },
new NoteBeat { beat = 113f, lane = 0 },
new NoteBeat { beat = 113f, lane = 1 },

new NoteBeat { beat = 114f, lane = 2 },
new NoteBeat { beat = 114.5f, lane = 3 },
new NoteBeat { beat = 115f, lane = 1 },
new NoteBeat { beat = 115.25f, lane = 0 },
new NoteBeat { beat = 115.5f, lane = 2 },

new NoteBeat { beat = 116f, lane = 3 },
new NoteBeat { beat = 116f, lane = 0 },
new NoteBeat { beat = 116.75f, lane = 1 },
new NoteBeat { beat = 117f, lane = 2 },
new NoteBeat { beat = 117.5f, lane = 3 },

new NoteBeat { beat = 118f, lane = 0 },
new NoteBeat { beat = 118.25f, lane = 1 },
new NoteBeat { beat = 118.5f, lane = 2 },
new NoteBeat { beat = 119f, lane = 3 },
new NoteBeat { beat = 119f, lane = 1 },

new NoteBeat { beat = 120f, lane = 2 },
new NoteBeat { beat = 120.5f, lane = 0 },
new NoteBeat { beat = 121f, lane = 3 },
new NoteBeat { beat = 121.25f, lane = 1 },
new NoteBeat { beat = 121.5f, lane = 2 },

new NoteBeat { beat = 122f, lane = 0 },
new NoteBeat { beat = 122f, lane = 3 },
new NoteBeat { beat = 122.75f, lane = 2 },
new NoteBeat { beat = 123f, lane = 1 },
new NoteBeat { beat = 123.5f, lane = 0 },

new NoteBeat { beat = 124f, lane = 2 },
new NoteBeat { beat = 124.25f, lane = 3 },
new NoteBeat { beat = 124.5f, lane = 1 },
new NoteBeat { beat = 125f, lane = 0 },
new NoteBeat { beat = 125f, lane = 2 },

new NoteBeat { beat = 126f, lane = 3 },
new NoteBeat { beat = 126.5f, lane = 1 },
new NoteBeat { beat = 127f, lane = 2 },
new NoteBeat { beat = 127.25f, lane = 0 },
new NoteBeat { beat = 127.5f, lane = 3 },

new NoteBeat { beat = 128f, lane = 1 },
new NoteBeat { beat = 128f, lane = 2 },
new NoteBeat { beat = 128.75f, lane = 0 },
new NoteBeat { beat = 129f, lane = 3 },
new NoteBeat { beat = 129.5f, lane = 1 },

new NoteBeat { beat = 130f, lane = 2 },
new NoteBeat { beat = 130.25f, lane = 3 },
new NoteBeat { beat = 130.5f, lane = 0 },
new NoteBeat { beat = 131f, lane = 1 },
new NoteBeat { beat = 131f, lane = 3 },

new NoteBeat { beat = 132f, lane = 2 },
new NoteBeat { beat = 132.5f, lane = 1 },
new NoteBeat { beat = 133f, lane = 0 },
new NoteBeat { beat = 133.25f, lane = 2 },
new NoteBeat { beat = 133.5f, lane = 3 },

new NoteBeat { beat = 134f, lane = 1 },
new NoteBeat { beat = 134f, lane = 2 },
new NoteBeat { beat = 134.75f, lane = 3 },
new NoteBeat { beat = 135f, lane = 0 },
new NoteBeat { beat = 135.5f, lane = 1 },

new NoteBeat { beat = 136f, lane = 2 },
new NoteBeat { beat = 136.25f, lane = 3 },
new NoteBeat { beat = 136.5f, lane = 0 },
new NoteBeat { beat = 137f, lane = 1 },
new NoteBeat { beat = 137f, lane = 3 },

new NoteBeat { beat = 138f, lane = 2 },

// ==========================
// MINUTO 3 (139–207)
// Media-Alta → Media → Cierre lento
// ==========================
/*
// --- 139–160: media-alta, aún con movimiento ---
new NoteBeat { beat = 139f, lane = 1 },
new NoteBeat { beat = 140f, lane = 2 },
new NoteBeat { beat = 140.5f, lane = 3 },
new NoteBeat { beat = 141f, lane = 0 },
new NoteBeat { beat = 142f, lane = 2 },
new NoteBeat { beat = 143f, lane = 1 },
new NoteBeat { beat = 144f, lane = 3 },
new NoteBeat { beat = 145f, lane = 0 },
new NoteBeat { beat = 146f, lane = 2 },
new NoteBeat { beat = 147f, lane = 1 },
new NoteBeat { beat = 148f, lane = 3 },
new NoteBeat { beat = 149f, lane = 2 },
new NoteBeat { beat = 150f, lane = 0 },
new NoteBeat { beat = 151f, lane = 1 },
new NoteBeat { beat = 152f, lane = 2 },
new NoteBeat { beat = 153f, lane = 3 },
new NoteBeat { beat = 154f, lane = 1 },
new NoteBeat { beat = 155f, lane = 0 },
new NoteBeat { beat = 156f, lane = 2 },
new NoteBeat { beat = 157f, lane = 3 },
new NoteBeat { beat = 158f, lane = 1 },
new NoteBeat { beat = 159f, lane = 2 },
new NoteBeat { beat = 160f, lane = 0 },

// --- 161–180: media, ritmo más respirable ---
new NoteBeat { beat = 161f, lane = 3 },
new NoteBeat { beat = 163f, lane = 1 },
new NoteBeat { beat = 164f, lane = 2 },
new NoteBeat { beat = 166f, lane = 0 },
new NoteBeat { beat = 167f, lane = 3 },
new NoteBeat { beat = 169f, lane = 1 },
new NoteBeat { beat = 170f, lane = 2 },
new NoteBeat { beat = 172f, lane = 0 },
new NoteBeat { beat = 173f, lane = 3 },
new NoteBeat { beat = 175f, lane = 1 },
new NoteBeat { beat = 176f, lane = 2 },
new NoteBeat { beat = 178f, lane = 0 },
new NoteBeat { beat = 179f, lane = 3 },
new NoteBeat { beat = 180f, lane = 1 },

// --- 181–195: transición al cierre ---
new NoteBeat { beat = 182f, lane = 2 },
new NoteBeat { beat = 184f, lane = 0 },
new NoteBeat { beat = 186f, lane = 3 },
new NoteBeat { beat = 188f, lane = 1 },
new NoteBeat { beat = 190f, lane = 2 },
new NoteBeat { beat = 192f, lane = 0 },
new NoteBeat { beat = 194f, lane = 3 },
new NoteBeat { beat = 195f, lane = 1 },

// --- 196–207: CIERRE LENTO ---
new NoteBeat { beat = 196f, lane = 2 },
new NoteBeat { beat = 197f, lane = 0 },
new NoteBeat { beat = 198f, lane = 3 },
new NoteBeat { beat = 199f, lane = 1 },
new NoteBeat { beat = 200f, lane = 2 },
new NoteBeat { beat = 201f, lane = 0 },
new NoteBeat { beat = 202f, lane = 3 },

new NoteBeat { beat = 203f, lane = 2 },
new NoteBeat { beat = 204f, lane = 0 },
new NoteBeat { beat = 205f, lane = 3 },
new NoteBeat { beat = 206f, lane = 1 },
new NoteBeat { beat = 206f, lane = 2 },
new NoteBeat { beat = 207f, lane = 0 },
new NoteBeat { beat = 207f, lane = 3 }, // ÚLTIMA NOTA

*/


        };
        //notes.Sort((a, b) => a.beat.CompareTo(b.beat));
    }

    public void Flash()
    {
        StartCoroutine(FlashRoutine());
    }
    IEnumerator FlashRoutine() //Efecto flash para altas puntuaciones
    {
        flashImage.color = new Color(1,1,1,0.6f);
        yield return new WaitForSeconds(0.05f);
        flashImage.color = new Color(1,1,1,0f);
    }

    IEnumerator FadeOutMusic()
    {
        float startVolume = musicSource.volume;
        float t = 0f;

        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            musicSource.volume = Mathf.Lerp(startVolume, 0f, t / fadeOutDuration);
            yield return null;
        }

        musicSource.volume = 0f;
        musicSource.Stop();
        //Aqui se finaliza la partida o ronda. Se muestran resultados o se va a pantalla de inicio
        ShowResults();
    }


}

