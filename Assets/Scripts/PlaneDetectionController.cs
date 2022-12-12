using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

namespace UnityEngine.XR.ARFoundation.Samples
{
    /// <summary>
    /// This example demonstrates how to toggle plane detection,
    /// and also hide or show the existing planes.
    /// </summary>
    [RequireComponent(typeof(ARPlaneManager))]
    public class PlaneDetectionController : MonoBehaviour
    {
        
        private bool planeVisualizationEnabled;

        public GameObject planePrefab;

        /// <summary>
        /// Toggles plane visualization of the planes.
        /// </summary>
        public void TogglePlaneVisualization()
        {
            planeVisualizationEnabled = !planeVisualizationEnabled;
            SetAllPlanesActive();
        }

        private void SetAllPlanesActive()
        {
            foreach (var plane in m_ARPlaneManager.trackables)
                plane.gameObject.SetActive(planeVisualizationEnabled);
        }


        void Awake()
        {
            planeVisualizationEnabled = true;
            m_ARPlaneManager = GetComponent<ARPlaneManager>();
            TogglePlaneVisualization();
        }

        ARPlaneManager m_ARPlaneManager;
    }
}