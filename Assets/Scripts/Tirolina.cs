using UnityEngine;

public class Tirolina : MonoBehaviour
{
    public GameObject leftObject;
    public GameObject rightObject;

    public Transform GetLeftObject()
    {
        return leftObject.transform;
    }

    public Transform GetRightObject()
    {
        return rightObject.transform;
    }
}
