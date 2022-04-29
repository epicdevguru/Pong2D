using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AnyMind
{
    public class WallHandler : MonoBehaviour
    {
		#region protected Field
		protected RectTransform _rectTransform;
		protected BoxCollider2D _boxCollider2D;
		#endregion
		#region Protected Methods
		void InitializeColliderSize()
		{
			_rectTransform = this.gameObject.GetComponent<RectTransform>();
			_boxCollider2D = this.gameObject.GetComponent<BoxCollider2D>();
			_boxCollider2D.size = new Vector2(_rectTransform.rect.size.x, _rectTransform.rect.size.y);
		}
		#endregion

		#region MonoBehaviour Callbacks
		protected virtual void OnEnable()
		{
			InitializeColliderSize();
		}

		private void Awake()
		{
			InitializeColliderSize();
		}
		#endregion
	}
}

