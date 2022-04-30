using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;
namespace AnyMind
{
    public class BrickUnit : MonoBehaviour
    {
        #region Serialization Field
        [SerializeField]
        Image _imgBrick;
        [SerializeField]
        Color[] _arrColors;
        [SerializeField]
        int _life;
        public int Life
		{
            get { return _life; }
            set { _life = value; }
		}
        [SerializeField]
        LayerMask _ballMask;
        [SerializeField]
        GameObject _breakParticle;
        [SerializeField]
        MMUIShaker _shaker;

        #endregion
        #region Private Field
        BallManager _manager;
		#endregion
		#region Public Methods
		public void InitBrick(int life,  BallManager manager)
		{
            _life = life;
            _imgBrick.color = _arrColors[_life - 1];
            _manager = manager;
		}

        #endregion
        #region Physics collision Detection Event
        void OnCollisionEnter2D(Collision2D collision)
		{
            // Check if the collided object is ball
            if (_ballMask == (_ballMask | (1 << collision.gameObject.layer))) 
			{
                _life--;
                collision.gameObject.GetComponent<BallController>().HitPusher();
                if (_life > 0)
				{
                    StartCoroutine(_shaker.Shake(Random.Range(0, 0.5f)));
                    _imgBrick.color = _arrColors[_life - 1];
                }
                else
				{
                    Vector3 v3ParticlePos = transform.localPosition;
                    v3ParticlePos.z = -100f;
                    GameObject objParticle = Instantiate(_breakParticle, transform.parent);
                    objParticle.transform.localPosition = v3ParticlePos;
                    _manager.DestroyBrick(this);
                }
			}
		}
        #endregion
    }
}

