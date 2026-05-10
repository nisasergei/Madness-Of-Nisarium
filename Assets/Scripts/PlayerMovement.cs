using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Настройки движения")]
    public float speed = 5f;
    public float sensitivity = 2f;
    public float gravity = -19.62f; // Сделаем чуть сильнее для динамики
    public float jumpHeight = 2f;

    [Header("Приседание")]
    public float crouchHeight = 1f;
    public float standHeight = 2f;
    public float crouchSpeed = 2.5f;

    CharacterController controller;
    Vector3 velocity;
    bool isGrounded;
    float xRotation = 0f;

    void Start() {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update() {
        // Проверка: стоим ли мы на земле
        isGrounded = controller.isGrounded;
        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f; // Прижимаем к земле
        }

        // --- ПОВОРОТ ГОЛОВЫ ---
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        Camera.main.transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);

        // --- ПРИСЕДАНИЕ (на Left Control) ---
        if (Input.GetKeyDown(KeyCode.LeftControl)) {
            controller.height = crouchHeight;
            speed = crouchSpeed;
        }
        if (Input.GetKeyUp(KeyCode.LeftControl)) {
            controller.height = standHeight;
            speed = 5f;
        }

        // --- ДВИЖЕНИЕ ---
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");
        Vector3 move = transform.right * x + transform.forward * z;
        controller.Move(move * speed * Time.deltaTime);

        // --- ПРЫЖОК ---
        if (Input.GetButtonDown("Jump") && isGrounded) {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        // --- ФИЗИКА ГРАВИТАЦИИ ---
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }
}