using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AnyMind
{
	/*************************************************
	 * The BonusItems are generated from bricks.
	 * It collides with ball pusher or floor layer.
	 * If it collides with Ball pusher then it sends
	 * the info to Ball pusher. 
	 * **********************************************/
    public class BonusItem : MonoBehaviour
    {
		#region Serialized Fields
		[SerializeField]
        int _bonusKind;
        [SerializeField]
        float _duration;
		[SerializeField]
		LayerMask _pusherMask;
		#endregion

		#region Collision Event handler
		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (_pusherMask == (_pusherMask | (1 << collision.gameObject.layer)))
			{
				collision.gameObject.GetComponent<BallPusher>().ReceiveBonus(_bonusKind, _duration);
			}
			Destroy(gameObject);
		}
		#endregion
	}

}

