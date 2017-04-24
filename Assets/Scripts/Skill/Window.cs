using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Window : MonoBehaviour
{

	[SerializeField] private Text mWindowTitle;
	[SerializeField] private Text mWindowText;

	public enum Skill
	{
		mUtitility,
		mDefense,
	}

	private Skill mSkill;

	private GameObject mClickedSkill;

	private OnUpgradeListener<GameObject> mOnUpgradeListener;

	//	================================================================================
	//	On Click Listeners
	//	================================================================================
	public void UpgradeClick ()
	{
		if (mOnUpgradeListener != null) {
			switch (mSkill) {
			case Skill.mDefense:
				mOnUpgradeListener.OnDefenseSkillClicked (mClickedSkill);
				break;
			case Skill.mUtitility:
				mOnUpgradeListener.OnUtilitySkillClicked (mClickedSkill);
				break;
			}
		}
		CloseClick ();
	}

	public void CloseClick ()
	{
		gameObject.SetActive (false);
	}

	//	================================================================================
	//	Setters
	//	================================================================================
	public string MWindowTitle {
		set {
			mWindowTitle.text = value;
		}
	}

	public string MWindowText {
		set {
			mWindowText.text = value;
		}
	}

	public Skill MSkill {
		set {
			mSkill = value;
		}
	}

	public GameObject MClickedSkill {
		set {
			mClickedSkill = value;
		}
	}

	public OnUpgradeListener<GameObject> MOnUpgradeListener {
		set {
			mOnUpgradeListener = value;
		}
	}
}
