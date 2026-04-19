using UnityEngine;

public class ToggleObject : MonoBehaviour
{
   //public GameObject TargetObject;

    public void toggle(GameObject TargetObject)
    {
        if (TargetObject != null)
        {
            TargetObject.SetActive(!TargetObject.activeSelf);
        }
    }
}
