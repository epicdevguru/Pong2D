using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnyMind
{
    public class BallPusher : MonoBehaviour
    {
        #region Serialization Field
        [SerializeField]
        float _force = 5f;
        [SerializeField]
        float _initialForce = 1f;
        [SerializeField]
        bool _isMoved = false;
        public bool IsMoved
        {
            get { return _isMoved; }
            set { _isMoved = value; }
        }
        [SerializeField]
        BallController _targetBall;
        #endregion
        #region Protected Field
        protected Vector2 _direction;
        Vector3 _v3InitialPos;
		#endregion
		#region MonoBehaviour Callbacks
		private void Start()
		{
            _v3InitialPos = transform.position;
		}
		#endregion
		#region Physics2D event Handler
		protected virtual void OnTriggerEnter2D(Collider2D collider)
		{
            if (collider.gameObject != _targetBall.gameObject)
			{
                return;
			}
            //_direction = (collider.transform.position - this.transform.position).normalized;
            //_direction.y = 1f;
            Vector2 v2PosOffSet = (collider.transform.position - this.transform.position).normalized;
            Vector2 moveDirection = _targetBall.GetMoveDirection();
            _direction = new Vector2(moveDirection.x, -moveDirection.y);
            _direction = (_direction.normalized + v2PosOffSet * 0.3f).normalized;
            collider.attachedRigidbody.velocity = Vector2.zero;
            float pushForce = _initialForce;
            if (_isMoved)
			{
                pushForce = _force;
			}
            collider.attachedRigidbody.AddForce(_direction * pushForce);
            _targetBall.HitPusher();
		}
		#endregion

        public void InitBallPusher ()
		{
            _isMoved = false;
            transform.position = _v3InitialPos;
		}

        public void BallPusherMoved()
		{
            if (!_isMoved)
			{
                _isMoved = true;
			}
		}

    }
}

