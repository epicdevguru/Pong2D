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
        BallController _targetBall;
        #endregion
        #region Protected Field
        protected Vector2 _direction;
		#endregion

		#region Physics2D event Handler
        protected virtual void OnTriggerEnter2D(Collider2D collider)
		{
            if (collider.gameObject != _targetBall.gameObject)
			{
                return;
			}
            _direction = (collider.transform.position - this.transform.position).normalized;
            _direction.y = 1f;
            collider.attachedRigidbody.velocity = Vector2.zero;
            collider.attachedRigidbody.AddForce(_direction * _force);
            _targetBall.HitPusher();
		}
		#endregion
	}
}

