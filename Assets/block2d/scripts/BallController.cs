using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.NiceVibrations;
namespace AnyMind
{
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
        Vector3 _v3InitialPos;
        #endregion

        #region MonoBehaviour Callbacks
        protected virtual void Awake()
		{
            _rigidBody = this.gameObject.GetComponent<Rigidbody2D>();
            _ballAnimator = this.gameObject.GetComponent<Animator>();
            _hitAnimationParameter = Animator.StringToHash("Hit");
            _v3InitialPos = transform.position;
            InitBallController();
        }

        // Update is called once per frame
        void Update()
        {
            Debug.DrawLine(this.transform.position, Vector3.down * _raycastLength, Color.red);

            if ((Time.time - _lastRaycastTimeStamp) > 1f)
			{
                _lastRaycastTimeStamp = Time.time;
                RaycastHit2D hit = Physics2D.Raycast(this.transform.position, Vector3.down, _raycastLength, _wallMask);
                if (hit.collider != null)
				{
                    HitBottom();
				}
			}
            UpdateMoveDirectionList();
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
		{
            if (_wallMask == (_wallMask | (1 << collision.gameObject.layer))) {
                HitWall();
            }
		}

        protected virtual void OnTriggerEnter2D (Collider2D collider)
		{
            if (_floorMask == (_floorMask | (1 << collider.gameObject.layer)))
			{
                HitFloor();
			}
		}
		#endregion
		#region Protected Methods
		protected virtual void HitBottom()
		{
			_rigidBody.AddForce(Vector2.up * _bottomHitPower);
			StartCoroutine(_logoShaker.Shake(Random.Range(0, _logoShakeTimeRange)));
		}

        protected virtual void HitWall()
		{
            float intensity = _rigidBody.velocity.magnitude / 100f;
            MMVibrationManager.TransientHaptic(intensity, 0.7f, true, this);
            _hitAudioSource.volume = intensity;
            StartCoroutine(_logoShaker.Shake(Random.Range(0, _logoShakeTimeRange)));
            _hitAudioSource.Play();
            _ballAnimator.SetTrigger(_hitAnimationParameter);
		}

        protected virtual void HitFloor()
		{
            float intensity = _rigidBody.velocity.magnitude / 50f;
            MMVibrationManager.TransientHaptic(intensity, 0.7f, true, this);
            _missAudioSource.volume = intensity;
            StartCoroutine(_logoShaker.Shake(Random.Range(0, _logoShakeTimeRange)));
            _missAudioSource.Play();
            _ballAnimator.SetTrigger(_hitAnimationParameter);
            _manager.LoseLife();
        }

        protected void UpdateMoveDirectionList()
		{
            Vector2 v2Direction = (Vector2)transform.position - _v2PrevPos;
            _listMoveDirection.Add(v2Direction);
            if (_listMoveDirection.Count > _nSampleCount)
			{
                _listMoveDirection.RemoveAt(0);
			}
            _v2PrevPos = (Vector2)transform.position;

        }
		#endregion
		#region Public Methods
        public virtual void HitPusher()
		{
            _hitPusherParticles.Play();
            MMVibrationManager.TransientHaptic(0.85f, 0.05f, true, this);
            _hitAudioSource.volume = 0.1f;
            StartCoroutine(_logoShaker.Shake(Random.Range(0, _logoShakeTimeRange)));
            _hitAudioSource.Play();
            _ballAnimator.SetTrigger(_hitAnimationParameter);
        }

        public void InitBallController ()
		{
            _listMoveDirection.Clear();
            transform.position = _v3InitialPos;
            _v2PrevPos = transform.position;
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
                _v2PrevPos = (Vector2)transform.position;
			}
            return v2Direction / nListCount;
		}
		#endregion
	}
}

