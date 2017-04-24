
namespace Assets.Scripts
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;

	public class PartPreview : MonoBehaviour
	{
		public static bool activePreview;
		private static PartPreview instance;
		private float horizPos;
		private float vertPos;
		
		public GameObject rustCP;
		public GameObject colorCP;
		public GameObject flameCP;

		public GameObject rustT;
		public GameObject colorT;
		public GameObject flameT;

		public GameObject rustW;
		public GameObject colorW;
		public GameObject flameW;

		private List<GameObject> partList;

		public Camera tempDisplay;

		public static PartPreview Self
		{
			get
			{
				instance = instance ?? FindObjectOfType(typeof(PartPreview)) as PartPreview;
				return instance;
			}
		}

		public List<GameObject> allParts
		{
			get
			{
				return this.partList;
			}
			set
			{
				this.partList = value;
			}
		}

		// Use this for initialization
		void Start()
		{
			instance = this;
			activePreview = true;

			partList = new List<GameObject>() { rustCP, colorCP, flameCP, rustT, colorT, flameT, rustW, colorW, flameW };

			rustCP.SetActive(false);
			colorCP.SetActive(false);
			flameCP.SetActive(false);
			rustT.SetActive(false);
			colorT.SetActive(false);
			flameT.SetActive(false);
			rustW.SetActive(false);
			colorW.SetActive(false);
			flameW.SetActive(false);

			this.horizPos = this.transform.GetComponent<RectTransform>().sizeDelta.x / 2 + 25;
			this.vertPos = this.transform.GetComponent<RectTransform>().sizeDelta.y / 2;
			this.gameObject.SetActive(false);
		}

		// Update is called once per frame
		void Update()
		{
			
			tempDisplay.transform.position = new Vector3(Input.mousePosition.x - this.horizPos, Input.mousePosition.y + this.vertPos, Input.mousePosition.z);
		}
	}
}