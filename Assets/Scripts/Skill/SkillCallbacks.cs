using UnityEngine;
using System.Collections;

public interface OnUpgradeListener<GameObject>
{
	void OnUtilitySkillClicked (GameObject clickedObject);

	void OnDefenseSkillClicked (GameObject clickedObject);
}
