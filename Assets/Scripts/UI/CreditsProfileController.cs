using UnityEngine;

public class CreditsProfileController : MonoBehaviour
{
    [SerializeField] private string link;

    public void GoToLink()
    {
        Application.OpenURL(link);
    }
}
