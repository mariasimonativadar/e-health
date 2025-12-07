using System.Collections;
using UnityEngine;
using TMPro;

namespace SojaExiles
{
    public class Drawer_Pull_X : MonoBehaviour
    {
        [Header("Drawer Animation")]
        public Animator pull_01;
        public Transform Player;
        public float interactDistance = 8f;

        [Header("Locker UI")]
        public string correctCode = "4729";
        public GameObject keypadPanel;          // KeypadPanel
        public TMP_InputField codeInput;        // CodeInput (TMP_InputField)
        public TMP_Text errorText;              // ErrorText (TMP_Text)
        public GameObject phoneDirectory;       // book_2 (optional)

        [Header("Audio")]
        public AudioSource audioSource;
        public AudioClip unlockSound;
        public AudioClip wrongSound;
        public AudioClip openSound;
        public AudioClip closeSound;

        bool isUnlocked = false;
        bool isOpen = false;
        bool panelOpen = false;

        void Start()
        {
            if (keypadPanel != null) keypadPanel.SetActive(false);
            if (errorText != null) errorText.text = "";
            if (phoneDirectory != null) phoneDirectory.SetActive(false);
        }

        // --------------------------------------------------
        // CLICK DRAWER
        // --------------------------------------------------
        void OnMouseDown()
        {
            if (Player == null) return;

            float dist = Vector3.Distance(Player.position, transform.position);
            if (dist > interactDistance) return;

            if (!isUnlocked)
            {
                OpenPanel();
            }
            else
            {
                ToggleDrawer();
            }
        }

        // --------------------------------------------------
        // PANEL CONTROL
        // --------------------------------------------------
        void OpenPanel()
        {
            if (keypadPanel == null) return;

            keypadPanel.SetActive(true);
            panelOpen = true;

            if (codeInput != null)
            {
                codeInput.text = "";
                codeInput.ActivateInputField();
                codeInput.Select();
            }

            if (errorText != null)
                errorText.text = "";
        }

        void ClosePanel()
        {
            if (keypadPanel == null) return;

            keypadPanel.SetActive(false);
            panelOpen = false;
        }

        // --------------------------------------------------
        // KEYBOARD: only cares about Enter / Escape
        // TMP_InputField handles typing for us
        // --------------------------------------------------
        void Update()
        {
            if (!panelOpen) return;

            // Submit with Enter
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                SubmitCode();
            }

            // Close with Esc
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ClosePanel();
            }
        }

        void SubmitCode()
        {
            if (codeInput == null) return;

            string entered = codeInput.text.Trim();

            if (entered == correctCode)
            {
                // Correct
                isUnlocked = true;

                if (audioSource != null && unlockSound != null)
                    audioSource.PlayOneShot(unlockSound);

                if (errorText != null)
                    errorText.text = "";

                if (phoneDirectory != null)
                    phoneDirectory.SetActive(true);

                ClosePanel();
                ToggleDrawer(); // auto open once
            }
            else
            {
                // Wrong
                if (audioSource != null && wrongSound != null)
                    audioSource.PlayOneShot(wrongSound);

                if (errorText != null)
                {
                    errorText.text = "Incorrect code. Try again.";
                    errorText.color = Color.red;
                }

                codeInput.text = "";
                codeInput.ActivateInputField();
                codeInput.Select();
            }
        }

        // --------------------------------------------------
        // DRAWER ANIMATION
        // --------------------------------------------------
        void ToggleDrawer()
        {
            if (!isOpen)
                StartCoroutine(OpenDrawer());
            else
                StartCoroutine(CloseDrawer());
        }

        IEnumerator OpenDrawer()
        {
            if (pull_01 != null)
                pull_01.Play("openpull_01");

            if (audioSource != null && openSound != null)
                audioSource.PlayOneShot(openSound);

            isOpen = true;
            yield return new WaitForSeconds(0.5f);
        }

        IEnumerator CloseDrawer()
        {
            if (pull_01 != null)
                pull_01.Play("closepush_01");

            if (audioSource != null && closeSound != null)
                audioSource.PlayOneShot(closeSound);

            isOpen = false;
            yield return new WaitForSeconds(0.5f);
        }
    }
}
