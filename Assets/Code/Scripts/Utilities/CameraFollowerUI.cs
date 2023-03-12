using UnityEngine;
public class CameraFollowerUI : MonoBehaviour
{
    [SerializeField] private float rotateSpeed = 5f;
    private Transform camTransform;
    private Quaternion rotation;
    private void Start(){
        camTransform = Camera.main.transform;
    }
    public void Update()
    {
        rotation = Quaternion.Slerp(rotation,camTransform.rotation,rotateSpeed * Time.deltaTime);
        this.transform.rotation = rotation;
    }
}