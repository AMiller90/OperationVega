
namespace Assets.Scripts
{
    using System.Collections;
    using System.Collections.Generic;

    using UnityEngine;
    using UnityEngine.SceneManagement;
    using UnityEngine.UI;

    /// <summary>
    /// The slide show class.
    /// Handles the slide show in the credit screen.
    /// </summary>
    public class SlideShow : MonoBehaviour
    {

        /// <summary>
        /// The fade time reference.
        /// The fade time will be 2 seconds.
        /// </summary>
        private const float Fadetime = 3.0f;

        /// <summary>
        /// The slide sprites reference.
        /// Reference to the sprites that can be displayed.
        /// </summary>
        [SerializeField]
        private List<Sprite> slidesprites;

        /// <summary>
        /// The list index reference.
        /// Reference to the image to display.
        /// </summary>
        private int listindex;

        /// <summary>
        /// The quit to main menu function.
        /// This brings the user back to the main menu.
        /// </summary>
        public void QuitToMainMenu()
        {
            SceneManager.LoadScene(0);
        }

        /// <summary>
        /// The quit function.
        /// This quits the application.
        /// </summary>
        public void Quit()
        {
            Application.Quit();
        }

        /// <summary>
        /// The start function.
        /// </summary>
        private void Start()
        {
            this.listindex = 0;
            this.GetComponent<Image>().sprite = this.slidesprites[0];
            this.StartCoroutine(this.FadeOut());
        }

        /// <summary>
        /// The fade out function.
        /// Handles the text fading out.
        /// </summary>
        /// <returns>
        /// The <see cref="IEnumerator"/>.
        /// </returns>
        private IEnumerator FadeOut()
        {
            // Get current alpha
            float startalpha = this.GetComponent<Image>().color.a;

            float endalpha;

            // If alpha is maxed
            if (startalpha >= 0.9f)
            {// Then set the goal alpha to 0
                endalpha = 0;
            }
            else
            { // Else set the goal to 1
                endalpha = 1.0f;
            }

            // The rate to fade at
            float rate = 1.0f / Fadetime;

            // Progress of the fade
            float progress = 0.0f;

            // While its less than 1
            while (progress < 1.0)
            {
                // Get the current color
                Color tmpColor = this.GetComponent<Image>().color;

                    // Set the color to the updated alpha
                    this.GetComponent<Image>().color = new Color(
                        tmpColor.r,
                        tmpColor.g,
                        tmpColor.b,
                        Mathf.Lerp(startalpha, endalpha, progress));

                // Increase the progress
                progress += rate * Time.deltaTime;

                yield return null;
            }

            // Once the image has made a full fade in and out then set next image
            if (endalpha <= 0)
            {
                // Get ready next image to load.
                this.listindex++;

                // If the list index is now greater than or equal to the last image..start over.
                if (this.listindex >= this.slidesprites.Count) this.listindex = 0;

                // Set the new image then wait 2 seconds
                this.GetComponent<Image>().sprite = this.slidesprites[this.listindex];
                this.GetComponent<Image>().preserveAspect = true;
            }

            yield return new WaitForSeconds(1.0f);
            
            yield return this.FadeOut();
        }

    }
}
