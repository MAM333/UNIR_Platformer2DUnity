using UnityEngine;

public class MainMenuAnimation : MonoBehaviour
{
    [Header("Background")]
    [SerializeField] float speedXBg;
    [SerializeField] float minXBg;
    [SerializeField] float maxXBg;
    [SerializeField] Transform bg1;
    [SerializeField] Transform bg2;
    [SerializeField] Transform bg3;
    [SerializeField] Transform bg4;
    [SerializeField] Transform bg5;

    [Header("Gameplay")]
    [SerializeField] float speedXGm;
    float minXGm;
    float maxXGm;
    [SerializeField] Transform ground1;
    [SerializeField] Transform ground2;
    [SerializeField] Transform ground3;
    [SerializeField] Transform ground4;
    [SerializeField] Transform ground5hide;


    [Header("Foreground")]
    [SerializeField] float speedXFr;
    [SerializeField] float minXFr;
    [SerializeField] float maxXFr;
    [SerializeField] Transform decoration1;
    [SerializeField] Transform decoration2;
    [SerializeField] Transform decoration3;

    private void Start()
    {
        minXGm = ground1.localPosition.x;
        maxXGm = ground5hide.localPosition.x;
    }

    private void Update()
    {
        MoveBackground();
        MoveGameplay();
        MoveForeground();
    }

    private void MoveThing(Transform thing, float speed, float minX, float maxX)
    {
        Vector3 vector = thing.localPosition;
        vector.x -= speed * Time.deltaTime;
        if (vector.x < minX) vector.x = maxX - (minX - vector.x);
        thing.localPosition = vector;
    }

    private void MoveBackground()
    {
        MoveThing(bg1, speedXBg, minXBg, maxXBg);
        MoveThing(bg2, speedXBg, minXBg, maxXBg);
        MoveThing(bg3, speedXBg, minXBg, maxXBg);
        MoveThing(bg4, speedXBg, minXBg, maxXBg);
        MoveThing(bg5, speedXBg, minXBg, maxXBg);
    }

    private void MoveGameplay()
    {
        MoveThing(ground1, speedXGm, minXGm, maxXGm);
        MoveThing(ground2, speedXGm, minXGm, maxXGm);
        MoveThing(ground3, speedXGm, minXGm, maxXGm);
        MoveThing(ground4, speedXGm, minXGm, maxXGm);
    }

    private void MoveForeground()
    {
        MoveThing(decoration1, speedXFr, minXFr, maxXFr);
        MoveThing(decoration2, speedXFr, minXFr, maxXFr);
        MoveThing(decoration3, speedXFr, minXFr, maxXFr);
    }
}
