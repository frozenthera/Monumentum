using Gamekit3D;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monumentum
{
    [RequireComponent(typeof(BoxCollider))]
    public class RoomController : MonoBehaviour
    {
        static RoomController m_CurrentRoom;
        static Vector3 m_SavedPosition;

        public RoomController roomOriginPrefab;

        void Start()
        {
            if (roomOriginPrefab == null)
            {
                Debug.LogError("Fill RoomOriginPrefab");
                //m_RoomOriginPrefab = Instantiate(this);
            }
        }

        private void Update()
        {
            
        }

        public static void Save(RoomController room)
        {
            m_SavedPosition = PlayerInput.inst.transform.position;
            m_CurrentRoom = room;
        }

        public static void Load()
        {
            if (m_CurrentRoom != null)
            {
                int childNum = m_CurrentRoom.transform.childCount;
                for(int i = childNum - 1; i >= 0; i--)
                {
                    Destroy(m_CurrentRoom.transform.GetChild(i).gameObject);
                }

                int newChildNum = m_CurrentRoom.roomOriginPrefab.transform.childCount;
                for (int i = newChildNum - 1; i >= 0; i--)
                {
                    Instantiate(m_CurrentRoom.roomOriginPrefab.transform.GetChild(i), m_CurrentRoom.transform);
                }

                GameObject player = PlayerInput.inst.gameObject;

                player.GetComponent<CharacterController>().enabled = false;
                player.transform.position = m_SavedPosition;
                player.GetComponent<CharacterController>().enabled = true;

                //GameObjectTeleporter.Teleport(PlayerInput.inst.gameObject, m_SavedPosition);
                //Debug.Log(m_SavedPosition);
                //Debug.Log(PlayerInput.inst.gameObject.transform.position);
                //Instantiate(m_CurrentRoom.roomOriginPrefab);
                //Destroy(m_CurrentRoom.gameObject);
            }
        }

        void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                Save(this);
            }
        }
    }
}