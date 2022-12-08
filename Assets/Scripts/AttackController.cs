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

        public float timeBetweenHits;

        private float timePassed = 0.0f;

        //Total hits to defeat foe
        public int totalHits;

        public GameObject heroPrefab;

        public GameObject dragonPrefab;

        public GameObject otherAttackButton;

        public GameObject sessionAR;

        public void attack()
        {
            if (timePassed >= timeBetweenHits)
            {
                otherAttackButton.SendMessage("WaitForAnimation", timeBetweenHits);
                this.WaitForAnimation(timeBetweenHits);
                if (character == "Hero")
                {    
                    Logger.Log(character + ": " + counterHits);
                    counterHits = counterHits + 1;
                    dragonAnimator.SetTrigger("Hit");
                    heroAnimator.SetTrigger("Attack");
                    if (counterHits > totalHits)
                    {
                        dragonAnimator.SetTrigger("Die");
                        dragonPrefab.SendMessage("Dissapear");
                        heroAnimator.SetTrigger("Victory");
                        counterHits = 1;
                        otherAttackButton.SendMessage("SetCounterHits", 1);
                        sessionAR.SendMessage("VictoryOfCharacter", "Hero");
                    }
                }
                else if (character == "Dragon")
                {
                    Logger.Log(character + ": " + counterHits);
                    counterHits = counterHits + 1;
                    heroAnimator.SetTrigger("Hit");
                    dragonAnimator.SetTrigger("Attack");
                    if (counterHits > totalHits)
                    {
                        heroAnimator.SetTrigger("Die");
                        heroPrefab.SendMessage("Dissapear");
                        dragonAnimator.SetTrigger("Victory");
                        counterHits = 1;
                        otherAttackButton.SendMessage("SetCounterHits", 1);
                        sessionAR.SendMessage("VictoryOfCharacter", "Dragon");
                    }
                }
            }
        }

        public void Update()
        {
            timePassed += Time.deltaTime;
            timePassed = Mathf.Min(timeBetweenHits, timePassed);
        }

        public void WaitForAnimation(float timeWaiting)
        {
            timePassed -= timeWaiting;
        }

        public void SetCounterHits(int hits)
        {
            counterHits = 1;
        }

    }

}