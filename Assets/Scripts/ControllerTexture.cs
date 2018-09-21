using UnityEngine;

/// <summary>
/// Override the texture of the Vive controllers, with your own texture, after SteamVR has loaded and applied the original texture.
/// Modified from script by Mr_FJ: https://steamcommunity.com/app/358720/discussions/0/357287304420388604/
/// </summary>
public class ControllerTexture : MonoBehaviour
{
    public Texture2D newBodyTexture; //The new texture.

    void OnEnable()
    {
        //Subscribe to the event that is called by SteamVR_RenderModel, when the controller mesh + texture, has been loaded completely.
        //SteamVR_Utils.Event.Listen("render_model_loaded", OnControllerLoaded);
        SteamVR_Events.RenderModelLoaded.Listen(OnControllerLoaded);
    }
    
    /// <summary>
    /// Override the texture of each of the parts, with your texture.
    /// </summary>
    /// <param name="newTexture">Override texture</param>
    /// <param name="modelTransform">Transform of the gameobject, which has the SteamVR_RenderModel component.</param>
    public void UpdateControllerTexture(Texture2D newTexture, Transform modelTransform)
    {
        string[] parts =
        {
            "body",
            "button",
            "led",
            "lgrip",
            "rgrip",
            "scroll_wheel",
            "sys_button",
            "trackpad",
            "trackpad_scroll_cut",
            "trackpad_touch",
            "trigger"
        };

        foreach(string part in parts)
        {
            modelTransform.Find(part).GetComponent<MeshRenderer>().material.mainTexture = newTexture;
        }
    }

    /// <summary>
    /// Call this method, when the "render_model_loaded" event is triggered.
    /// </summary>
    void OnControllerLoaded(SteamVR_RenderModel model, bool isConnected)
    {
        if (isConnected && newBodyTexture != null)
        {
            UpdateControllerTexture(newBodyTexture, model.gameObject.transform);
        }
    }
}