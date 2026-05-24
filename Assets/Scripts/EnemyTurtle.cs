using UnityEngine;

public class EnemyTurtle : EnemyBase
{
    [Header("Turtle")]
    public GameObject pinchito;
    public Transform bulletPointRight;
    public Transform bulletPointLeft;
    public bool right = false;

    private void Start()
    {
        spr.flipX = !right;
    }

    public void OnShootAnimation()
    {
        Vector3 spawnPos = (right ? bulletPointRight.position : bulletPointLeft.position);
        GameObject spike = Instantiate(pinchito, spawnPos, Quaternion.identity);
        Spike spikeScript = spike.GetComponent<Spike>();
        spikeScript.InitSpike(right);
    }
}
