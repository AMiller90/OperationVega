﻿
namespace Assets.Scripts
{
	using System.Collections.Generic;
	using System.Linq;

	using Assets.Scripts.BaseClasses;
	using Assets.Scripts.Interfaces;

	using UnityEngine;

	/// <summary>
	/// The rocket class.
	/// </summary>
	public class Rocket : MonoBehaviour
	{
		/// <summary>
		/// The all parts.
		/// </summary>
		private List<IRocketParts> allParts;

		/// <summary>
		/// The total quality.
		/// </summary>
		private uint totalQuality;

		/// <summary>
		/// The selectable cockpit.
		/// </summary>
		// private Cockpit selectableCockpit;

		/// <summary>
		/// The selectable chassis.
		/// </summary>
		// private Chassis selectableChassis;

		/// <summary>
		/// The selectable wings.
		/// </summary>
		// private Wings selectableWings;

		/// <summary>
		/// The selectable thrusters.
		/// </summary>
		// private Thrusters selectableThrusters;

		private BaseCockpit currentCockpit;

		/// <summary>
		/// The cockpit 1.
		/// </summary>
		public Cockpit Cockpit1;

		/// <summary>
		/// The cockpit 2.
		/// </summary>
		public Cockpit Cockpit2;

		/// <summary>
		/// The cockpit 3.
		/// </summary>
		public Cockpit Cockpit3;

		/// <summary>
		/// Gets or sets the part list.
		/// </summary>
		public List<IRocketParts> PartList
		{
			get
			{
				return this.allParts;
			}

			set
			{
				this.allParts = value;
			}
		}

		/// <summary>
		/// Gets or sets the total quality.
		/// </summary>
		public uint FullQuality
		{
			get
			{
				return this.totalQuality;
			}

			set
			{
				this.totalQuality = value;
			}
		}

		/// <summary>
		/// The ship build.
		/// </summary>
		/// <returns>
		/// The <see cref="bool"/>.
		/// </returns>
		public bool ShipBuild()
		{
			return this.allParts.OfType<BaseCockpit>().Any() &&
			       this.allParts.OfType<BaseChassis>().Any() &&
			       this.allParts.OfType<BaseThrusters>().Any() &&
			       this.allParts.OfType<BaseWings>().Any();
		}

		/// <summary>
		/// Function for adding a cockpit to the list.
		/// </summary>
		/// <param name="secondaryList">
		/// The secondaryList. A second list of the parts.
		/// Solves error that's thrown when a part is removed from the list.
		/// </param>
		/// <param name="selectedParts">
		/// The selected Parts.
		/// </param>
		public void AddParts(List<IRocketParts> secondaryList, IRocketParts selectedParts)
		{
			if (secondaryList.Contains(selectedParts))
			{
				this.allParts.Remove(selectedParts);
				this.totalQuality -= selectedParts.Quality;
			}
			else if (!secondaryList.Contains(selectedParts))
			{
				if (User.SteelCount >= selectedParts.SteelCost && User.FuelCount >= selectedParts.FuelCost)
				{
					this.allParts.Add(selectedParts);
					this.totalQuality += selectedParts.Quality;
					User.SteelCount -= selectedParts.SteelCost;
					User.FuelCount -= selectedParts.FuelCost;
				}

				if (User.SteelCount < selectedParts.SteelCost)
				{
					Debug.Log("You don't have enough steel.");
				}

				if (User.FuelCount < selectedParts.FuelCost)
				{
					Debug.Log("You don't have enough fuel.");
				}
			}
		}

		public void AddCockpit(List<IRocketParts> secondaryList, Cockpit selectedCockpit)
		{

			if (User.SteelCount >= selectedCockpit.Accessed.SteelCost && User.FuelCount >= selectedCockpit.Accessed.FuelCost)
			{
				if (!secondaryList.OfType<BaseCockpit>().Any())
				{
					this.allParts.Add(selectedCockpit.Accessed);
					this.totalQuality += selectedCockpit.Accessed.Quality;
					User.SteelCount -= selectedCockpit.Accessed.SteelCost;
					User.FuelCount -= selectedCockpit.Accessed.FuelCost;
					this.currentCockpit = selectedCockpit.Accessed;
				}
				else if (secondaryList.OfType<BaseCockpit>().Any() && !secondaryList.Contains(selectedCockpit.Accessed))
				{
					this.totalQuality -= this.currentCockpit.Quality;
					this.allParts.Remove(this.currentCockpit);

					this.allParts.Add(selectedCockpit.Accessed);
					this.totalQuality += selectedCockpit.Accessed.Quality;
					User.SteelCount -= selectedCockpit.Accessed.SteelCost;
					User.FuelCount -= selectedCockpit.Accessed.FuelCost;
					this.currentCockpit = selectedCockpit.Accessed;
				}
			}
		}

		/// <summary>
		/// Use this for initialization
		/// </summary>
		private void Start()
		{
			this.allParts = new List<IRocketParts>();

			// this.selectableCockpit = FindObjectOfType<Cockpit>();
			// this.selectableChassis = FindObjectOfType<Chassis>();
			// this.selectableWings = FindObjectOfType<Wings>();
			//this.selectableThrusters = FindObjectOfType<Thrusters>();
			this.Cockpit1.Accessed = new BaseCockpit(20, 200, 0, 20);
			this.Cockpit2.Accessed = new BaseCockpit(30, 200, 0, 50);
			this.Cockpit3.Accessed = new BaseCockpit(40, 200, 0, 80);

			// this.selectableCockpit.Accessed = new BaseCockpit(20, 200, 0, 20);
			// this.selectableChassis.Accessed = new BaseChassis(200, 0, 20);
			// this.selectableWings.Accessed = new BaseWings(200, 0, 20);
			// this.selectableThrusters.Accessed = new BaseThrusters(200, 50, 20);
		}

		/// <summary>
		/// Update is called once per frame
		/// </summary>
		private void Update()
		{
			var spareList = this.allParts.ToList();

			if (Input.GetMouseButtonDown(0))
			{
				this.AddCockpit(spareList, this.Cockpit1);
			}

			if (Input.GetMouseButtonDown(1))
			{
				this.AddCockpit(spareList, this.Cockpit2);
			}

			if (Input.GetKeyDown(KeyCode.Keypad1))
			{
				this.AddCockpit(spareList, this.Cockpit3);
			}

			//if (Input.GetKeyDown(KeyCode.Keypad2))
			//{
			//	this.AddParts(spareList, this.selectableThrusters.Accessed);
			//}

			if (Input.GetKeyDown(KeyCode.A))
			{
				User.SteelCount += 100;
			}

			if (Input.GetKeyDown(KeyCode.S))
			{
				User.FuelCount += 100;
			}
			
			Debug.Log(this.allParts.Count);
			Debug.Log("Total: " + this.totalQuality);
		}
	}
}
