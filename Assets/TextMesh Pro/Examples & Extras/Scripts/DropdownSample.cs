using TMPro;
using UnityEngine;

namespace TextMesh_Pro.Examples___Extras.Scripts
{
	public class DropdownSample: MonoBehaviour
	{
		[SerializeField] TextMeshProUGUI text;

		[SerializeField] TMP_Dropdown dropdownWithoutPlaceholder;

		[SerializeField] TMP_Dropdown dropdownWithPlaceholder;

		public void OnButtonClick()
		{
			text.text = dropdownWithPlaceholder.value > -1 ? "Selected values:\n" + dropdownWithoutPlaceholder.value + " - " + dropdownWithPlaceholder.value : "Error: Please make a selection";
		}
	}
}
