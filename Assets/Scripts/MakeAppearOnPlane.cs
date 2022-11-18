using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

namespace UnityEngine.XR.ARFoundation.Samples
{
    /// <summary>
    /// Moves the ARSessionOrigin in such a way that it makes the given content appear to be
    /// at a given location acquired via a raycast.
    /// </summary>
    [RequireComponent(typeof(ARSessionOrigin))]
    [RequireComponent(typeof(ARRaycastManager))]
    public class MakeAppearOnPlane : MonoBehaviour
    {
        [SerializeField]
        [Tooltip("A transform which should be made to appear to be at the touch point.")]
        Transform m_Content_Dog;

        /// <summary>
        /// A transform which should be made to appear to be at the touch point.
        /// </summary>
        public Transform contentDog
        {
            get { return m_Content_Dog; }
            set { m_Content_Dog = value; }
        }

        [SerializeField]
        [Tooltip("A transform which should be made to appear to be at the touch point.")]
        Transform m_Content_Cat;

        /// <summary>
        /// A transform which should be made to appear to be at the touch point.
        /// </summary>
        public Transform contentCat
        {
            get { return m_Content_Cat; }
            set { m_Content_Cat = value; }
        }

        // bool to switch between dog and cat
        private bool isCat = true;

        [SerializeField]
        [Tooltip("The rotation the content should appear to have.")]
        Quaternion m_Rotation;

        /// <summary>
        /// The rotation the content should appear to have.
        /// </summary>
        public Quaternion rotation
        {
            get { return m_Rotation; }
            set
            {
                m_Rotation = value;
                if (m_SessionOrigin != null)
                {
                    if (isCat)
                        m_SessionOrigin.MakeContentAppearAt(contentCat, contentCat.transform.position, m_Rotation);
                    else
                        m_SessionOrigin.MakeContentAppearAt(contentDog, contentDog.transform.position, m_Rotation);
                }
            }
        }

        void Awake()
        {
            m_SessionOrigin = GetComponent<ARSessionOrigin>();
            m_RaycastManager = GetComponent<ARRaycastManager>();
        }

        void Update()
        {
            if (Input.touchCount == 0 || m_Content_Dog == null || m_Content_Cat == null)
                return;

            var touch = Input.GetTouch(0);

            if (m_RaycastManager.Raycast(touch.position, s_Hits, TrackableType.PlaneWithinPolygon))
            {
                // Raycast hits are sorted by distance, so the first one
                // will be the closest hit.
                var hitPose = s_Hits[0].pose;

                // This does not move the content; instead, it moves and orients the ARSessionOrigin
                // such that the content appears to be at the raycast hit position.
                if (isCat)
                {
                    m_SessionOrigin.MakeContentAppearAt(contentCat, hitPose.position, m_Rotation);
                } 
                else 
                {
                    m_SessionOrigin.MakeContentAppearAt(contentDog, hitPose.position, m_Rotation);
                }
            }
        }

        static List<ARRaycastHit> s_Hits = new List<ARRaycastHit>();

        ARSessionOrigin m_SessionOrigin;

        ARRaycastManager m_RaycastManager;
    }
}