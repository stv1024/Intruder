//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2014 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Tween the object's alpha.
/// </summary>

[AddComponentMenu("NGUI/Tween/Tween Alpha 2D")]
public class TweenAlpha2D : UITweener
{
#if UNITY_3_5
	public float from = 1f;
	public float to = 1f;
#else
	[Range(0f, 1f)] public float from = 1f;
	[Range(0f, 1f)] public float to = 1f;
#endif

    SpriteRenderer mRect;

	public SpriteRenderer cachedRect
	{
		get
		{
			if (mRect == null)
			{
                mRect = GetComponent<SpriteRenderer>();
                if (mRect == null) mRect = GetComponentInChildren<SpriteRenderer>();
			}
			return mRect;
		}
	}

    /// <summary>
    /// Tween's current value.
    /// </summary>

    public float value
    {
        get { return cachedRect.color.a; }
        set
        {
            var color = cachedRect.color;
            color.a = value;
            cachedRect.color = color;
        }
    }

    /// <summary>
	/// Tween the value.
	/// </summary>

	protected override void OnUpdate (float factor, bool isFinished) { value = Mathf.Lerp(from, to, factor); }

	/// <summary>
	/// Start the tweening operation.
	/// </summary>

	static public TweenAlpha2D Begin (GameObject go, float duration, float alpha)
	{
		var comp = UITweener.Begin<TweenAlpha2D>(go, duration);
		comp.from = comp.value;
		comp.to = alpha;

		if (duration <= 0f)
		{
			comp.Sample(1f, true);
			comp.enabled = false;
		}
		return comp;
	}

	public override void SetStartToCurrentValue () { from = value; }
	public override void SetEndToCurrentValue () { to = value; }
}
