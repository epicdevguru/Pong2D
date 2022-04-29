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
        LayerMask _pusherMask;
        [SerializeField]
        MMUIShaker _logoShaker;
        [SerializeField]
        AudioSource _transientAudioSource;
        #endregion
        #region protected Field
        protected Rigidbody2D _rigidBody;
        protected float _lastRaycastTimeStamp = 0f;
        protected Animator _ballAnimator;
        protected int _hitAnimationParameter;
        float _raycastLength = 5f;
        float _bottomHitPower = 2500f;
        float _logoShakeTimeRange = 0.5f;
        #endregion

        #region MonoBehaviour Callbacks
        protected virtual void Awake()
		{
            _rigidBody = this.gameObject.GetComponent<Rigidbody2D>();
            _ballAnimator = this.gameObject.GetComponent<Animator>();
            _hitAnimationParameter = Animator.StringToHash("Hit");
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
        }

        protected virtual void OnCollisionEnter2D(Collision2D collision)
		{
            if (_wallMask == (_wallMask | (1 << collision.gameObject.layer))) {
                HitWall();
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
            _transientAudioSource.volume = intensity;
            StartCoroutine(_logoShaker.Shake(Random.Range(0, _logoShakeTimeRange)));
            _transientAudioSource.Play();
            _ballAnimator.SetTrigger(_hitAnimationParameter);
		}

		#endregion
		#region Public Methods
        public virtual void HitPusher()
		{
            _hitPusherParticles.Play();
            MMVibrationManager.TransientHaptic(0.85f, 0.05f, true, this);
            _transientAudioSource.volume = 0.1f;
            StartCoroutine(_logoShaker.Shake(Random.Range(0, _logoShakeTimeRange)));
            _transientAudioSource.Play();
            _ballAnimator.SetTrigger(_hitAnimationParameter);
        }
		#endregion
	}
}

