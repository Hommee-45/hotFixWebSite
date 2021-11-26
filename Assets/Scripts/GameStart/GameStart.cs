using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PureMVC.Tutorial
{


    public class GameStart : MonoBehaviour
    {
        private void Awake()
        {
            ApplicationFacade.Instance.StartUpHandle();
        }
        // Start is called before the first frame update
        void Start()
        {
            ApplicationFacade.Instance.GameStartHandle();
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

