using System;
using UnityEngine;
using UnityEngine.UI;

namespace TransitionRandomiser.UI
{
    class CustomText
    {

        private GameObject textObject;
        private Text textText;
        private uGUI_TextFade textFade;
        private ContentSizeFitter textFitter;

        private float xOffset, yOffset;

        private bool useGlobalTextWidth;

        public CustomText(string text, bool useGlobalTextWidth = true, int fontSize = 20, TextAnchor alignment = TextAnchor.MiddleLeft)
        {
            this.useGlobalTextWidth = useGlobalTextWidth;

            textObject = new GameObject("TwitchInteractionTimerCooldown");
            textText = textObject.AddComponent<Text>();
            textFade = textObject.AddComponent<uGUI_TextFade>();
            textFitter = textObject.AddComponent<ContentSizeFitter>();

            textFitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            textFitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

            textText.font = uGUI.main.intro.mainText.text.font;
            textText.fontSize = fontSize;
            textText.fontStyle = uGUI.main.intro.mainText.text.fontStyle;
            textText.alignment = alignment;
            textText.color = uGUI.main.intro.mainText.text.color;
            textText.material = uGUI.main.intro.mainText.text.material;

            // Do this so it also shows over black screens
            Graphic g = uGUI.main.overlays.overlays[0].graphic;
            textObject.transform.SetParent(g.transform, false);
            textText.canvas.overrideSorting = true;
            textObject.layer = 1;

            SetText(text);
            Update();
        }

        public void SetSize(int textSize)
        {
            textText.fontSize = textSize;
        }

        public void Update(int xOffset = 0, int yOffset = 0)
        {
            this.xOffset = xOffset;
            this.yOffset = yOffset;
            AlignText();
        }

        public void Destroy()
        {
            GameObject.Destroy(textObject);
        }

        public void SetText(string text)
        {
            textFade.SetText(text, false);
            //AlignText();
            textFade.SetState(true);
            textObject.SetActive(true);
        }

        public float getTextWidth()
        {
            return textText.preferredWidth;
        }

        private void AlignText()
        {
            float scaleX = (1920f / Screen.width);
            float scaleY = (1920f / Screen.width);

            //float width = textText.preferredWidth;
            float width = getTextWidth();

            float widestText = useGlobalTextWidth ? CustomUI.widestText : getTextWidth();

            float x = Screen.width / 2 - (Screen.width / 1920f * widestText) - xOffset - CustomUI.ActualTextHeight();
            float y = -Screen.height / 2 - yOffset + CustomUI.ActualTextHeight();

            x *= scaleX;
            y *= scaleY;

            float displayX;
            switch (textText.alignment)
            {
                case TextAnchor.UpperLeft:
                case TextAnchor.MiddleLeft:
                case TextAnchor.LowerLeft:
                    displayX = x + width / 2f;
                    goto IL_9A;
                case TextAnchor.UpperRight:
                case TextAnchor.MiddleRight:
                case TextAnchor.LowerRight:
                    displayX = x - width / 2f;
                    goto IL_9A;
            }
            displayX = x;
        IL_9A:
            float displayY = y;
            textObject.transform.localPosition = new Vector3(displayX, displayY, 0f);
        }

    }

    public class CustomUI
    {

        private static CustomText firstText, secondText, bigText;

        public static int pixelTextHeight = 20;

        private static bool initialised = false;

        public static float widestText = 0;

        public static int ActualTextHeight()
        {
            return (int)(pixelTextHeight * (Screen.width / 1920f));
        }

        public static void SetFirstText(String text)
        {
            if (!initialised)
            {
                Initialise();
            }
            firstText.SetText(text);
        }
        public static void SetSecondText(String text)
        {
            if (!initialised)
            {
                Initialise();
            }
            secondText.SetText(text);
        }

        public static void SetBigText(String text)
        {
            if (!initialised)
            {
                Initialise();
            }
            bigText.SetText(text);
        }

        public static void Update()
        {
            float newWidestText = 0;
            if (!initialised)
            {
                Initialise();
            }

            try
            {
                firstText.Update(0, -ActualTextHeight() * 2);
                secondText.Update(0, -ActualTextHeight() * 1);
               
                if (firstText.getTextWidth() > newWidestText)
                {
                    newWidestText = firstText.getTextWidth();
                }
                if (secondText.getTextWidth() > newWidestText)
                {
                    newWidestText = secondText.getTextWidth();
                }

                bigText.Update((int) (Screen.width / 2 - bigText.getTextWidth()), -Screen.height / 2);
            }
            catch (Exception)
            {
                Initialise();
                firstText.Update(0, -ActualTextHeight() * 2);
                secondText.Update(0, -ActualTextHeight() * 1);
                bigText.Update((int)(Screen.width / 2 - bigText.getTextWidth()), -Screen.height / 2);
            }

            widestText = newWidestText;
        }

        public static Boolean IsInitialised()
        {
            return initialised;
        }

        public static void Initialise()
        {
            firstText = new CustomText("");
            secondText = new CustomText("");
            bigText = new CustomText("", false, 60, TextAnchor.MiddleCenter);

            initialised = true;
        }

    }
}
