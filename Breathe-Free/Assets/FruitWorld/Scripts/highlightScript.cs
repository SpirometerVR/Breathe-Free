using UnityEngine;
using System.Collections.Generic;

public class highlightScript : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private string selectableTag = "selectable";
    [SerializeField] private Material highlightedMaterial;
    [SerializeField] private Material defaultMaterial;
    [SerializeField] private Camera cam;
    

    private Transform _selection;


    private void Awake()
    {
        OVRCameraRig cameraRig = gameObject.GetComponent<OVRCameraRig>();
        
        if (cameraRig == null)
        {
            cameraRig = gameObject.AddComponent<OVRCameraRig>();
            cameraRig.disableEyeAnchorCameras = true;  // this line disables the cameras on the CameraRig, and let the project uses the original camera for VR rendering
            Debug.Log("OVRCameraRig injected");
        }
    }
    void Start()
    {
        cam = GameObject.Find("OVRCameraRig").transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.GetComponent<Camera>();
        //Debug.Log(GameObject.Find("OVRCameraRig").transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.name);
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (_selection != null)
        {
            var selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material = defaultMaterial;
            _selection = null;
        }
        
        //var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 center = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, Camera.main.nearClipPlane));
        RaycastHit hit;

        //if (Physics.Raycast(ray, out hit))
        if (Physics.Raycast(center,this.transform.forward,out hit,1000))
        {
            var selection = hit.transform;
            if(selection.CompareTag(selectableTag))
            {
                var selectionRenderer = selection.GetComponent<Renderer>();
                if (selectionRenderer != null)
                {
                    selectionRenderer.material = highlightedMaterial;
                }
                _selection = selection;

            }


        }


    }
}
