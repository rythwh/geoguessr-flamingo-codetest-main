using TMPro;
using UnityEngine;

public class FloatingText : MonoBehaviour
{
	[SerializeField] private Canvas canvas;
	[SerializeField] private TMP_Text text;

	private void Start() {
		canvas.transform.LookAt(Camera.main?.transform);
		canvas.transform.Rotate(0, 180, Random.Range(-20, 20));
	}

	public void SetText(string contents) {
		text.SetText(contents);
	}
}