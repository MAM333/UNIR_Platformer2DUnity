using UnityEngine;

public class AIControl : MonoBehaviour
{
    [SerializeField] float attackDistance = 0.25f;
    [SerializeField] Transform target;
    CharacterController2D characterController;


    private void Awake()
    {
        characterController = GetComponent<CharacterController2D>();
    }

    void Update()
    {
        Vector2 rawMove = Vector2.zero;
        if (target) 
        { 
            if (transform.position.x > target.position.x) rawMove = Vector2.left; 
            else rawMove = Vector2.right;

            if (Mathf.Abs(target.transform.position.x - transform.position.x) < attackDistance)
            {
                rawMove = Vector2.zero;
                characterController.Punch();
            }
        } 

        characterController.SetRawMove(rawMove);    
    }

    public void SetTarget(Transform tg)
    {
        target = tg;
    }
}
