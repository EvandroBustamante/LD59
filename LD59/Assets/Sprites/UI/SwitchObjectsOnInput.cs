using UnityEngine;

public class SwitchObjectsOnInput : MonoBehaviour
{
    public GameObject objetoParaDesativar;
    public GameObject objetoParaAtivar;

    private bool jaTrocou = false;

    private void Start()
    {
        AudioManager.Instance.StopMusicMenu();
    }

    void Update()
    {
       
        if (Input.anyKeyDown && !jaTrocou)
        {
            if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1) || Input.GetMouseButtonDown(2))
                return;

            objetoParaDesativar.SetActive(false);
            objetoParaAtivar.SetActive(true);

            jaTrocou = true;
        }
    }
}
