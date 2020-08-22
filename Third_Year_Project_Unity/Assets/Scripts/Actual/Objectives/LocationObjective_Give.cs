using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace tpopl001.Questing
{
    [CreateAssetMenu(menuName = "Quests/Objectives/Location Give")]
    public class LocationObjective_Give : LocationObjective
    {
        public string path;
        void OnDestroy()
        {
            QuestEvents.OnLocationChange -= LocationObject;
        }
        protected override void UpdateCompleted()
        {
            GameObject g = Instantiate<GameObject>(Resources.Load<GameObject>(path));
        //    g.transform.SetParent(GameManager.instance.hmd.transform);
            g.transform.localPosition = Vector3.zero;
            g.transform.localRotation = Quaternion.Euler(Vector3.zero);
            base.UpdateCompleted();
        }
    }
}