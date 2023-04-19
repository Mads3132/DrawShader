using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Draw : MonoBehaviour
{
    public Camera cam;
    public Shader paintShader;
    //float startTime;
    public float speed = 0.2f;
    public Color startColor = Color.red;
    public Color endColor = Color.yellow;
    public Gradient gradient;
    private GradientColorKey[] colorKey;
    private GradientAlphaKey[] alphaKey;

    //public float duration = 2f;
    //public float t = 0f;

    RenderTexture splatMap;
    Material snowMaterial,drawMaterial;

    RaycastHit hit;
    private float lastMouseMoveTime;
    private Vector3 lastMousePosition;

    void Start()
    {
        //startTime = Time.time;
        lastMouseMoveTime = Time.time;
        lastMousePosition = Input.mousePosition;

        drawMaterial = new Material(paintShader);

        snowMaterial = GetComponent<MeshRenderer>().material;
        splatMap = new RenderTexture(1024, 1024, 0, RenderTextureFormat.ARGBFloat);
        snowMaterial.mainTexture = splatMap;

        gradient = new Gradient();
        colorKey = new GradientColorKey[2];
        colorKey[0].color = startColor;
        colorKey[0].time = 0.0f;
        colorKey[1].color = endColor;
        colorKey[1].time = 1.0f;

        alphaKey = new GradientAlphaKey[2];
        alphaKey[0].alpha = 1.0f;
        alphaKey[0].time = 0.0f;
        alphaKey[1].alpha = 0.0f;
        alphaKey[1].time = 1.0f;

        gradient.SetKeys(colorKey, alphaKey);
    }
    void Update()
    {
        //float value = Mathf.Lerp(0f, 0.95f, speed);
        //t += Time.deltaTime / duration;
        Vector3 currentMousePosition = Input.mousePosition;
        if(currentMousePosition != lastMousePosition)
        {
            lastMouseMoveTime = Time.time;
            lastMousePosition = currentMousePosition;

            if(Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition),out hit))
                {
                    drawMaterial.SetVector("_Coordinate", new Vector4(hit.textureCoord.x, hit.textureCoord.y, 0, 0));
                    RenderTexture temp = RenderTexture.GetTemporary(splatMap.width, splatMap.height, 0, RenderTextureFormat.ARGBFloat);
                    Graphics.Blit(splatMap, temp);
                    Graphics.Blit(temp, splatMap, drawMaterial);
                    RenderTexture.ReleaseTemporary(temp);
                }
            drawMaterial.SetVector("_Color", gradient.Evaluate(0f));
            //drawMaterial.SetVector("_Color", startColor);
        }
        else if(Time.time - lastMouseMoveTime > 0.00001f)
            if(Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition),out hit))
            {
                drawMaterial.SetVector("_Coordinate", new Vector4(hit.textureCoord.x, hit.textureCoord.y, 0, 0));
                RenderTexture temp = RenderTexture.GetTemporary(splatMap.width, splatMap.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(splatMap, temp);
                Graphics.Blit(temp, splatMap, drawMaterial);
                RenderTexture.ReleaseTemporary(temp);
                //drawMaterial.SetVector("_Color", Color.Lerp(startColor, endColor, speed));
                drawMaterial.SetVector("_Color", gradient.Evaluate(speed));
            }
    }

/*
    void onMouseDown()
    {
        if(Input.GetMouseButtonDown(0))
        {
            timeAtButtonDown = timeCurrent;
            drawMaterial.SetVector("_Color", Color.red);
            if(Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition),out hit))
                {
                    drawMaterial.SetVector("_Coordinate", new Vector4(hit.textureCoord.x, hit.textureCoord.y, 0, 0));
                    RenderTexture temp = RenderTexture.GetTemporary(splatMap.width, splatMap.height, 0, RenderTextureFormat.ARGBFloat);
                    Graphics.Blit(splatMap, temp);
                    Graphics.Blit(temp, splatMap, drawMaterial);
                    RenderTexture.ReleaseTemporary(temp);
                }
        }
    }

    void onMouseUp()
    {
        if(Input.GetMouseButton(0))
        {
            timeAtButtonUp = timeCurrent;

            timeButtonHeld = (timeAtButtonUp - timeAtButtonDown);

            if(Physics.Raycast(cam.ScreenPointToRay(Input.mousePosition),out hit))
            {
                Vector3 localHit = transform.InverseTransformPoint(hit.point);
                drawMaterial.SetVector("_Coordinate", new Vector3(hit.textureCoord.x, hit.textureCoord.y, 0));
                RenderTexture temp = RenderTexture.GetTemporary(splatMap.width, splatMap.height, 0, RenderTextureFormat.ARGBFloat);
                Graphics.Blit(splatMap, temp);
                Graphics.Blit(temp, splatMap, drawMaterial);
                RenderTexture.ReleaseTemporary(temp);
            }

            if(timeButtonHeld > 0.5)
            {
                float t = (Time.time - startTime) * speed;
                drawMaterial.SetVector("_Color", Color.Lerp(Color.red, Color.green, t));
            }
        }
    }
*/
}