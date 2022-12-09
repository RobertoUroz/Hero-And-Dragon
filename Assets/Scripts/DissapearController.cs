using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissapearController : MonoBehaviour
{

    private bool canDissapear = false;
    private bool dissapearing = false;

    public float timeToWaitForDissapearing;

    private float timeWaiting = 0.0f;

    public float framesScalingDown;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (canDissapear && !dissapearing)
        {
            timeWaiting += Time.deltaTime;
            if (timeWaiting >= timeToWaitForDissapearing)
            {
                dissapearing = true;
            }
        }
        if (dissapearing)
        {
            gameObject.transform.localScale = Vector3.one * Mathf.Max(0, gameObject.transform.localScale.x - (Time.deltaTime / framesScalingDown));
        }
        if (gameObject.transform.localScale.x == 0)
        {
            Animator animator = gameObject.GetComponent(typeof(Animator)) as Animator;
            animator.SetTrigger("Deleted");
            canDissapear = false;
            dissapearing = false;
        }
    }

    void Dissapear()
    {
        canDissapear = true;
    }
}
