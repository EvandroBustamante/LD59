using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeSceneInteractable : MonoBehaviour
{
    public int sceneToLoad;

    public void Interact()
    {
        SceneManager.LoadScene(sceneToLoad);
    }
}
