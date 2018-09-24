using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceQuad : ImageQuad
{

	private void Start()
	{
		LoadImage();
	}

	private void LoadImage()
	{
		Texture2D[] textures = Resources.LoadAll<Texture2D>("Hearth");
		Texture2D texture = textures[0];
		//SetTexture(texture);
	}

}
