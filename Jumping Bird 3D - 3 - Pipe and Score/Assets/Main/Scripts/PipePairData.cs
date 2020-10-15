using UnityEngine;

[CreateAssetMenu(fileName = "PipePairData", menuName = "Jumping Bird 3D/PipePairData")]
public class PipePairData : ScriptableObject {
	[SerializeField] private float suckUpAcceleration = 1.5f;
	[SerializeField] private float suckDownAcceleration = -2f;
	[SerializeField] private float openDistance = 0.52f;
	[SerializeField] private float suckingDistance = 0.64f;
	[SerializeField] private float closeDistance = 0.2f;
	[SerializeField] private float outDistance = 10f;
	[SerializeField] private float closeRate = 0.1f;
	[SerializeField] private float suckRate = 0.2f;
	[SerializeField] private float fadeInSpeed = 2f;
	[SerializeField] private float fadeOutSpeed = 1f;

	public float SuckUpAcceleration { get { return suckUpAcceleration; } }
	public float SuckDownAcceleration { get { return suckDownAcceleration; } }
	public float OpenDistance { get { return openDistance; } }
	public float SuckingDistance { get { return suckingDistance; } }
	public float CloseDistance { get { return closeDistance; } }
	public float OutDistance { get { return outDistance; } }
	public float CloseRate { get { return closeRate; } }
	public float SuckRate { get { return suckRate; } }
	public float FadeInSpeed { get { return fadeInSpeed; } }
	public float FadeOutSpeed { get { return fadeOutSpeed; } }
}