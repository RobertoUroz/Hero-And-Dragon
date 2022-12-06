using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARSubsystems;

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

        public Animator dragonAnimator;

        private bool first = true;

        private bool dragonSpawned = false;
        private bool heroSpawned = false;

        public void SelectCharacter(string character)
        {
            if (StaticClass.characterSelected.Equals(character))
            {
                StaticClass.characterSelected = "";
            }
            else
            {
                StaticClass.characterSelected = character;
            }
        }

        public void RemoveAllAnchors()
        {
            dragonSpawned = false;
            heroSpawned = false;
            prefabDragon.transform.localScale = Vector3.zero;
            prefabHero.transform.localScale = Vector3.zero;
            StaticClass.characterSelected = "";
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
                        dragonSpawned = true;
                    }
                    else if (StaticClass.characterSelected.Equals("Hero"))
                    {
                        prefabHero.transform.SetParent(anchor.transform);
                        prefabHero.transform.localScale = Vector3.one;
                        prefabHero.transform.localPosition = Vector3.zero;
                        heroSpawned = true;
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
                if ((StaticClass.characterSelected.Equals("Dragon") && !dragonSpawned) || (StaticClass.characterSelected.Equals("Hero") && !heroSpawned))
                {
                    var anchor = CreateAnchor(hit);
                }
            }
        }

        private bool interactWithCharacters()
        {
            if (dragonSpawned && StaticClass.characterSelected.Equals("Dragon"))
            {
                return true;
            }
            else if (heroSpawned && StaticClass.characterSelected.Equals("Hero"))
            {
                Logger.Log("setting condition to true.");
                Logger.Log(heroAnimator.ToString());

                heroAnimator.SetBool("ClickHero", true);

                heroAnimator.SetBool("ClickHero", false);
                return true;
            }
            return false;
        }

        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

        ARRaycastManager m_RaycastManager;

        ARAnchorManager m_AnchorManager;
    }
}
