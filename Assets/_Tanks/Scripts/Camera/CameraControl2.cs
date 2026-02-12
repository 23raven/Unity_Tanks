using UnityEngine;

namespace Tanks.Complete
{
    public class CameraControl2 : MonoBehaviour
    {
        [Header("Targets")]
        public Transform[] targets;

        [Header("Movement")]
        public float moveSmoothTime = 0.2f;

        [Header("Zoom")]
        public float zoomSmoothTime = 0.2f;
        public float screenEdgeBuffer = 1.5f;
        public float minSize = 7f;
        public float maxSize = 18f; // 🔥 ограничение максимального отдаления

        private Camera cam;
        private Vector3 moveVelocity;
        private float zoomVelocity;

        private void Awake()
        {
            cam = GetComponentInChildren<Camera>();
        }

        private void FixedUpdate()
        {
            Move();
            Zoom();
        }

        // ================= MOVE =================

        private void Move()
        {
            Vector3 centerPoint = GetCenterPoint();

            transform.position = Vector3.SmoothDamp(
                transform.position,
                new Vector3(centerPoint.x, transform.position.y, centerPoint.z),
                ref moveVelocity,
                moveSmoothTime
            );
        }

        private Vector3 GetCenterPoint()
        {
            if (targets.Length == 1)
                return targets[0].position;

            Bounds bounds = new Bounds(targets[0].position, Vector3.zero);

            for (int i = 0; i < targets.Length; i++)
            {
                if (!targets[i].gameObject.activeSelf)
                    continue;

                bounds.Encapsulate(targets[i].position);
            }

            return bounds.center;
        }

        // ================= ZOOM =================

        private void Zoom()
        {
            float requiredSize = FindRequiredSize();

            cam.orthographicSize = Mathf.SmoothDamp(
                cam.orthographicSize,
                requiredSize,
                ref zoomVelocity,
                zoomSmoothTime
            );
        }

        private float FindRequiredSize()
        {
            Bounds bounds = new Bounds(targets[0].position, Vector3.zero);

            for (int i = 0; i < targets.Length; i++)
            {
                if (!targets[i].gameObject.activeSelf)
                    continue;

                bounds.Encapsulate(targets[i].position);
            }

            float size = Mathf.Max(bounds.size.z, bounds.size.x / cam.aspect);

            size *= 0.5f;
            size += screenEdgeBuffer;

            // 🔥 главный фикс — ограничиваем zoom
            size = Mathf.Clamp(size, minSize, maxSize);

            return size;
        }
    }
}
