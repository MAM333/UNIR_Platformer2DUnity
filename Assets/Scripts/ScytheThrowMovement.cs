using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class ScytheThrowMovement : MonoBehaviour
{
    public UnityEvent finishedMovement;
    public UnityEvent isReturning;

    [SerializeField] float velocity = 3f;
    [SerializeField] float maxRange = 2f;
    [SerializeField] float timeToReturn = 0.4f;

    bool isRight = false;
    public void StartMove(bool right)
    {
        isRight = right;

        StartCoroutine(Movement());
    }

    IEnumerator Movement()
    {
        Vector3 initPos = transform.position;

        Vector3 finalPos = transform.position + Vector3.right * maxRange * (isRight ? 1 : -1);
        while (transform.position != finalPos)
        {
            yield return null;
            transform.position = Vector3.MoveTowards(transform.position, finalPos, velocity * Time.deltaTime);
        }

        yield return new WaitForSeconds(timeToReturn/2);
        isReturning.Invoke();
        yield return new WaitForSeconds(timeToReturn/2);

        finalPos = initPos;
        while (transform.position != finalPos)
        {
            yield return null;
            transform.position = Vector3.MoveTowards(transform.position, finalPos, velocity * Time.deltaTime);
        }

        finishedMovement.Invoke();
        Destroy(gameObject);
    }

    public void FinishMovement()
    {
        StopAllCoroutines();
        finishedMovement.Invoke();
        Destroy(gameObject);
    }
}
