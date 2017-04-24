
namespace Assets.Scripts.Managers
{
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    /// <summary>
    /// The audio manager class.
    /// Handles audio for the game.
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        /// <summary>
        /// The audio manager reference.
        /// </summary>
        private static AudioManager audiomanager;

        /// <summary>
        /// The changed volume reference.
        /// Reference for when the user changes the volume in the options menu.
        /// </summary>
        public static float ChangedVolume = 0.2f;

        /// <summary>
        /// The audio source reference.
        /// </summary>
        private AudioSource audiosource;

        /// <summary>
        /// The clips reference.
        /// Stores reference to the list of playable clips.
        /// </summary>
        [SerializeField]
        private List<AudioClip> clips = new List<AudioClip>();

        /// <summary>
        /// Gets the self reference of the audio manager.
        /// </summary>
        public static AudioManager Self
        {
            get
            {
                if (audiomanager == null)
                {
                    audiomanager = FindObjectOfType<AudioManager>();
                }

                return audiomanager;
            }
        }

        /// <summary>
        /// Gets or sets the volume for the audio source.
        /// </summary>
        public float Volume
        {
            get
            {
                return this.audiosource.volume;
            }

            set
            {
                this.audiosource.volume = value;
            }
        }

        /// <summary>
        /// The start function.
        /// Handles the music for each scene.
        /// </summary>
        private void Start()
        {
            audiomanager = this;

            this.audiosource = this.gameObject.GetComponent<AudioSource>();

            Scene currentscene = SceneManager.GetActiveScene();

            // The clips are set based on the order of scenes.
            // So just call the clip index associated with the current build index.
            // Only if the index is less than the clip count.
            if (currentscene.buildIndex < this.clips.Count)
            {
                this.audiosource.volume = ChangedVolume;
                this.audiosource.PlayOneShot(this.clips[currentscene.buildIndex]);
            }
        }

    }
}
