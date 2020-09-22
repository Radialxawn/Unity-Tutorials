#pragma warning disable 0649

using System.Collections;
using UnityEngine;

public class BirdController : MonoBehaviour {
	public static BirdController I;

	[SerializeField] private Animator animator;
	[SerializeField] private float glideVelocity = 0.6f;
	[SerializeField] private float jumpVelocity = 1.5f;
	[SerializeField] private float turnSpeed = 2f;
	[SerializeField] private float dashVelocity = 10f;
	[SerializeField] private float dashDistance = 10f;

	private Rigidbody rb;
	private bool alive = true;
	private int aliveHash;
	private int flapHash;
	private Vector3 acceleration;

	private float turnXTarget;
	private float turnXStepLength;
	private float turnXMinStep, turnXMaxStep;

	private bool dashing = false;
	private Coroutine dashCoroutine;
	private float dashElapsedDistance;

	private string[] deadlyTags;

	private Vector3 revivePosition;
	private Quaternion reviveRotation;

	public void SetDeadlyTags(params string[] tags) {
		deadlyTags = tags;
	}

	public void SetRevivePositionAndRotation(Vector3 position, Quaternion rotation) {
		revivePosition = position;
		reviveRotation = rotation;
	}

	public void InitializeSelf() {
		I = this;
		rb = GetComponent<Rigidbody>();
		aliveHash = Animator.StringToHash("Alive");
		flapHash = Animator.StringToHash("Flap");
		animator.SetBool(aliveHash, alive);
		SetTurnLimit(0.8f, -1, 1); // test
		SetRevivePositionAndRotation(rb.position, rb.rotation); // test
	}

	private void FixedUpdate() {
		if (!alive) { return; }

		Vector3 pos = rb.position;
		Vector3 vec = rb.velocity;

		float newX = Mathf.Lerp(pos.x, turnXTarget, Time.fixedDeltaTime * turnSpeed);
		vec.x = (newX - pos.x) / Time.fixedDeltaTime;
		Vector3 turningDirection = new Vector3(turnXTarget - pos.x, 0f, 1f);
		Quaternion turningRotation = Quaternion.LookRotation(turningDirection, Vector3.up);

		float rotationAngle = (90f * (vec.y + 1.5f) / 1.5f) - 90f;
		rotationAngle = rotationAngle < -90f ? -90f : rotationAngle > 40f ? 40f : rotationAngle;
		Quaternion fallingRotation = Quaternion.AngleAxis(rotationAngle, Vector3.left);

		rb.rotation = turningRotation * fallingRotation;
		rb.velocity = vec;
		rb.AddForce(acceleration, ForceMode.Acceleration);

		Debug.DrawRay(transform.position, turningDirection, Color.red);
	}

	public void Glide() {
		if (alive) {
			rb.velocity = rb.velocity.SetZ(glideVelocity);
			acceleration.y = -Physics.gravity.y;
			animator.SetBool(flapHash, false);
		}
	}

	public void Jump() {
		if (alive) {
			rb.velocity = rb.velocity.SetY(jumpVelocity);
			acceleration.y = 0f;
			animator.SetBool(flapHash, true);
		}
	}

	public void SetTurnLimit(float turnXStepLength, float turnXMinStep, float turnXMaxStep) {
		this.turnXStepLength = turnXStepLength;
		this.turnXMinStep = turnXMinStep;
		this.turnXMaxStep = turnXMaxStep;
	}

	public void TurnLeft() {
		if (alive) {
			turnXTarget = (turnXTarget - turnXStepLength).Snap(turnXStepLength).Clamp(turnXMinStep * turnXStepLength, turnXMaxStep * turnXStepLength);
			Debug.Log("TurnLeft");
		}
	}

	public void TurnRight() {
		if (alive) {
			turnXTarget = (turnXTarget + turnXStepLength).Snap(turnXStepLength).Clamp(turnXMinStep * turnXStepLength, turnXMaxStep * turnXStepLength);
			Debug.Log("TurnRight");
		}
	}

	public void Dash() {
		if (alive && !dashing) {
			dashing = true;
			dashElapsedDistance = 0f;
			rb.velocity = rb.velocity.SetZ(dashVelocity);
			if (dashCoroutine != null) { StopCoroutine(dashCoroutine); }
			dashCoroutine = StartCoroutine(DashCoroutine());
		}
	}

	private WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
	private IEnumerator DashCoroutine() {
		while (dashElapsedDistance < dashDistance) {
			dashElapsedDistance += Mathf.Abs(rb.velocity.z) * Time.fixedDeltaTime;
			yield return waitForFixedUpdate;
		}
		dashing = false;
		yield break;
	}

	private void OnCollisionEnter(Collision collision) {
		if (alive) {
			Collider cld = collision.collider;
			for (int i = 0; i < deadlyTags.Length; i++) {
				if (cld.CompareTag(deadlyTags[i])) {
					Die();
					break;
				}
			}
		}
	}

	public System.Action OnDead;
	private void Die() {
		alive = false;
		animator.SetBool(aliveHash, false);
		OnDead?.Invoke();
		Debug.Log("Die");
	}

	public void Revive() {
		alive = true;
		animator.SetBool(aliveHash, true);
		dashing = false;
		if (dashCoroutine != null) { StopCoroutine(dashCoroutine); }
		rb.position = revivePosition;
		rb.rotation = reviveRotation;
		Glide();
		Debug.Log("Revive");
	}
}