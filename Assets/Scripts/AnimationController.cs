using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    [SerializeField]
    private Animator opponentChoiceAnimator;

    public void ShuffleOpponentChoicesAnimation()
    {
        opponentChoiceAnimator.Play("ShuffleOpponentChoices");
    }
}
