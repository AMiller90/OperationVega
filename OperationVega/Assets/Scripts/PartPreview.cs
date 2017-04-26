
namespace Assets.Scripts
{
	using System.Collections;
	using System.Collections.Generic;
	using UnityEngine;
	using UnityEngine.UI;

	public class PartPreview : MonoBehaviour
	{
		private bool positionSet = false;

		public static bool activePreview;
		private static PartPreview instance;
		public float horizPos;
		public float vertPos;
		
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

		public static PartPreview Self
		{
			get
			{
				instance = instance ?? FindObjectOfType(typeof(PartPreview)) as PartPreview;
				return instance;
			}
		}

		public List<GameObject> exampleParts
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

			foreach (GameObject t in partList)
			{
				t.transform.eulerAngles = new Vector3(0, 0, 35);
				t.SetActive(false);
			}

			this.horizPos = this.transform.GetComponent<RectTransform>().sizeDelta.x / 2 + 10;
			this.vertPos = this.transform.GetComponent<RectTransform>().sizeDelta.y / 2 + 20;
			this.gameObject.SetActive(false);
		}

		// Update is called once per frame
		void Update()
		{
			//this.transform.position = new Vector3(
			//	Input.mousePosition.x - this.horizPos,
			//	Input.mousePosition.y + this.vertPos,
			//	Input.mousePosition.z);
			foreach (GameObject t in partList)
			{
				
				if (t.activeInHierarchy)
				{
					t.transform.Rotate(
						Vector3.right.x * 0,
						Vector3.up.y * Time.deltaTime * 20,
						Vector3.forward.z * Time.deltaTime,
						Space.World);
				}
			}
		}

		private void OnEnable()
		{
			if (!positionSet)
			{
				this.transform.position = new Vector3(
					Input.mousePosition.x - this.horizPos,
					Input.mousePosition.y + this.vertPos,
					Input.mousePosition.z);
				positionSet = true;
			}
		}

		private void OnDisable()
		{
			positionSet = false;
		}
	}
}