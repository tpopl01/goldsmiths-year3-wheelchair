using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace tpopl001.Questing
{
    public class CollectionTriggerDestroyObj : CollectionTrigger
    {
        //Destroys the collectible on collision
        //Used for no hand mode so the user can visually see they have collected it
        //I might make it spawn next to the wheelchair so the user has a better visual indication through a collection pile
        protected override void Process(Collider col)
        {
            base.Process(col);
            if (col.tag.Equals("pickable"))
            {
                Destroy(col.gameObject);
            }
        }
    }
}