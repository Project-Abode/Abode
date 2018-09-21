using System.Collections;
using System.Collections.Generic;
using System.Reflection;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public class ImageQuad : MediaQuad
{

   /* public GameObject rendObject;
    public bool loadOnStart;
    public Texture2D image;

    private Renderer rend;

    protected override void Awake()
    {
        base.Awake();

        if (rendObject == null)
        {
            rendObject = gameObject;
        }
        rend = rendObject.GetComponent<Renderer>();

        if (loadOnStart) SetTexture(image);
    }

    public void SetTexture(Texture2D texture)
    {
		//Get image dimensions- this is a pain due to Unity fitting textures to powers of 2 with no easy way to get original dimensions
		int texWidth;
		int texHeight;
		bool success = GetImageSize(texture, out texWidth, out texHeight);
		if (!success)
		{
			//automatically getting dimensions failed- use less accurate method
			texWidth = texture.width;
			texHeight = texture.height;
		}
		
		Scale(texWidth, texHeight);
        rend.material.mainTexture = texture;
    }

	//By numberkruncher
	//https://forum.unity.com/threads/getting-original-size-of-texture-asset-in-pixels.165295/
	public static bool GetImageSize(Texture2D asset, out int width, out int height)
	{
		if (asset != null)
        {
            string assetPath = GetAssetPath(asset);
            TextureImporter importer = AssetImporter.GetAtPath(assetPath) as TextureImporter;

            if (importer != null)
            {
                object[] args = new object[2] { 0, 0 };
                MethodInfo mi = typeof(TextureImporter).GetMethod("GetWidthAndHeight", BindingFlags.NonPublic | BindingFlags.Instance);
                mi.Invoke(importer, args);

                width = (int)args[0];
                height = (int)args[1];

                return true;
            }
        }

        height = width = 0;
		return false;
	}
    //edited code
    private static string GetAssetPath(Texture2D asset)
    {
        return AssetDatabase.GetAssetPath(asset);
    }*/
}
