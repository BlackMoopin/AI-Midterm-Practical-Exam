using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Summary: Moves this civilian slowly around its Waypoints while the villain stays far away.

// Instructions: Setup necessary variables in the inspector.
//               Your animation parameter code goes inside StudentUpdate at the bottom
//               do not edit the rest of this script apart from StudentUpdate()


public class WanderBehavior : StateMachineBehaviour {

    private CivilianBrain brain;
    private int waypointIndex;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        brain = animator.GetComponent<CivilianBrain>();
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        if (brain.waypoints.Length == 0) {
            return;
        }

        Transform target = brain.waypoints[waypointIndex];
        Vector2 pos = animator.transform.position;
        Vector2 next = Vector2.MoveTowards(pos, target.position, brain.wanderSpeed * Time.deltaTime);
        animator.transform.position = next;

        if (Vector2.Distance(next, target.position) < 0.1f) {
            waypointIndex = (waypointIndex + 1) % brain.waypoints.Length;
        }

        if (brain.spriteRenderer != null) {
            brain.spriteRenderer.flipX = next.x < pos.x;
        }

        StudentUpdate(animator);
    }

    // INPUT YOUR CODE HERE - write your own animation parameter calls for this state.
    // Example: animator.SetBool("isWalking", true);
    // Use SetBool or SetInteger. Do not use SetTrigger here - this runs every frame.
    private void StudentUpdate(Animator animator) {

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {

    }
}
