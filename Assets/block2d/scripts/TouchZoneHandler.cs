using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AnyMind
{
    public class TouchZoneHandler : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler
    {
		#region Serialization Field
		[SerializeField]
        RenderMode _parentCanvasRenderMode;
        [SerializeField]
        RectTransform _ballMover;
        #endregion

        #region Protected Field
        protected bool _holding = false;
        protected PointerEventData _pointerEventData;
        protected Vector3 _newPosition;
        protected Canvas _canvas;
        protected float _initialZPosition;
        protected Vector2 _workPosition;
		#endregion
		#region MonoBehaviour Callbacks
		// Start is called before the first frame update
		void Start()
        {
            Initialization();
        }

        // Update is called once per frame
        void Update()
        {
            HandleRealtimePosition();
        }
		#endregion
		#region Private Methods
        protected virtual void Initialization()
		{
            _parentCanvasRenderMode = GetComponentInParent<Canvas>().renderMode;
            _canvas = GetComponentInParent<Canvas>();
            _initialZPosition = transform.position.z;
		}

        protected virtual Vector3 GetWorldPosition(Vector3 testPosition)
		{
            if (_parentCanvasRenderMode == RenderMode.ScreenSpaceCamera)
			{
                RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas.transform as RectTransform, 
                    testPosition, _canvas.worldCamera, out _workPosition);
                return _canvas.transform.TransformPoint(_workPosition);
			} else
			{
                return testPosition;
			}
		}

        protected virtual void HandleRealtimePosition()
		{
            if (_holding)
			{
                _newPosition = GetWorldPosition(_pointerEventData.position);
			} else
			{
                _newPosition = Vector3.one * 5000f;
			}
            _newPosition.z = _initialZPosition;
            _ballMover.position = _newPosition;
		}
		#endregion
		#region IPointerExitHandler & IPointerEnterHandler event receiver
		public virtual void OnPointerEnter(PointerEventData data)
		{
            _holding = true;
            _pointerEventData = data;
		}
        public virtual void OnPointerExit(PointerEventData data)
		{
            _holding = false;
		}
		#endregion

	}
}

