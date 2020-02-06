using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ClickableTile : MonoBehaviour
{
	public int tileX;
	public int tileZ;
	public TileMap map;

	void OnMouseUp()
	{
		Debug.Log("Click!");

		if (EventSystem.current.IsPointerOverGameObject())
			return;

		map.GeneratePathTo(tileX, tileZ);
	}

}
