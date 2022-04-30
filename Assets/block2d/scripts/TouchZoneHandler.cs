using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace AnyMind
{
    /****************************************************************
     * It is the script that handles the touch zone of the game.
     * (the black area, where the player can drag ball pusher object,
     * by their fingers. There are point event handlers and
     * ball pusher's new position calculation functions.
     * ************************************************************/
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
        protected BallPusher _ballPusher;
        protected bool _pauseListener = true;
        public bool PauseListener
		{
            get { return _pauseListener; }
            set { _pauseListener = value; }
		}
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
            _ballPusher = _ballMover.GetComponent<BallPusher>();
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
                if (!_ballPusher.IsMoved)
				{
                    _ballPusher.IsMoved = true;
				}
			}
            if (_pauseListener)
            {
                _newPosition.x = 0;
                return;
            }
            _ballMover.position = new Vector3(_newPosition.x, _ballMover.position.y, _ballMover.position.z);
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

