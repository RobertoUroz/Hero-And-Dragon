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
        private Animator anchor_heroAnimator;

        [SerializeField]
        GameObject m_Prefab_Dragon;

        public GameObject prefabDragon
        {
            get => m_Prefab_Dragon;
            set => m_Prefab_Dragon = value;
        }

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
            Logger.Log($"Removing all anchors ({m_Anchors.Count})");
            foreach (var anchor in m_Anchors)
            {
                Destroy(anchor.gameObject);
            }
            m_Anchors.Clear();
            dragonSpawned = false;
            heroSpawned = false;
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
            GameObject prefab;
            if (StaticClass.characterSelected.Equals("Dragon"))
            {
                prefab = prefabDragon;
                dragonSpawned = true;
            }
            else
            {
                prefab = prefabHero;
                heroSpawned = true;
            }

            // If we hit a plane, try to "attach" the anchor to the plane
            if (hit.trackable is ARPlane plane)
            {
                var planeManager = GetComponent<ARPlaneManager>();
                if (planeManager)
                {
                    Logger.Log("Creating anchor attachment.");
                    var oldPrefab = m_AnchorManager.anchorPrefab;
                    m_AnchorManager.anchorPrefab = prefab;
                    anchor = m_AnchorManager.AttachAnchor(plane, hit.pose);
                    m_AnchorManager.anchorPrefab = oldPrefab;
                    return anchor;
                }
            }

            // Otherwise, just create a regular anchor at the hit pose
            Logger.Log("Creating regular anchor.");

            // Note: the anchor can be anywhere in the scene hierarchy
            var gameObject = Instantiate(prefab, hit.pose.position, hit.pose.rotation);
            if (StaticClass.characterSelected.Equals("Hero"))
                anchor_heroAnimator = gameObject.GetComponent<Animator>();

            // Make sure the new GameObject has an ARAnchor component
            anchor = gameObject.GetComponent<ARAnchor>();
            if (anchor == null)
            {
                anchor = gameObject.AddComponent<ARAnchor>();
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
                    if (anchor)
                    {
                        // Remember the anchor so we can remove it later.
                        m_Anchors.Add(anchor);
                    }
                    else
                    {
                        Logger.Log("Error creating anchor");
                    }   
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
                Logger.Log(anchor_heroAnimator.ToString());

                anchor_heroAnimator.SetBool("ClickHero", true);

                anchor_heroAnimator.SetBool("ClickHero", false);
                return true;
            }
            return false;
        }

        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

        List<ARAnchor> m_Anchors = new List<ARAnchor>();

        ARRaycastManager m_RaycastManager;

        ARAnchorManager m_AnchorManager;
    }
}
