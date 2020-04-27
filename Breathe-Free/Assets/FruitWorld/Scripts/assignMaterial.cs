using UnityEngine;

public class assignMaterial : MonoBehaviour
{
    // Start is called before the first frame update
    private mechanics m;
    [SerializeField] private Material sky_mat;
    [SerializeField] private Material changeMat;
    private Material currentMat1;
    private Material currentMat2;

    void Start()
    {
        currentMat1 = m.fru[Random.Range(0, m.fru.Count)].GetComponent<MeshRenderer>().material;
        currentMat2 = m.fru[Random.Range(0, m.fru.Count)].GetComponent<MeshRenderer>().material;
        m = GameObject.Find("hand").GetComponent<mechanics>();
        if(RenderSettings.skybox == sky_mat)
        {
            currentMat1 = changeMat;
            currentMat2 = changeMat;
        }
    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
