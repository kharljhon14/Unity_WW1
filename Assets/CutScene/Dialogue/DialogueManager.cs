using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogueManager : MonoBehaviour
{

    public Text nameText;
    public TextMeshProUGUI dialogueText;
    public Image fadeScreen;
    public float fadeSpeed;

    public Animator animator;

    private Queue<string> sentences;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(FadeIn());
        sentences = new Queue<string>();
    }

    public void StartDialogue (Dialogue dialogue)
    {

        animator.SetBool("IsOpen", true);

        nameText.text = dialogue.name;

        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence ()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

      string sentence = sentences.Dequeue();
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";

        //foreach (char letter in sentence.ToCharArray())
        //{
        //    dialogueText.text += letter;
        //    yield return null;
        //}

        dialogueText.text = sentence;
        yield return null;
        
    }
    void EndDialogue()
    {
        animator.SetBool("IsOpen", false);
        StartCoroutine(FadeOutContinue("Character Selection"));
    }

    protected virtual IEnumerator FadeIn()
    {
        float timeStarted = Time.time;
        float timeSinceStarted = Time.time - timeStarted;
        float percentageComplete = timeSinceStarted / fadeSpeed;
        Color currentColor = fadeScreen.color;

        while (true)
        {
            timeSinceStarted = Time.time - timeStarted;
            percentageComplete = timeSinceStarted / fadeSpeed;
            currentColor.a = Mathf.Lerp(1, 0, percentageComplete);

            fadeScreen.color = currentColor;

            if (percentageComplete >= 1)
            {
                fadeScreen.raycastTarget = false;
                break;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    protected virtual IEnumerator FadeOut(SceneReference scene)
    {
        float timeStarted = Time.time;
        float timeSinceStarted = Time.time - timeStarted;
        float percentageComplete = timeSinceStarted / fadeSpeed;
        Color currentColor = fadeScreen.color;

        while (true)
        {
            timeSinceStarted = Time.time - timeStarted;
            percentageComplete = timeSinceStarted / fadeSpeed;
            currentColor.a = Mathf.Lerp(0, 1, percentageComplete);
            fadeScreen.color = currentColor;
            fadeScreen.raycastTarget = true;
            if (percentageComplete >= 1)
                break;

            yield return new WaitForEndOfFrame();
        }

        SceneManager.LoadScene(scene);
    }

    protected virtual IEnumerator FadeOutContinue(string scene)
    {
        float timeStarted = Time.time;
        float timeSinceStarted = Time.time - timeStarted;
        float percentageComplete = timeSinceStarted / fadeSpeed;
        Color currentColor = fadeScreen.color;

        while (true)
        {
            timeSinceStarted = Time.time - timeStarted;
            percentageComplete = timeSinceStarted / fadeSpeed;
            currentColor.a = Mathf.Lerp(0, 1, percentageComplete);
            fadeScreen.color = currentColor;
            fadeScreen.raycastTarget = true;
            if (percentageComplete >= 1)
                break;

            yield return new WaitForEndOfFrame();
        }

        SceneManager.LoadScene(scene);
    }

}
