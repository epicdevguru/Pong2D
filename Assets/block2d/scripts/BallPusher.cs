using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnyMind
{
    /***************************************************
     * BallPusher is pushs the ball whenever hit with it.
     * It gives physical force to the ball whenever hitting,
     * and collect the bonus items dropped from the broken
     * bricks.
     * *************************************************/
    public class BallPusher : MonoBehaviour
    {
        #region Serialization Field
        [SerializeField]
        float _force = 5f;
        [SerializeField]
        float _initialForce = 1f;
        [SerializeField]
        bool _isMoved = false;
        [SerializeField]
        BallManager _manager;
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
        int _currentBonus = -1;
        float _fBonusCount = 0;
        float _fBonusDuration = 0;
		#endregion
		#region MonoBehaviour Callbacks
		private void Start()
		{
            _v3InitialPos = transform.position;
		}

		private void Update()
		{
			if (_currentBonus > -1)
			{
                if (_fBonusCount <= _fBonusDuration)
				{
                    _fBonusCount += Time.deltaTime;
                    if (_fBonusCount > _fBonusDuration)
					{
                        ClearBonusBuff();
                    }
				} 
			}
		}
		#endregion
		#region Physics2D event Handler
		protected virtual void OnTriggerEnter2D(Collider2D collider)
		{
            if (collider.gameObject != _targetBall.gameObject)
			{
                return;
			}
            float pushForce = _initialForce;
            if (_isMoved)
            {
                pushForce = _force;
            }
            if (_manager.IsWaitStatus())
			{
                _direction = new Vector2(0, 1f);
                pushForce = _initialForce;
            } else
			{
                Vector2 v2PosOffSet = (collider.transform.position - this.transform.position).normalized;
                Vector2 moveDirection = _targetBall.GetMoveDirection();
                _direction = new Vector2(moveDirection.x, -moveDirection.y);
                _direction = (_direction.normalized + v2PosOffSet * 0.3f).normalized;
            }

            collider.attachedRigidbody.velocity = Vector2.zero;
            
            collider.attachedRigidbody.AddForce(_direction * pushForce);
            _targetBall.HitPusher();
		}
		#endregion
		#region Public Methods
		public void InitBallPusher ()
		{
            _isMoved = false;
            transform.position = _v3InitialPos;
            ClearBonusBuff();

        }

        public void BallPusherMoved()
		{
            if (!_isMoved)
			{
                _isMoved = true;
			}
		}

        public void ReceiveBonus(int effectKind, float fTime)
		{
            _currentBonus = effectKind;
            _fBonusDuration = fTime;
            _fBonusCount = 0;
            switch (effectKind)
			{
                case 0:
                    transform.localScale = new Vector3(2f, 1f, 1f);
                    break;
                case 1:
                    transform.localScale = new Vector3(0.5f, 1f, 1f);
                    break;
			}
        }
		#endregion
		#region Private Methods
        private void ClearBonusBuff()
		{
            _currentBonus = -1;
            _fBonusCount = 0;
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
		#endregion
	}
}

