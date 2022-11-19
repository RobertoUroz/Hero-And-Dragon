﻿using UnityEngine;
using UnityEngine.UI;

namespace UnityEngine.XR.ARFoundation.Samples
{
    /// <summary>
    /// Controls the orientation of content place by the <see cref="RotationCharacter"/>
    /// component using a UI.Slider to affect the rotation about the Y axis.
    /// </summary>
    [RequireComponent(typeof(RotationCharacter))]
    public class RotationController : MonoBehaviour
    {
        RotationCharacter m_RotationCharacter;
        
        [SerializeField]
        [Tooltip("The slider used to control rotation.")]
        Slider m_Slider;

        /// <summary>
        /// The slider used to control scale.
        /// </summary>
        public Slider slider
        {
            get { return m_Slider; }
            set { m_Slider = value; }
        }

        [SerializeField]
        [Tooltip("The text used to display the current rotation on the screen.")]
        Text m_Text;

        /// <summary>
        /// The text used to display the current rotation on the screen.
        /// </summary>
        public Text text
        {
            get { return m_Text; }
            set { m_Text = value; }
        }

        [SerializeField]
        [Tooltip("Minimum rotation angle in degrees.")]
        public float m_Min = 0f;

        /// <summary>
        /// Minimum angle in degrees.
        /// </summary>
        public float min
        {
            get { return m_Min; }
            set { m_Min = value; }
        }

        [SerializeField]
        [Tooltip("Maximum angle in degrees.")]
        public float m_Max = 360f;

        /// <summary>
        /// Maximum angle in degrees.
        /// </summary>
        public float max
        {
            get { return m_Max; }
            set { m_Max = value; }
        }

        /// <summary>
        /// Invoked when the slider's value changes
        /// </summary>
        public void OnSliderValueChanged()
        {
            if (slider != null)
                angle = slider.value * (max - min) + min;
            Logger.Log("Hello");
        }

        float angle
        {
            get
            {
                return m_RotationCharacter.getCharacterRotation().eulerAngles.y;
            }
            set
            {
                m_RotationCharacter.setCharacterRotation(Quaternion.AngleAxis(value, Vector3.up));
                UpdateText();
            }
        }

        void Awake()
        {
            m_RotationCharacter = GetComponent<RotationCharacter>();
        }

        void OnEnable()
        {
            if (slider != null)
                slider.value = (angle - min) / (max - min);
            UpdateText();
        }

        void UpdateText()
        {
            if (m_Text != null)
                m_Text.text = "Rotation: " + angle + " degrees";
        }
    }
}