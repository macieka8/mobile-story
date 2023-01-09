using UnityEngine;
using System;
using TMPro;
using System.Collections;

namespace Game
{
    public class TextDisplayer : MonoBehaviour
    {
		[SerializeField] float _timeBetweenLetterDisplay;
		[SerializeField] float _fullTextWaitTime;
		[SerializeField] TextMeshProUGUI _text;
		[SerializeField] SoundtrackPlayer _textDisplaySoundPlayer;

		string _textToDisplay;
		int _displayedLetters;
		int _lettersToDisplay;
		bool _isPaused;

		Timer _letterTimer;
		Timer _fullTextWaitTimer;

		Coroutine _letterCoroutine;
		Coroutine _textCoroutine;

		public event Action OnTextDisplayed;

		public bool IsFinished { get; private set; }

        void Start()
        {
			_letterTimer = new Timer(_timeBetweenLetterDisplay);
			_fullTextWaitTimer = new Timer(_fullTextWaitTime);

			_letterTimer.OnElapsed += HandleNextLetter;
			_fullTextWaitTimer.OnElapsed += HandleTextDisplayed;
		}

		void HandleTextDisplayed()
        {
			_fullTextWaitTimer.Restart();

			IsFinished = true;
			OnTextDisplayed?.Invoke();
			StopCoroutine(_textCoroutine);
		}

		void HandleNextLetter()
		{
			_letterTimer.Restart();
			if (_isPaused) return;

			// Display all whitespaces
			while (_displayedLetters != _lettersToDisplay
				&& char.IsWhiteSpace(_textToDisplay[_displayedLetters++]))
			{ }

			_text.text = _textToDisplay.Substring(0, _displayedLetters);
			_textDisplaySoundPlayer.PlayRandom();

			if (_displayedLetters == _lettersToDisplay)
			{
				_textCoroutine = StartCoroutine(TextCoroutine());
				StopCoroutine(_letterCoroutine);
			}
		}

		IEnumerator LetterCoroutine()
        {
			_letterTimer.Restart();
			while (true)
            {
				if (!_isPaused)
					_letterTimer.Update(Time.deltaTime);
				yield return null;
            }
        }

		IEnumerator TextCoroutine()
        {
			_fullTextWaitTimer.Restart();
			while (true)
			{
				if (!_isPaused)
					_fullTextWaitTimer.Update(Time.deltaTime);
				yield return null;
			}
		}

		public void DisplayText(string text)
        {
			_textToDisplay = text;
			_lettersToDisplay = _textToDisplay.Length;
			_displayedLetters = 0;
			_text.text = "";
			IsFinished = false;

			if (_letterCoroutine != null) StopCoroutine(_letterCoroutine);
			if (_textCoroutine != null) StopCoroutine(_textCoroutine);
			_letterCoroutine = StartCoroutine(LetterCoroutine());
		}

        public void SetPause(bool pause)
        {
			_isPaused = pause;
			_text.text = "";
		}
	}
}
