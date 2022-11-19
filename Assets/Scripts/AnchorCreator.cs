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
        GameObject m_Prefab_Dog;

        public GameObject prefabDog
        {
            get => m_Prefab_Dog;
            set => m_Prefab_Dog = value;
        }
        public Animator dogAnimator;
        private Animator anchor_dogAnimator;

        [SerializeField]
        GameObject m_Prefab_Cat;

        public GameObject prefabCat
        {
            get => m_Prefab_Cat;
            set => m_Prefab_Cat = value;
        }
        public Animator catAnimator;

        private string petSelected = "";
        private bool first = true;

        private bool catSpawned = false;
        private bool dogSpawned = false;

        public void SelectPet(string selectionPet)
        {
            if (petSelected.Equals(selectionPet))
            {
                petSelected = "";
            }
            else
            {
                petSelected = selectionPet;
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
            catSpawned = false;
            dogSpawned = false;
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
            if (petSelected.Equals("Cat"))
            {
                prefab = prefabCat;
                catSpawned = true;
            }
            else
            {
                prefab = prefabDog;
                dogSpawned = true;
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
            if (petSelected.Equals("Dog"))
                anchor_dogAnimator = gameObject.GetComponent<Animator>();

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
            if (interactWithPets())
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
                if ((petSelected.Equals("Cat") && !catSpawned) || (petSelected.Equals("Dog") && !dogSpawned))
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

        private bool interactWithPets()
        {
            if (catSpawned && petSelected.Equals("Cat"))
            {
                return true;
            }
            else if (dogSpawned && petSelected.Equals("Dog"))
            {
                Logger.Log("setting condition to true.");
                Logger.Log(dogAnimator.ToString());

                dogAnimator.SetBool("ClickDog", true);
                anchor_dogAnimator.SetBool("ClickDog", true);

                dogAnimator.SetBool("ClickDog", false);
                anchor_dogAnimator.SetBool("ClickDog", false);
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
