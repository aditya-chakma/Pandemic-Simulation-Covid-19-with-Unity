using UnityEngine;

public class RandVectUtil
{
    public Vector3 GetRandVect2D()
    {
        float x= Random.Range(-1.0f,1.0f);
        float y= Random.Range(-1.0f,1.0f);
        Vector3 result = new Vector3(x,y,0);
        return result;
    }
}
