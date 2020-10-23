using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public Spawner spawner;
    public Rotator rotator;
    public Animator animator;
    public UnityEngine.UI.Text LevelText;

    List<AttachPin> attachedPins;
    int Level = 1;

    [System.Serializable]
    public struct Levels
    {
        public int PinCount;
        public float Speed;
        public bool Reverse;
        public bool isHaveAnim;

    }

    public Levels[] levels;

    private void Start()
    {
        if (PlayerPrefs.HasKey("Level"))
        {
            Level = PlayerPrefs.GetInt("Level");
        }

        LevelText.text = Level.ToString();
        rotator.GetComponent<Animator>().SetInteger("Level", Level);

        Levels selectedLevel = levels[Level - 1];

        spawner.Reverse = selectedLevel.Reverse;
        rotator.GetComponent<Animator>().enabled = selectedLevel.isHaveAnim;
        rotator.RotateSpeed = selectedLevel.Speed;

        spawner.SpawnPin(selectedLevel.PinCount);
    }


    IEnumerator LoseAnimations()
    {
        //while (true)
        //{
        //    for(int i = 0;i < attachedPins.Count; i++)
        //    {
        //        attachedPins[i].transform.localPosition = Vector3.Lerp(attachedPins[i].transform.localPosition, new Vector3(0, -60, 0), Time.fixedDeltaTime);
        //    }

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        //}
    }


    IEnumerator WinAnimations()
    {
        while (true)
        {
            for (int i = 0; i < attachedPins.Count; i++)
            {
                attachedPins[i].transform.Translate(-transform.up * 20 * Time.fixedDeltaTime);
                attachedPins[i].line.SetPosition(1, attachedPins[i].line.GetPosition(1) + Vector3.up * 55 * Time.fixedDeltaTime);
            }

            yield return new WaitForSeconds(Time.fixedDeltaTime);
        }
    }

    private void LevelEnd()
    {
        rotator.enabled = false;
        attachedPins = spawner.attachedPins;
        spawner.enabled = false;
    }

    public void EndGame()
    {
        LevelEnd();

        StartCoroutine(LoseAnimations());

        animator.SetTrigger("EndTrigger");
    }

    public void NextLevel()
    {
        LevelEnd();

        Level++;

        if (Level > levels.Length)
            Level = 1;

        PlayerPrefs.SetInt("Level", Level);
        StartCoroutine(WinAnimations());

        animator.SetTrigger("WinTrigger");
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
