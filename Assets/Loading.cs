using UnityEngine;
public class Loading : MonoBehaviour
{
    public Material material;
    void Start()
    {

    }

    float time;
    float speed = 50f;
    float loadingDt = 0f;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            loadingDt = 0.000001f;            
        }
        if (loadingDt != 0f)
        {
            time += (Time.deltaTime * speed);
            material.SetFloat("_circleSizePercent", time);
        }
    }    
}
