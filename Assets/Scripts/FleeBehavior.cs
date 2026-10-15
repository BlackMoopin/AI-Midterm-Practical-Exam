using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Summary: Makes this civilian sprint straight away from the villain while the villain stays close.

// Instructions: Setup necessary variables in the inspector.
//               Your animation parameter code goes inside StudentUpdate at the bottom
//               do not edit the rest of this script apart from StudentUpdate()


public class FleeBehavior : StateMachineBehaviour {

    private CivilianBrain brain;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        brain = animator.GetComponent<CivilianBrain>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (brain.threat == null) {
            return;
        }

        Vector2 pos = animator.transform.position;
        Vector2 away = (pos - (Vector2)brain.threat.position).normalized;
        Vector2 next = pos + away * brain.panicSpeed * Time.deltaTime;
        animator.transform.position = next;

        if (brain.spriteRenderer != null) {
            brain.spriteRenderer.flipX = next.x < pos.x;
        }

        StudentUpdate(animator);
    }

    // INPUT YOUR - write your own animation parameter calls for this state.
    // Example: animator.SetBool("isRunning", true);
    // Use SetBool or SetInteger. Do not use SetTrigger here - this runs every frame.
    private void StudentUpdate(Animator animator) {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    }
}
