using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LimbRemoval : MonoBehaviour
{

    private const int ObstacleLayerInt = 8;


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == ObstacleLayerInt)
        {

            gameObject.transform.parent = null;
            if (gameObject.transform.parent == null)
                Destroy(gameObject);
            this.GetComponent<LimbRemoval>().enabled = false;

        }
    }

}
