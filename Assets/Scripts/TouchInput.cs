using UnityEngine;

public class TouchInput : MonoBehaviour
{
	[SerializeField] TouchDetector brakeButton;
	[SerializeField] TouchDetector gasButton;

	[SerializeField] float brakeSmoothTime = 0.1f;
	[SerializeField] float gasSmoothTime = 0.1f;

	static float rawBrakeInput;
	static float rawGasInput;

	static float dampedBrakeInput;
	static float dampedGasInput;

	[Header("Alternative Input (fallback)")]
	[SerializeField] bool useKeyboardFallback = true;
	[SerializeField] bool useMouseFallback = true;
	
	[Header("Debug")]
	[SerializeField] bool showDebug = false;

	void Update()
	{
		// Detectar entrada de freno
		bool brakePressed = GetBrakeInput();
		if (brakePressed)
		{
			rawBrakeInput = 1f;
			dampedBrakeInput = ValueDamper.Damp(dampedBrakeInput, rawBrakeInput, brakeSmoothTime);
		}
		else
		{
			rawBrakeInput = 0f;
			dampedBrakeInput = ValueDamper.Damp(dampedBrakeInput, rawBrakeInput, brakeSmoothTime);
		}

		// Detectar entrada de aceleración
		bool gasPressed = GetGasInput();
		if (gasPressed)
		{
			rawGasInput = 1f;
			dampedGasInput = ValueDamper.Damp(dampedGasInput, rawGasInput, gasSmoothTime);
		}
		else
		{
			rawGasInput = 0f;
			dampedGasInput = ValueDamper.Damp(dampedGasInput, 0f, gasSmoothTime);
		}
		
		if (showDebug && (brakePressed || gasPressed))
		{
			Debug.Log($"Brake: {brakePressed} | Gas: {gasPressed}");
		}
	}
	
	bool GetBrakeInput()
	{
		// Entrada del botón táctil
		if (brakeButton != null && brakeButton.IsTouched())
			return true;
			
		// Fallback de teclado
		if (useKeyboardFallback && (Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S)))
			return true;
			
		return false;
	}
	
	bool GetGasInput()
	{
		// Entrada del botón táctil
		if (gasButton != null && gasButton.IsTouched())
			return true;
			
		// Fallback de teclado
		if (useKeyboardFallback && (Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W)))
			return true;
			
		return false;
	}

	public static float GetDampedBrakeInput()
	{
		return dampedBrakeInput;
	}

	public static float GetDampedGasInput()
	{
		return dampedGasInput;
	}

	public static float GetRawBrakeInput()
	{
		return rawBrakeInput;
	}

	public static float GetRawGasInput()
	{
		return rawGasInput;
	}
}
