using UnityEngine;

public class EnemyTurtle : EnemyBase
{
    [Header("Turtle")]
    public GameObject pinchito;
    public Transform bulletPointRight;
    public Transform bulletPointLeft;
    public bool right = false;

    protected override void Start()
    {
        base.Start();

        spr.flipX = !right;
    }

    public void OnShootAnimation()
    {
        if (!initialized) return;

        Vector3 spawnPos = (right ? bulletPointRight.position : bulletPointLeft.position);
        GameObject spike = Instantiate(pinchito, spawnPos, Quaternion.identity);
        Spike spikeScript = spike.GetComponent<Spike>();
        spikeScript.InitSpike(right);
    }
}
