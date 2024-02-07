using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemIconMaker : MonoBehaviour
{
    [SerializeField]
    private Camera m_camera;
    [SerializeField]
    private GameObject m_gameObject;
    [SerializeField]
    private Image m_image;

    public KeyCode activationKeycode;

    public void Start()
    {
        GetIcon(m_gameObject, m_camera, m_image);
    }

    public void Update()
    {
        if(Input.GetKeyDown(activationKeycode))
        {
            GetIcon(m_gameObject, m_camera, m_image);
        }
    }

    public static Sprite GetIcon(GameObject item, Camera cam, Image icon)
    {
        cam.orthographicSize = item.GetComponent<Renderer>().bounds.extents.y + 0.1f;

        int xResolution = cam.pixelWidth;
        int yResolution = cam.pixelHeight;

        int clipX = 0;
        int clipY = 0;

        if(xResolution > yResolution)
        {
            clipX = xResolution - yResolution;
        }
        else if(yResolution > xResolution)
        {
            clipY = yResolution - xResolution;
        }
        

        Texture2D tex = new Texture2D(xResolution - clipX, yResolution - clipY, TextureFormat.RGBA32, false);

        RenderTexture renderTex = new RenderTexture(xResolution, yResolution, 24);
        cam.targetTexture = renderTex;

        RenderTexture.active = renderTex;
        cam.Render();
        tex.ReadPixels(new Rect(clipX / 2, clipY / 2, xResolution - clipX, yResolution - clipY), 0, 0);
        tex.Apply();

        //TODO
        //byte[] bytes = tex.EncodeToPNG();
        //System.IO.File.WriteAllBytes("mypath/myfile.png", bytes);

        cam.targetTexture = null;
        RenderTexture.active = null;

        Destroy(renderTex);

        Sprite result = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0, 0));
        icon.sprite = result;
        return result;
    }
}
