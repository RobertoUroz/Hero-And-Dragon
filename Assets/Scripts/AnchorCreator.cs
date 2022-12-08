using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

namespace UnityEngine.XR.ARFoundation.Samples
{
    [RequireComponent(typeof(ARAnchorManager))]
    [RequireComponent(typeof(ARRaycastManager))]
    public class AnchorCreator : MonoBehaviour
    {
        [SerializeField]
        GameObject m_Prefab_Hero;

        public GameObject prefabHero
        {
            get => m_Prefab_Hero;
            set => m_Prefab_Hero = value;
        }
        public Animator heroAnimator;

        [SerializeField]
        GameObject m_Prefab_Dragon;

        public GameObject prefabDragon
        {
            get => m_Prefab_Dragon;
            set => m_Prefab_Dragon = value;
        }

        public GameObject scaleAndRotationSystem;

        public GameObject heroAttackButton;

        public GameObject dragonAttackButton;

        public Slider scaleSlider;

        public Slider rotationSlider;

        public Text characterSelectedText;

        public void SelectCharacter(string character)
        {
            if (StaticClass.characterSelected.Equals(character))
            {
                StaticClass.characterSelected = "";
                characterSelectedText.text = "";
                scaleAndRotationSystem.SetActive(false);
            }
            else
            {
                StaticClass.characterSelected = character;
                characterSelectedText.text = "Character Selected: " + character;
                if ((character.Equals("Hero") && StaticClass.heroSpawned) || (character.Equals("Dragon") && StaticClass.dragonSpawned))
                {
                    if (character.Equals("Hero"))
                    {
                        rotationSlider.value = prefabHero.transform.localRotation.eulerAngles.y;
                        scaleSlider.value = prefabHero.transform.localScale.x;
                    }
                    else if (character.Equals("Dragon"))
                    {
                        rotationSlider.value = prefabDragon.transform.localRotation.eulerAngles.y;
                        scaleSlider.value = prefabDragon.transform.localScale.x;
                    }
                    scaleAndRotationSystem.SetActive(true);
                }
            }
        }

        public void RemoveAllAnchors()
        {
            StaticClass.dragonSpawned = false;
            StaticClass.heroSpawned = false;
            scaleAndRotationSystem.SetActive(false);
            characterSelectedText.text = "";
            heroAttackButton.SetActive(false);
            dragonAttackButton.SetActive(false);
            prefabDragon.transform.localScale = Vector3.zero;
            prefabHero.transform.localScale = Vector3.zero;
            StaticClass.characterSelected = "";
        }

        public void VictoryOfCharacter(string character)
        {
            scaleAndRotationSystem.SetActive(false);
            characterSelectedText.text = "";
            heroAttackButton.SetActive(false);
            dragonAttackButton.SetActive(false);           
            StaticClass.characterSelected = "";
            if (character.Equals("Dragon")) {
                StaticClass.heroSpawned = false;
            } else if (character.Equals("Hero"))
            {
                StaticClass.dragonSpawned = false;
            }
        }

        void Awake()
        {
            m_RaycastManager = GetComponent<ARRaycastManager>();
            m_AnchorManager = GetComponent<ARAnchorManager>();
        }

        ARAnchor CreateAnchor(in ARRaycastHit hit)
        {
            ARAnchor anchor = null;
            if (hit.trackable is ARPlane plane)
            {
                // If we hit a plane, try to "attach" the anchor to the plane
                var planeManager = GetComponent<ARPlaneManager>();
                if (planeManager)
                {
                    Logger.Log("Creating anchor attachment.");
                    anchor = m_AnchorManager.AttachAnchor(plane, hit.pose);
                    if (StaticClass.characterSelected.Equals("Dragon"))
                    {
                        prefabDragon.transform.SetParent(anchor.transform);
                        prefabDragon.transform.localScale = Vector3.one;
                        prefabDragon.transform.localPosition = Vector3.zero;
                        StaticClass.dragonSpawned = true;
                        scaleAndRotationSystem.SetActive(true);
                        if (StaticClass.heroSpawned && StaticClass.dragonSpawned)
                        {
                            dragonAttackButton.SetActive(true);
                            heroAttackButton.SetActive(true);
                        }
                    }
                    else if (StaticClass.characterSelected.Equals("Hero"))
                    {
                        prefabHero.transform.SetParent(anchor.transform);
                        prefabHero.transform.localScale = Vector3.one;
                        prefabHero.transform.localPosition = Vector3.zero;
                        StaticClass.heroSpawned = true;
                        scaleAndRotationSystem.SetActive(true);
                        if (StaticClass.heroSpawned && StaticClass.dragonSpawned)
                        {
                            dragonAttackButton.SetActive(true);
                            heroAttackButton.SetActive(true);
                        }
                    }
                    return anchor;
                }
            }

            return anchor;
        }

        void Update()
        {
            if (Input.touchCount == 0)
                return;

            var touch = Input.GetTouch(0);
            if (touch.phase != TouchPhase.Began)
                return;
            
            //interaction with the pets when clicking
            if (interactWithCharacters())
                return;

            // Raycast against planes and feature points
            const TrackableType trackableTypes =
                TrackableType.FeaturePoint |
                TrackableType.PlaneWithinPolygon;

            // Perform the raycast
            if (m_RaycastManager.Raycast(touch.position, s_Hits, trackableTypes))
            {
                // Raycast hits are sorted by distance, so the first one will be the closest hit.
                var hit = s_Hits[0];

                // Create a new anchor only if there is a pet selected
                Logger.Log(StaticClass.characterSelected);
                if ((StaticClass.characterSelected.Equals("Dragon") && !StaticClass.dragonSpawned) 
                    || (StaticClass.characterSelected.Equals("Hero") && !StaticClass.heroSpawned))
                {
                    var anchor = CreateAnchor(hit);
                }
            }
        }

        private bool interactWithCharacters()
        {
            if (StaticClass.dragonSpawned && StaticClass.characterSelected.Equals("Dragon"))
            {
                return true;
            }
            else if (StaticClass.heroSpawned && StaticClass.characterSelected.Equals("Hero"))
            {
                return true;
            }
            return false;
        }

        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

        ARRaycastManager m_RaycastManager;

        ARAnchorManager m_AnchorManager;
    }
}
