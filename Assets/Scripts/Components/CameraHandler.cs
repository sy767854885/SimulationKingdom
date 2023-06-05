using Components.Math;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utilities;

namespace Components
{
    public class CameraHandler : MonoBehaviour
    {
        public Transform cameraTransform;
        [Space]
        public float horizontalSpeedData=10;
        [Header("x,z:����-x/2,x/2����-z/2,z/2����y:����0��y��")]
        public Vector3 horizonZone;
        [Space]
        public ClampData localRadiusClampData;//����ľֲ�����ռ�
        public ClampData localAngleClampData;//����ľֲ��ƶ��Ƕȿռ�
        public float angle = -45;
        public float radius = 20;

        private Vector3 tempMousePos = Vector3.zero;

        private void Start()
        {
            
        }

        private void Update()
        {
            HorizontalMove();
            RadiusUpdate();
            AngleUpdate();
            UpdateCameraPos();
        }

        //ˮƽ�ƶ�
        private void HorizontalMove()
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            Vector3 targetPos = transform.position + transform.right * horizontal + transform.forward * vertical + Vector3.up * horizonZone.y;
            targetPos = ClampHorizontalPos(targetPos);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, horizontalSpeedData * Time.deltaTime);
        }

        //ˮƽλ������
        private Vector3 ClampHorizontalPos(Vector3 targetPos)
        {
            Vector3 pos = targetPos;
            pos.x = Mathf.Clamp(pos.x, -horizonZone.x / 2, horizonZone.x / 2);
            pos.z = Mathf.Clamp(pos.z, -horizonZone.z / 2, horizonZone.z / 2);
            pos.y = horizonZone.y;
            return pos;
        }

        //����������������ĵİ뾶
        private void RadiusUpdate()
        {
            float s = Input.GetAxis("Mouse ScrollWheel");
            float sign = s != 0 ? -Mathf.Sign(s) * 10 : 0;
            radius += sign * 10 * Time.deltaTime;
            radius = localRadiusClampData.GetClampValue(radius);
        }

        //��������ĽǶ�
        private void AngleUpdate()
        {
            if (!Input.GetKey(KeyCode.Mouse2)) return;
            Vector3 mP = Input.mousePosition;
            Vector3 offset = mP - tempMousePos;
            tempMousePos = mP;
            
            HorizontalRotate(offset.x);
            VerticalRotate(offset.y);
        }

        //ˮƽ��ת
        private void HorizontalRotate(float inputDir)
        {
            float sign = inputDir != 0 ? Mathf.Sign(inputDir) : 0;
            transform.Rotate(Vector3.up, sign * 150 * Time.deltaTime);
        }

        //������ת
        private void VerticalRotate(float inputDir)
        {
            float signV = inputDir != 0 ? -Mathf.Sign(inputDir) : 0;
            angle += signV * 100 * Time.deltaTime;
            angle = localAngleClampData.GetClampValue(angle);
        }

        private void UpdateCameraPos()
        {
            Vector2 p = VectorTool.AngleRadiusToPosition(angle, radius);

            cameraTransform.localPosition = new Vector3(0, p.x, p.y);
            cameraTransform.LookAt(transform.position, Vector3.up);
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(Vector3.zero + Vector3.up * horizonZone.y, new Vector3(horizonZone.x, 0, horizonZone.z));
        }
    }
}

