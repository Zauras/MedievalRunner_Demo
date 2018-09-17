using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneFader : MonoBehaviour
{

	public static SceneFader instance;

	[SerializeField]
	private GameObject fadeCanvas;

	[SerializeField]
	private Animator fadeAnim;

	void Awake() {
		MakeSingleton ();
	}

	void MakeSingleton() {
		if (instance != null) {
			Destroy (gameObject);
		} else {
			instance = this;
			DontDestroyOnLoad(gameObject);
		}
	}

	public void LoadScene(string sceneName) {
		StartCoroutine (FadeInAnimation(sceneName));
	}
	IEnumerator FadeInAnimation(string sceneName) {
		fadeCanvas.SetActive (true);
		fadeAnim.Play ("FadeIn");
		yield return StartCoroutine (MyCoroutine.WaitForRealSeconds(0.3f));

		SceneManager.LoadScene (sceneName);
		fadeAnim.Play ("FadeOut");
		yield return StartCoroutine (MyCoroutine.WaitForRealSeconds (0.3f));
		fadeCanvas.SetActive (false);
	}

} // class


















































