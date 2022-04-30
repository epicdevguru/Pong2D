using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MoreMountains.NiceVibrations;
namespace AnyMind
{
    /****************************************************
     * BallController is the controller script of the ball.
     * It handles the ball's physics2D behavior and 
     * corresponding events such as sound, vibration etc.
     * *************************************************/
    public class BallController : MonoBehaviour
    {
        #region Serialization Field
        [SerializeField]
        bool _hapticsEnabled = true;
        [SerializeField]
        ParticleSystem _hitParticles;
        [SerializeField]
        ParticleSystem _hitPusherParticles;
        [SerializeField]
        LayerMask _wallMask;
        [SerializeField]
        LayerMask _floorMask;
        [SerializeField]
        LayerMask _pusherMask;
        [SerializeField]
        MMUIShaker _logoShaker;
        [SerializeField]
        AudioSource _hitAudioSource;
        [SerializeField]
        AudioSource _missAudioSource;
        [SerializeField]
        BallManager _manager;
        [SerializeField]
        float UNITY_BUG_AVOID_Y = 0f;
        [SerializeField]
        RectTransform _rectTrans;
        [SerializeField]
        Vector2 _v2InitialPos = new Vector2(0, 700f);
        #endregion
        #region protected Field
        protected Rigidbody2D _rigidBody;
        protected float _lastRaycastTimeStamp = 0f;
        protected Animator _ballAnimator;
        protected int _hitAnimationParameter;
        float _raycastLength = 5f;
        float _bottomHitPower = 2500f;
        float _logoShakeTimeRange = 0.5f;
        int _nSampleCount = 5;
        List<Vector2> _listMoveDirection = new List<Vector2>();
        Vector2 _v2PrevPos;
        #endregion

        #region MonoBehaviour Callbacks
        protected virtual void Awake()
		{
            _rigidBody = this.gameObject.GetComponent<Rigidbody2D>();
            _ballAnimator = this.gameObject.GetComponent<Animator>();
            _hitAnimationParameter = Animator.StringToHash("Hit");
        }

		private void Start()
		{
            InitBallController();
        }
		// Update is called once per frame
		void Update()
        {
            UpdateMoveDirectionList();
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
		{
            if (_wallMask == (_wallMask | (1 << collision.gameObject.layer))) {
                HitWall();
            }
        }

        protected virtual void OnTriggerEnter2D(Collider2D collider)
		{
            if (_floorMask == (_floorMask | (1 << collider.gameObject.layer)))
			{
                HitFloor();
			}
		}

		#endregion
		#region Protected Methods
		protected virtual void HitBottom() // buttom hit function but not used anymore, just saved for reference
		{
			_rigidBody.AddForce(Vector2.up * _bottomHitPower);
			StartCoroutine(_logoShaker.Shake(Random.Range(0, _logoShakeTimeRange)));
		}

        protected virtual void HitWall() //Wall collision hit handling
		{
            float intensity = _rigidBody.velocity.magnitude / 100f;
            if (intensity > 1) intensity = 1;
            MMVibrationManager.TransientHaptic(0.75f, 0.7f, true, this);
            _hitAudioSource.volume = intensity;
            StartCoroutine(_logoShaker.Shake(Random.Range(0, _logoShakeTimeRange)));
            _hitAudioSource.Play();
            _ballAnimator.SetTrigger(_hitAnimationParameter);
		}

        protected void UpdateMoveDirectionList()
		{
            Vector2 v2Direction = (Vector2)_rectTrans.anchoredPosition - _v2PrevPos;
            _listMoveDirection.Add(v2Direction);
            if (_listMoveDirection.Count > _nSampleCount)
			{
                _listMoveDirection.RemoveAt(0);
			}
            _v2PrevPos = (Vector2)_rectTrans.anchoredPosition;

        }
        #endregion
        #region Public Methods
        // whenever a ball hits the ball pusher, animation, vibration, hit sound is played
        public virtual void HitPusher() 
		{
            _hitPusherParticles.Play();
            MMVibrationManager.TransientHaptic(0.85f, 0.05f, true, this);
            _hitAudioSource.volume = 0.1f;
            StartCoroutine(_logoShaker.Shake(Random.Range(0, _logoShakeTimeRange)));
            _hitAudioSource.Play();
            _ballAnimator.SetTrigger(_hitAnimationParameter);
        }

        //when hit the floor collider then lose the life 
        public void HitFloor()
		{
            float intensity = _rigidBody.velocity.magnitude / 100f;
            if (intensity > 1) intensity = 1;
            MMVibrationManager.TransientHaptic(0.75f, 0.7f, true, this);
            _missAudioSource.volume = intensity;
            StartCoroutine(_logoShaker.Shake(Random.Range(0, _logoShakeTimeRange)));
            _missAudioSource.Play();
            _ballAnimator.SetTrigger(_hitAnimationParameter);
            _manager.LoseLife();
        }
        //Ball initialization function for life respawn and level restart etc
        public void InitBallController ()
		{
            _listMoveDirection.Clear();
            _rectTrans.anchoredPosition = _v2InitialPos;
            _v2PrevPos = _rectTrans.anchoredPosition;
            _rigidBody.velocity = Vector2.zero;
		}

        public Vector2 GetMoveDirection(bool bClear=true)
		{
            Vector2 v2Direction = Vector2.zero;
            for (int i = 0; i < _listMoveDirection.Count; i++)
			{
                v2Direction += _listMoveDirection[i];
			}
            int nListCount = _listMoveDirection.Count;
            if (bClear)
			{
                _listMoveDirection.Clear();
                _v2PrevPos = (Vector2)_rectTrans.anchoredPosition;
			}
            return v2Direction / nListCount;
		}
		#endregion
	}
}

