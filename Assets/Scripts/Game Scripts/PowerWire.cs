using Monumentum.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Monumentum
{
    public class PowerWire : MonoBehaviour, IPowerReactable
    {
        public Directions directions;

        public bool attachWithBlock;

        public GameObject upPowerEffect;
        public GameObject rightPowerEffect;
        public GameObject downPowerEffect;
        public GameObject leftPowerEffect;

        /// <summary>
        /// 전기를 가합니다.
        /// </summary>
        /// <param name="dir">전기를 작용하는 방향입니다.</param>
        /// <returns>밖으로 나가는 전기의 방향을 반환합니다.</returns>
        public Directions ForcePower(SoleDir dir)
        {
            bool isTurnedOn = directions.TrySubtract(dir, out Directions output);
            Debug.Log("전류 받음");
            if (isTurnedOn) ;
                //켜졌다면;
            return output;
        }

        public Vector3 Position => transform.position;

        public void OnRotate(bool isClockwise = true)
        {
            if (attachWithBlock)
                directions.Rotate(isClockwise);
        }
    }

    public interface IPowerReactable
    {
        Directions ForcePower(SoleDir dir);

        Vector3 Position { get; }
    }
}