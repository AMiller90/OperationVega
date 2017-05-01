
namespace Assets.Scripts.Controllers
{
    using UnityEngine;

    /// <summary>
    /// The camera controller class.
    /// Handles the camera movement functionality
    /// </summary>
    public class CameraController : MonoBehaviour
    {
        /// <summary>
        /// The speed at which the camera moves.
        /// </summary>
        public static uint MoveSpeed = 10;


        /// <summary>
        /// The number of clicks reference.
        /// Reference to how many clicks in succession.
        /// </summary>
        public static int Numofclicks = 0;

        /// <summary>
        /// The double clicked object.
        /// Reference to the object the user double clicked on it.
        /// </summary>
        public static GameObject Doubleclickedobj;

        /// <summary>
        /// The pan with mouse reference.
        /// </summary>
        private bool panwithmouse;

        /// <summary>
        /// The mouse position x.
        /// </summary>
        private float mousePosX;

        /// <summary>
        /// The border offset reference.
        /// </summary>
        private float borderoffset;

        /// <summary>
        /// The rotate speed of the camera reference.
        /// </summary>
        private uint rotateSpeed;

        /// <summary>
        /// The double click delay.
        /// Reference to the delay of the checking for double click.
        /// </summary>
        private float doubleclickdelay = 0;

        /// <summary>
        /// The start function.
        /// </summary>
        private void Start()
        {
            this.panwithmouse = false;
            this.borderoffset = 20;
        }

        /// <summary>
        /// The update function.
        /// </summary>
        private void Update()
        {
            this.ZoomCamera();
            this.PanCamera();

            MoveSpeed = (uint)Mathf.Clamp(MoveSpeed, 10, 15);
            this.rotateSpeed = (uint)Mathf.Clamp(this.rotateSpeed, 3, 5);

            // Clicked once and less than time delay
            if (Numofclicks == 1 && this.doubleclickdelay < 0.25f)
            { // Start counting
                this.doubleclickdelay += Time.deltaTime * 1;
            }

            // Clicked once and greater than or equal to time delay
            if (Numofclicks == 1 && this.doubleclickdelay >= 0.25f)
            { // Reset
                this.doubleclickdelay = 0;
                Numofclicks = 0;
                Doubleclickedobj = null;
            }
            // If clicked twice and within the time delay
            if (Numofclicks == 2 && this.doubleclickdelay < 0.25f)
            { // Successful double click
                Numofclicks = 0;
                this.doubleclickdelay = 0;

                // This line of code snaps the camera to the clicked objects position
                this.gameObject.transform.position = new Vector3(Doubleclickedobj.transform.position.x, 0, Doubleclickedobj.transform.position.z);
                
                // This line of code snaps the camera to zoom in to the clicked object
                Camera.main.transform.localPosition = new Vector3(0, 0, -15);
            }

        }

        /// <summary>
        /// The late update function.
        /// </summary>
        private void LateUpdate()
        {
            this.RotateCamera();
            this.mousePosX = Input.mousePosition.x;
        }

        /// <summary>
        /// The pan camera function.
        /// Moves the camera based on key presses or mouse position.
        /// </summary>
        private void PanCamera()
        {
            // WASD Keys
            if(Input.GetKey(KeyCode.W))
            {
                // Due to the rotation this needs to move forward and right to move up.
                this.transform.position += this.transform.forward * MoveSpeed * Time.deltaTime;
                this.transform.position += this.transform.right * MoveSpeed * Time.deltaTime;

                // Clamp the position
                this.transform.position = new Vector3(
                Mathf.Clamp(this.transform.position.x, -150, 200),
                this.transform.position.y,
                Mathf.Clamp(this.transform.position.z, -170, 220));
            }
            if(Input.GetKey(KeyCode.S))
            {
                // Due to the rotation this needs to move back and left to move down.
                this.transform.position += -this.transform.forward * MoveSpeed * Time.deltaTime;
                this.transform.position += -this.transform.right * MoveSpeed * Time.deltaTime;

                // Clamp the position
                this.transform.position = new Vector3(
                Mathf.Clamp(this.transform.position.x, -150, 200),
                this.transform.position.y,
                Mathf.Clamp(this.transform.position.z, -170, 220));
            }
            if(Input.GetKey(KeyCode.A))
            {
                Camera.main.transform.position += -Camera.main.transform.right * MoveSpeed * Time.deltaTime;

                // Clamp the position
                Camera.main.transform.position = new Vector3(
                Mathf.Clamp(Camera.main.transform.position.x, -150, 200),
                Camera.main.transform.position.y,
                Mathf.Clamp(Camera.main.transform.position.z, -170, 220));
            }
            if(Input.GetKey(KeyCode.D))
            {
                Camera.main.transform.position += Camera.main.transform.right * MoveSpeed * Time.deltaTime;

                // Clamp the position
                Camera.main.transform.position = new Vector3(
                Mathf.Clamp(Camera.main.transform.position.x, -150, 200),
                Camera.main.transform.position.y,
                Mathf.Clamp(Camera.main.transform.position.z, -170, 220));
            }

            // Arrow Keys
            if(Input.GetKey(KeyCode.UpArrow))
            {
                // Due to the rotation this needs to move forward and right to move up.
                this.transform.position += this.transform.forward * MoveSpeed * Time.deltaTime;
                this.transform.position += this.transform.right * MoveSpeed * Time.deltaTime;

                // Clamp the position
                this.transform.position = new Vector3(
                Mathf.Clamp(this.transform.position.x, -150, 200),
                this.transform.position.y,
                Mathf.Clamp(this.transform.position.z, -170, 220));
            }
            if(Input.GetKey(KeyCode.DownArrow))
            {
                // Due to the rotation this needs to move back and left to move down.
                this.transform.position += -this.transform.forward * MoveSpeed * Time.deltaTime;
                this.transform.position += -this.transform.right * MoveSpeed * Time.deltaTime;

                // Clamp the position
                this.transform.position = new Vector3(
                Mathf.Clamp(this.transform.position.x, -150, 200),
                this.transform.position.y,
                Mathf.Clamp(this.transform.position.z, -170, 220));
            }
            if(Input.GetKey(KeyCode.LeftArrow))
            {
                Camera.main.transform.position += -Camera.main.transform.right * MoveSpeed * Time.deltaTime;

                // Clamp the position
                Camera.main.transform.position = new Vector3(
                Mathf.Clamp(Camera.main.transform.position.x, -150, 200),
                Camera.main.transform.position.y,
                Mathf.Clamp(Camera.main.transform.position.z, -170, 220));
            }
            if(Input.GetKey(KeyCode.RightArrow))
            {
                Camera.main.transform.position += Camera.main.transform.right * MoveSpeed * Time.deltaTime;

                // Clamp the position
                Camera.main.transform.position = new Vector3(
                Mathf.Clamp(Camera.main.transform.position.x, -150, 200),
                Camera.main.transform.position.y,
                Mathf.Clamp(Camera.main.transform.position.z, -170, 220));
            }

            // Reset the camera 
            if(Input.GetKeyDown(KeyCode.T))
            {
                this.transform.eulerAngles = Vector3.zero;
                this.transform.position = Vector3.zero;

                // This line of code snaps the camera to zoom in to the clicked object
                Camera.main.transform.localPosition = new Vector3(0, 0, -45);
            }

            // Toggle on or off panning of camera with mouse
            if(Input.GetKeyDown(KeyCode.Y))
            {
                this.panwithmouse = !this.panwithmouse;
            }

            // If can pan with the mouse
            if(this.panwithmouse)
            {
                if(Input.mousePosition.x >= Screen.width - this.borderoffset && Input.mousePosition.x < Screen.width)
                {
                    this.transform.position += this.transform.right * MoveSpeed * Time.deltaTime;
                }
                if(Input.mousePosition.x <= this.borderoffset && Input.mousePosition.x > 0)
                {
                    this.transform.position += -this.transform.right * MoveSpeed * Time.deltaTime;
                }
                if(Input.mousePosition.y >= Screen.height - this.borderoffset && Input.mousePosition.y < Screen.height)
                {
                    this.transform.position += Vector3.up * MoveSpeed * Time.deltaTime;
                }
                if(Input.mousePosition.y <= this.borderoffset && Input.mousePosition.y > 0)
                {
                    this.transform.position += Vector3.down * MoveSpeed * Time.deltaTime;
                }
            }
        }

        /// <summary>
        /// The zoom camera function.
        /// This function changes the orthographic size of the camera to zoom in or out.
        /// </summary>
        private void ZoomCamera()
        {
    
                if (Input.mouseScrollDelta.y < 0)
                {
                    Camera.main.transform.localPosition -= new Vector3(0, 0, 0.25f) * 5f;
                    var campos = Camera.main.transform.localPosition;
                    Camera.main.transform.localPosition
                        = new Vector3(campos.x, campos.y, Mathf.Clamp(campos.z, -85.0f ,- 15.0f));
            }
                else if (Input.mouseScrollDelta.y > 0)
                {
                    Camera.main.transform.localPosition += new Vector3(0, 0, 0.25f) * 5f;
                    var campos = Camera.main.transform.localPosition;
                    Camera.main.transform.localPosition
                        = new Vector3(campos.x, campos.y, Mathf.Clamp(campos.z, -85.0f ,-15.0f));
            }
        }

        /// <summary>
        /// The rotate camera function.
        /// This function allows camera rotation.
        /// </summary>
        private void RotateCamera()
        {
            if(Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.LeftAlt))
            {
                if(Input.mousePosition.x != this.mousePosX)
                {
                    float camroty = (Input.mousePosition.x - this.mousePosX) * this.rotateSpeed * Time.deltaTime;
                    this.transform.Rotate(0.0f, camroty, 0.0f);
                    this.transform.eulerAngles = new Vector3(0, this.ClampAngle(this.transform.eulerAngles.y, -85.0f, 90.0f), 0);
                }
            }
        }

        /// <summary>
        /// The Clamp angle function.
        /// Clamps the angle to passed in arguments.
        /// <para></para>
        /// <remarks><paramref name="angle"></paramref> -The angle to rotate.</remarks>
        /// <para></para>
        /// <remarks><paramref name="min"></paramref> -The lowest angle allowed.</remarks>
        /// <para></para>
        /// <remarks><paramref name="max"></paramref> -The max angle allowed.</remarks>
        /// </summary>
        /// <returns>
        /// The <see cref="float"/>.
        /// </returns>
        private float ClampAngle(float angle, float min, float max)
        {
            // Make sure the numbers are never less than 0 and greater than 360.
            angle = Mathf.Repeat(angle, 360);
            min = Mathf.Repeat(min, 360);
            max = Mathf.Repeat(max, 360);
            bool inverse = false;
            float tmin = min;
            float tangle = angle;
            if(min > 180)
            {
                inverse = !inverse;
                tmin -= 180;
            }
            if(angle > 180)
            {
                inverse = !inverse;
                tangle -= 180;
            }
            bool result = !inverse ? tangle > tmin : tangle < tmin;
            if(!result)
                angle = min;

            inverse = false;
            tangle = angle;
            float tmax = max;
            if(angle > 180)
            {
                inverse = !inverse;
                tangle -= 180;
            }
            if(max > 180)
            {
                inverse = !inverse;
                tmax -= 180;
            }

            result = !inverse ? tangle < tmax : tangle > tmax;
            if(!result)
                angle = max;

            return angle;
        }
    }
}