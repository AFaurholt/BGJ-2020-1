using UnityEngine;

public class NoiseTransform : MonoBehaviour{

    public Transform target;
    Vector3 rotation = Vector3.zero;
    Vector3 position = Vector3.zero;

    public bool noiseRotation = true;
    public bool noisePosition = false;

    public float rotationMagnitude = 40f;
    public int rotationResolution = 1000;
    public float rotationDampening = 0;

    public float positionMagnitude = 1f;
    public int positionResolution = 100;
    public float positionDampening = 0;

    public bool offsetOlderRotation = false;
    Quaternion storedRotation;
    public bool offsetOlderPosition = false;
    Vector3 storedPosition;
    public Transform offsetParent = null;
    Vector3 parentOffset;

    int timeRotation = 0;
    int timePosition = 0;

    float offsetXrot;
    float offsetYrot;
    float offsetZrot;
    

    float offsetXpos;
    float offsetYpos;
    float offsetZpos;

    float xCoord, yCoord, zCoord;

    private void Start()
    {
        if (target == null)
        {
            target = GetComponent<Transform>();
        }
        if (offsetParent != null)
        {
            parentOffset = target.position - offsetParent.position;
        }
        updateRotation();
        updatePosition();
        
        offsetXrot = Random.Range(100, 9999);
        offsetYrot = offsetXrot + Random.Range(100, 9999);
        offsetZrot = offsetYrot + Random.Range(100, 9999);

        offsetXpos = Random.Range(100, 9999);
        offsetYpos = offsetXpos + Random.Range(100, 9999);
        offsetZpos = offsetYpos + Random.Range(100, 9999);
    }

    public void updateRotation()
    {
        storedRotation = transform.rotation;
    }
    public void updateRotation(Transform t)
    {
        storedRotation = t.rotation;
    }
    public void setUsingOffsetOlderRotation(bool x)
    {
        offsetOlderRotation = x;
    }

    public void updatePosition()
    {
        storedPosition = transform.position;
    }
    public void updatePosition(Transform t)
    {
        storedPosition = t.position;
    }
    public void setUsingOffsetOlderPosition(bool x)
    {
        offsetOlderPosition = x;
    }

    void LateUpdate()
    {
        if(offsetParent != null)
        {
            storedPosition = offsetParent.position + parentOffset;
        }

        if (noiseRotation)
            rotateNoise();
        if (noisePosition)
            positionNoise();
    }

    void positionNoise()
    {
        xCoord = (float)timePosition / positionResolution * Mathf.PI * 2;
        yCoord = (float)timePosition / positionResolution * Mathf.PI * 2;
        zCoord = (float)timePosition / positionResolution * Mathf.PI * 2;

        timePosition = (timePosition + 1) % positionResolution;

        position.x = Mathf.PerlinNoise(offsetXpos + Mathf.Cos(xCoord), offsetXpos + Mathf.Sin(xCoord)) * positionMagnitude - positionMagnitude / 2;
        position.y = Mathf.PerlinNoise(offsetYpos + Mathf.Cos(yCoord), offsetYpos + Mathf.Sin(yCoord)) * positionMagnitude - positionMagnitude / 2;
        position.z = Mathf.PerlinNoise(offsetZpos + Mathf.Cos(zCoord), offsetZpos + Mathf.Sin(zCoord)) * positionMagnitude - positionMagnitude / 2;


        if (positionDampening > 0)
        {
            position.x *= 1 / (positionDampening + 1);
            position.y *= 1 / (positionDampening + 1);
            position.z *= 1 / (positionDampening + 1);
        }

        if (!offsetOlderPosition)
            target.position += position;
        else
            target.position = storedPosition + position;
    }

    void rotateNoise()
    {
        xCoord = (float)timeRotation / rotationResolution * Mathf.PI * 2;
        yCoord = (float)timeRotation / rotationResolution * Mathf.PI * 2;
        zCoord = (float)timeRotation / rotationResolution * Mathf.PI * 2;

        timeRotation = (timeRotation + 1) % rotationResolution;

        rotation.x = Mathf.PerlinNoise(offsetXrot + Mathf.Cos(xCoord), offsetXrot + Mathf.Sin(xCoord)) * rotationMagnitude - rotationMagnitude / 2;
        rotation.y = Mathf.PerlinNoise(offsetYrot + Mathf.Cos(yCoord), offsetYrot + Mathf.Sin(yCoord)) * rotationMagnitude - rotationMagnitude / 2;
        rotation.z = Mathf.PerlinNoise(offsetZrot + Mathf.Cos(zCoord), offsetZrot + Mathf.Sin(zCoord)) * rotationMagnitude - rotationMagnitude / 2;

        if (rotationDampening > 0)
        {
            rotation.x *= 1 / (rotationDampening + 1);
            rotation.y *= 1 / (rotationDampening + 1);
            rotation.z *= 1 / (rotationDampening + 1);
        }

        if (!offsetOlderRotation)
            target.rotation = transform.rotation * Quaternion.Euler(rotation * Time.fixedDeltaTime);
        else
            target.rotation = storedRotation * Quaternion.Euler(rotation * Time.fixedDeltaTime);
    }

}