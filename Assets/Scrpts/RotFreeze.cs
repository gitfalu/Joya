using UnityEngine;

public class RotFreeze : MonoBehaviour
{
    [SerializeField]
    private bool _local = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(_local)
            transform.localRotation = Quaternion.identity;
        else
            transform.rotation = Quaternion.identity;
    }
}
