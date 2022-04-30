using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AnyMind
{
    public class BonusItem : MonoBehaviour
    {
		#region Serialized Fields
		[SerializeField]
        int _bonusKind;
        [SerializeField]
        float _duration;
		#endregion

		#region Collision Event handler
		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (collision.gameObject.layer == LayerMask.NameToLayer("Pusher"))
			{
				collision.gameObject.GetComponent<BallPusher>().ReceiveBonus(_bonusKind, _duration);
			}
			Destroy(gameObject);
		}
		#endregion
	}

}

