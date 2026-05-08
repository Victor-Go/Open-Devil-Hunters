using Code.Scripts.Src.Configurations;
using Code.Scripts.Src.Configurations;
using UnityEngine;
using UnityEngine.UI;

namespace Code.Scripts.Behaviour
{
    public class FpsDisplayer : MonoBehaviour
    {
        private Text text;

        int frameCounter = 0;
        float timeCounter = 0.0f;
        float lastFramerate = 0.0f;
        public float refreshTime = 0.5f;

        private static readonly string[] fpsStrings;

        static FpsDisplayer()
        {
            fpsStrings = new string[300];
            for (int i = 0; i < 300; i++)
            {
                fpsStrings[i] = "FPS: " + i.ToString("F2");
            }
        }

        private void Awake()
        {
            text = GetComponentInChildren<Text>();
            refreshTime = Mathf.Max(0.01f, refreshTime);
        }

        private void Start()
        {
            if (!DebugConfigurations.DebugEnabled)
            {
                gameObject.SetActive(false);
            }
        }

        void Update()
        {

            if (timeCounter < refreshTime)
            {
                timeCounter += Time.deltaTime;
                frameCounter++;
            }
            else
            {
                //This code will break if you set your m_refreshTime to 0, which makes no sense.
                if (timeCounter > 0f)
                {
                    lastFramerate = frameCounter / timeCounter;
                }
                frameCounter = 0;
                timeCounter = 0.0f;
                int fpsIndex = Mathf.Clamp(Mathf.RoundToInt(lastFramerate), 0, 299);
                text.text = fpsStrings[fpsIndex];
            }
        }
    }
}
