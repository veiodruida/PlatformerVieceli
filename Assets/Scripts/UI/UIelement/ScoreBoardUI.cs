using System;
using UnityEngine;
using TMPro;

/// <summary>
/// Handles updating the Leaderboard display with Top 10 scores and time.
/// </summary>
public class ScoreBoardUI : UIelement
{
    [Header("References")]
    [Tooltip("The parent container object holding the 10 rows (e.g. 'Registros')")]
    public Transform rowsContainer;
    [Tooltip("The text rows. If empty, it will auto-populate from rowsContainer's children.")]
    public TMP_Text[] scoreRows;

    /// <summary>
    /// Description:
    /// Standard Unity function called when the script starts
    /// </summary>
    private void Start()
    {
        if (scoreRows == null || scoreRows.Length == 0)
        {
            if (rowsContainer != null)
            {
                scoreRows = rowsContainer.GetComponentsInChildren<TMP_Text>();
            }
            else
            {
                // Fallback to siblings/self if no container is specified
                scoreRows = GetComponentsInChildren<TMP_Text>();
            }
        }
    }

    /// <summary>
    /// Updates the UI when the script (and the panel containing it) is activated.
    /// </summary>
    private void OnEnable()
    {
        UpdateScoreBoard();
    }

    /// <summary>
    /// Overrides the base UI Update function from UIelement to also trigger our logic
    /// when GameManager dispatches generic UpdateUIElements().
    /// </summary>
    public override void UpdateUI()
    {
        base.UpdateUI();
        UpdateScoreBoard();
    }

    /// <summary>
    /// Reads the JSON board from PlayerPrefs and populates the text lines
    /// </summary>
    public void UpdateScoreBoard()
    {
        if (scoreRows == null || scoreRows.Length == 0) return;

        string currentJson = PlayerPrefs.GetString("HighscoreBoard", "");
        ScoreBoardData data = null;

        if (!string.IsNullOrEmpty(currentJson))
        {
            data = JsonUtility.FromJson<ScoreBoardData>(currentJson);
        }

        for (int i = 0; i < scoreRows.Length; i++)
        {
            if (scoreRows[i] == null) continue;

            // If we have an entry for this row
            if (data != null && data.entries != null && i < data.entries.Count)
            {
                ScoreEntry entry = data.entries[i];

                // Convert seconds to MM:SS format
                TimeSpan t = TimeSpan.FromSeconds(entry.time);
                string timeFormatted = string.Format("{0:D2}:{1:D2}", t.Minutes, t.Seconds);

                scoreRows[i].text = $"{i + 1}. Score: {entry.score} | Time: {timeFormatted}";
            }
            else
            {
                // Empty row
                scoreRows[i].text = $"{i + 1}. ---";
            }
        }
    }

    /// <summary>
    /// Clears the entire highscore board data
    /// </summary>
    public void ClearLeaderboard()
    {
        PlayerPrefs.DeleteKey("HighscoreBoard");
        PlayerPrefs.Save();
        UpdateScoreBoard();
    }
}
