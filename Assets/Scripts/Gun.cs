using System.Collections;
using UnityEngine;


[RequireComponent(typeof(AudioSource))]
public class Gun : MonoBehaviour {

	public enum GunType {
		Burst,
		Automatic,
		SingleFire
	}

	public GunType gunType;
	public float rpm;

	//Components
	public Transform spawn;
	public LineRenderer tracer;
	public Light light;

	//System Variables
	private float secondsBetweenShots;
	private float nextPossibleShootTime;

	private void Start() {
		secondsBetweenShots = 60 / rpm;
		if (GetComponent<LineRenderer>()) {
			tracer = GetComponent<LineRenderer>();
		}
		if (GetComponent<Light>()) {
			light = GetComponent<Light>();
		}
	}

	public void Shoot() {
		if (CanShoot()) {
			Ray ray = new Ray(spawn.position, spawn.forward);
			RaycastHit hit;

			float shotDistance = 20;

			if (Physics.Raycast(ray, out hit, shotDistance)) {
				shotDistance = hit.distance;
			}

			nextPossibleShootTime = Time.time + secondsBetweenShots;

			GetComponent<AudioSource>().Play();

			if (tracer) {
				StartCoroutine("RenderTracer", ray.direction * shotDistance);
			}
		}
	}

	public void ShootAutomatic() {
		if (gunType == GunType.Automatic) {
			Shoot();
		}
	}

	private bool CanShoot() {
		bool canShoot = true;

		if (Time.time < nextPossibleShootTime) {
			canShoot = false;
		}

		return canShoot;
	}

	IEnumerator RenderTracer(Vector3 hitPoint) {
		tracer.enabled = true;
		light.enabled = true;
		tracer.SetPosition(0, spawn.position);
		tracer.SetPosition(1, spawn.position + hitPoint);

		yield return null;
		tracer.enabled = false;
		light.enabled = false;
	}
}
