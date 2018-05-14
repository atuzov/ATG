using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTnaksWidgetView : MonoBehaviour {

	[SerializeField] private RectTransform _iconHolder;

	public RectTransform IconHolder {
		get { return _iconHolder; }
	}
}
