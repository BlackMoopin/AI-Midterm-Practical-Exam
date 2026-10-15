using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Summary: Runs this civilian to the nearest Cover Spot and rations panic while hiding from the villain.

// Instructions: Setup necessary variables in the inspector.
//               Your animation parameter code goes inside StudentUpdate at the bottom
//               do not edit the rest of this script apart from StudentUpdate()


public class HideBehavior : StateMachineBehaviour {

    private CivilianBrain brain;
    private Transform nearestCover;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        brain = animator.GetComponent<CivilianBrain>();
        nearestCover = FindNearestCover(animator.transform.position);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (nearestCover == null) {
            return;
        }

        Vector2 pos = animator.transform.position;
        Vector2 next = Vector2.MoveTowards(pos, nearestCover.position, brain.panicSpeed * Time.deltaTime);
        animator.transform.position = next;

        if (brain.spriteRenderer != null) {
            brain.spriteRenderer.flipX = next.x < pos.x;
        }

        StudentUpdate(animator);
    }

    private Transform FindNearestCover(Vector2 from) {
        Transform best = null;
        float bestDist = float.MaxValue;
        for (int i = 0; i < brain.coverSpots.Length; i++) {
            float d = Vector2.Distance(from, brain.coverSpots[i].position);
            if (d < bestDist) {
                bestDist = d;
                best = brain.coverSpots[i];
            }
        }
        return best;
    }

    // INPUT YOUR CODE HERE - write your own animation parameter calls for this state.
    // Example: animator.SetBool("isHiding", true);
    // Use SetBool or SetInteger. Do not use SetTrigger here - this runs every frame.
    private void StudentUpdate(Animator animator) {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    }
}
