using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FinalScoreDisplay : MonoBehaviour {

	void Start ()
	{
		Score scoreRecord = FindObjectOfType<Score>();
		GetComponent<Text>().text = scoreRecord.score.ToString();		
	}
}
