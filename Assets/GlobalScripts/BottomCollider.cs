using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using UnityEngine;
using Assets;


namespace Assets
{
    public class BottomCollider:MonoBehaviour
    {
        GameObject MasterGamecharacter;
        private bool isGround = false;
        public bool IsGrounded()
        {
            return isGround;
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.transform.tag == "floor")
                isGround = true;
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.transform.tag == "floor")
                isGround = true;
            else
                isGround = false;
        }

        private void OnTriggerExit(Collider other)
        {
            isGround = false;
        }
        
    }
}
