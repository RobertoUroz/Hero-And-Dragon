using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEngine.XR.ARFoundation.Samples
{
    public class AttackController : MonoBehaviour
    {

        public string character;

        public Animator heroAnimator;

        public Animator dragonAnimator;

        public GameObject thisObject;

        private int counterHits = 1;

        //Total hits to defeat foe
        public int totalHits;

        public void attack()
        {
            if (character == "Hero")
            {
                if (true) //TODO: Make something that stops the user from glitching the animations
                {
                    Logger.Log(character + ": " + counterHits);
                    counterHits = counterHits + 1;
                    dragonAnimator.SetTrigger("Hit");
                    heroAnimator.SetTrigger("Attack");
                    if (counterHits > totalHits)
                    {
                        heroAnimator.SetTrigger("Victory");
                        dragonAnimator.SetTrigger("Die");
                        counterHits = 1;
                    }
                }
            } 
            else if (character == "Dragon")
            {
                if (true) //TODO: Make something that stops the user from glitching the animations
                {
                    Logger.Log(character + ": " + counterHits);
                    counterHits = counterHits + 1;
                    heroAnimator.SetTrigger("Hit");
                    dragonAnimator.SetTrigger("Attack");
                    if (counterHits > totalHits)
                    {
                        heroAnimator.SetTrigger("Die");
                        counterHits = 1;
                    }
                }
            }

        }
    }

}