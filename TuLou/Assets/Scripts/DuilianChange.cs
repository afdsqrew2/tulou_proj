using System.Collections.Generic;
using UnityEngine;

public class DuilianChange : MonoBehaviour
{
    [Header("Resources 中的贴图文件夹路径")]
    public string resourcesFolderPath = "MyImages"; // Assets/Resources/MyImages

    private List<Texture2D> textures = new List<Texture2D>();
    private int currentIndex = 0;

    private Renderer planeRenderer;

    void Start()
    {
        planeRenderer = GetComponent<Renderer>();

        // 加载贴图
        Texture2D[] loadedTextures = Resources.LoadAll<Texture2D>(resourcesFolderPath);
        if (loadedTextures.Length == 0)
        {
            Debug.LogWarning("未在 Resources/" + resourcesFolderPath + " 中找到贴图！");
        }
        else
        {
            textures.AddRange(loadedTextures);
        }
    }

    // 这个方法将被 Button 点击事件调用
    public void SwitchTexture()
    {
        this.GetComponent<AudioSource>().Play();
        if (textures.Count == 0 || planeRenderer == null) return;

        planeRenderer.material.mainTexture = textures[currentIndex];
        currentIndex = (currentIndex + 1) % textures.Count;
    }
}
