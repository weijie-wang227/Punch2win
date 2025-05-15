using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIData : MonoBehaviour
{
	private static UIData _Instance;
	public static UIData Instance
	{
		get
		{
			if (!_Instance)
			{
				_Instance = new GameObject().AddComponent<UIData>();
				// name it for easy recognition
				_Instance.name = _Instance.GetType().ToString();
				// mark root as DontDestroyOnLoad();
				DontDestroyOnLoad(_Instance.gameObject);
			}
			return _Instance;
		}
	}
	[SerializeField] private CharacterMotor opponent;
	[SerializeField] private CharacterMotor player;
	[SerializeField] private Slider oppHealthBar;
	[SerializeField] private Slider oppStaminaBar;
	[SerializeField] private Slider playerHealthBar;
	[SerializeField] private Slider playerStaminaBar;
	[SerializeField] private Image blackScreen;

	// Use this for initialization
	void Awake()
	{
		oppHealthBar.maxValue = PlayerManager.maxHealth[1];
		oppStaminaBar.maxValue = opponent.stamina;
		playerHealthBar.maxValue = PlayerManager.maxHealth[0];
		playerStaminaBar.maxValue = player.stamina;
		blackScreen = transform.Find("BlackScreen").GetComponent<Image>();
	}

	// Update is called once per frame
	void Update()
	{
		if (opponent != null)
		{
			oppHealthBar.value = opponent.health;
			oppStaminaBar.value = opponent.stamina;
		}
		if (player != null)
		{
			playerHealthBar.value = player.health;
			playerStaminaBar.value = player.stamina;
		}
	}

	void OnEnable()
	{
		EventManager.RoundEnd += EventManagerRoundEnd;
	}
	void OnDisable()
	{
		EventManager.RoundEnd -= EventManagerRoundEnd;
	}

	private void EventManagerRoundEnd()
	{
		StartCoroutine(FadeToBlack(1.0f));
	}

	private IEnumerator FadeToBlack(float duration)
	{
		yield return new WaitForSeconds(1.0f);
		float timer = 0f;
		Color newColor = blackScreen.color;
		while (timer < duration)
		{
			newColor.a = Mathf.SmoothStep(0, 1, timer / duration);
			blackScreen.color = newColor;
			timer += Time.deltaTime;
			yield return null;
		}
	}


}
