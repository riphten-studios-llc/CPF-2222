using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {

	//Handling Variables
	public float rotationSpeed = 1080;
	public float walkSpeed = 3;
	public float runSpeed = 8;
	public float accel = 5;

	//System Variables
	private Quaternion targetRotation;
	private Vector3 currentVelocityMod;

	//Required Components
	public Gun gun;
	private CharacterController controller;
	private Camera cam;

	// Start is called before the first frame update
	void Start() {
		controller = GetComponent<CharacterController>();
		cam = Camera.main;
	}

	// Update is called once per frame
	void Update() {
		ControlMouse();

		if (Input.GetButtonDown("Shoot")) {
			gun.Shoot();
		}
		else if (Input.GetButton("Shoot")) {
			gun.ShootAutomatic();
		}
	}

	void ControlMouse() {
		Vector3 mousePos = Input.mousePosition;
		mousePos = cam.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, cam.transform.position.y));
		targetRotation = Quaternion.LookRotation(mousePos - new Vector3(transform.position.x, 0, transform.position.z));
		transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);

		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

		currentVelocityMod = Vector3.MoveTowards(currentVelocityMod, input, accel * Time.deltaTime);
		Vector3 motion = input;
		motion *= (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1) ? .7f : 1;
		motion *= (Input.GetButton("Run")) ? runSpeed : walkSpeed;
		motion += Vector3.up * -8;

		controller.Move(motion * Time.deltaTime);
	}

	void ControlWASD() {
		Vector3 input = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));

		if (input != Vector3.zero) {
			targetRotation = Quaternion.LookRotation(input);
			transform.eulerAngles = Vector3.up * Mathf.MoveTowardsAngle(transform.eulerAngles.y, targetRotation.eulerAngles.y, rotationSpeed * Time.deltaTime);
		}

		currentVelocityMod = Vector3.MoveTowards(currentVelocityMod, input, accel * Time.deltaTime);
		Vector3 motion = input;
		motion *= (Mathf.Abs(input.x) == 1 && Mathf.Abs(input.z) == 1) ? .7f : 1;
		motion *= (Input.GetButton("Run")) ? runSpeed : walkSpeed;
		motion += Vector3.up * -8;

		controller.Move(motion * Time.deltaTime);
	}
}
