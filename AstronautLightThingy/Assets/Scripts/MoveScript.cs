using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour
{
    public static MoveScript instance;

    private int horizontalInput;
    private int verticalInput;

    public static Rigidbody2D rb;

    public float thrustForce = 50;

    public float maxFuel;
    public float fuelUsePerSecond;
    private float currentFuel;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            rb = GetComponent<Rigidbody2D>();
            currentFuel = maxFuel;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    private void Update()
    {
        horizontalInput = (int)Input.GetAxisRaw("Horizontal");
        verticalInput = (int)Input.GetAxisRaw("Vertical");

    }

    private void FixedUpdate()
    {
        if ((horizontalInput == 0 && verticalInput == 0) || currentFuel <= 0)
        {
            Audio.instance.stopJetpack();
            return;
        }
        Audio.instance.playJetpack();
        Vector2 thrustVector = new Vector2(horizontalInput, verticalInput).normalized * thrustForce;
        currentFuel -= fuelUsePerSecond * Time.fixedDeltaTime;
        FuelBar.instance.SetValue(currentFuel / maxFuel);
        rb.AddForce(thrustVector);
    }

    public Vector2 GetInput()
    {
        return new Vector2(horizontalInput, verticalInput);
    }

    public void ResetPlayer()
    {
        instance = null;
        Destroy(gameObject);
    }

    public void AddFuel(float value)
    {
        currentFuel += value;
        if (currentFuel > maxFuel)
            currentFuel = maxFuel;
    }
}
