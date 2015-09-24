using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class BombMeter : MonoBehaviour {
	public BombLauncher weapon;
	
	private int count;
	private GameObject icon_1;
	private GameObject icon_2;
	private GameObject icon_3;
	private GameObject icon_4;
	private GameObject icon_5;
	private GameObject icon_6;
	
	void Start () {
		icon_1 = transform.Find ("Icon 1").gameObject;
		icon_2 = transform.Find ("Icon 2").gameObject;
		icon_3 = transform.Find ("Icon 3").gameObject;
		icon_4 = transform.Find ("Icon 4").gameObject;
		icon_5 = transform.Find ("Icon 5").gameObject;
		icon_6 = transform.Find ("Icon 6").gameObject;
	}
	
	void Update () {
		count = weapon.currentBombCount;
		icon_1.SetActive(count >= 1);
		icon_2.SetActive(count >= 2);
		icon_3.SetActive(count >= 3);
		icon_4.SetActive(count >= 4);
		icon_5.SetActive(count >= 5);
		icon_6.SetActive(count >= 6);
	}
}
