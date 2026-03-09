using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace Security_Test
{
    public partial class Form1 : Form
    {
        // =========================
        // IMMUNE SYSTEM STATE
        // =========================
        private int suspicionScore = 0;
        private string currentStatus = "Normal";

        // "Expected body state"
        private string expectedTitle = "TRY TO INJECT ME";
        private string expectedMainLabel = "TRY TO INJECT ME";
        private int expectedControlCount = 0;

        // Hidden trap values
        private bool trapFlag = false;
        private int trapNumber = 1337;

        // Behavior tracking
        private readonly List<DateTime> uiChangeTimes = new List<DateTime>();
        private readonly List<DateTime> buttonClickTimes = new List<DateTime>();

        private readonly Random rng = new Random();

        // =========================
        // UI CONTROLS
        // =========================
        private Label lblMain;
        private Label lblStatus;
        private Label lblScore;
        private Label lblLastScan;

        private Button btnRandomNoise;
        private Button btnToggleTrap;
        private Button btnResetScore;
        private Button btnSimulateAnomaly;

        private CheckBox cbDecoy1;
        private CheckBox cbDecoy2;
        private CheckBox cbDecoy3;

        private ListBox lstLog;

        private Timer timerScan;
        private Timer timerNoise;

        public Form1()
        {
            BuildInterface();
            CaptureInitialSnapshot();
            StartSystems();
        }

        // =========================
        // UI BUILD
        // =========================
        private void BuildInterface()
        {
            this.Text = expectedTitle;
            this.StartPosition = FormStartPosition.CenterScreen;
            this.ClientSize = new Size(1000, 650);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.WhiteSmoke;

            lblMain = new Label
            {
                Text = expectedMainLabel,
                Font = new Font("Segoe UI", 20, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(340, 40)
            };

            lblStatus = new Label
            {
                Text = "Status: Normal",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(30, 30)
            };

            lblScore = new Label
            {
                Text = "Suspicion Score: 0",
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(30, 65)
            };

            lblLastScan = new Label
            {
                Text = "Last Scan: not started yet",
                Font = new Font("Segoe UI", 10, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(30, 100)
            };

            cbDecoy1 = new CheckBox
            {
                Text = "Decoy A",
                AutoSize = true,
                Location = new Point(80, 180)
            };

            cbDecoy2 = new CheckBox
            {
                Text = "Decoy B",
                AutoSize = true,
                Location = new Point(760, 280)
            };

            cbDecoy3 = new CheckBox
            {
                Text = "Decoy C",
                AutoSize = true,
                Location = new Point(140, 470)
            };

            btnRandomNoise = new Button
            {
                Text = "Random Noise",
                Size = new Size(140, 40),
                Location = new Point(380, 280)
            };
            btnRandomNoise.Click += BtnRandomNoise_Click;

            btnToggleTrap = new Button
            {
                Text = "Toggle Trap",
                Size = new Size(140, 40),
                Location = new Point(540, 280)
            };
            btnToggleTrap.Click += BtnToggleTrap_Click;

            btnResetScore = new Button
            {
                Text = "Reset Score",
                Size = new Size(140, 40),
                Location = new Point(700, 280)
            };
            btnResetScore.Click += BtnResetScore_Click;

            btnSimulateAnomaly = new Button
            {
                Text = "Simulate Anomaly",
                Size = new Size(160, 40),
                Location = new Point(200, 280)
            };
            btnSimulateAnomaly.Click += BtnSimulateAnomaly_Click;

            lstLog = new ListBox
            {
                Size = new Size(920, 220),
                Location = new Point(30, 380),
                Font = new Font("Consolas", 10, FontStyle.Regular)
            };

            timerScan = new Timer();
            timerScan.Interval = 1000;
            timerScan.Tick += TimerScan_Tick;

            timerNoise = new Timer();
            timerNoise.Interval = 3500;
            timerNoise.Tick += TimerNoise_Tick;

            this.Controls.Add(lblMain);
            this.Controls.Add(lblStatus);
            this.Controls.Add(lblScore);
            this.Controls.Add(lblLastScan);

            this.Controls.Add(cbDecoy1);
            this.Controls.Add(cbDecoy2);
            this.Controls.Add(cbDecoy3);

            this.Controls.Add(btnRandomNoise);
            this.Controls.Add(btnToggleTrap);
            this.Controls.Add(btnResetScore);
            this.Controls.Add(btnSimulateAnomaly);

            this.Controls.Add(lstLog);
        }

        // =========================
        // STARTUP
        // =========================
        private void CaptureInitialSnapshot()
        {
            expectedControlCount = this.Controls.Count;
            Log("Initial immune snapshot captured.");
            Log($"Expected control count: {expectedControlCount}");
            Log($"Expected form title: {expectedTitle}");
            Log($"Expected main label: {expectedMainLabel}");
        }

        private void StartSystems()
        {
            timerScan.Start();
            timerNoise.Start();
            Log("Immune monitoring started.");
        }

        // =========================
        // BUTTON EVENTS
        // =========================
        private void BtnRandomNoise_Click(object sender, EventArgs e)
        {
            RecordButtonClick();
            PerformHarmlessNoise();
            Log("Manual harmless noise generated.");
        }

        private void BtnToggleTrap_Click(object sender, EventArgs e)
        {
            RecordButtonClick();

            // This simulates a suspicious hidden-state change.
            trapFlag = !trapFlag;
            Log($"Trap flag changed manually to: {trapFlag}");
        }

        private void BtnResetScore_Click(object sender, EventArgs e)
        {
            RecordButtonClick();

            suspicionScore = 0;
            currentStatus = "Normal";

            // Reset hidden values too
            trapFlag = false;
            trapNumber = 1337;

            // Restore expected visible values
            this.Text = expectedTitle;
            lblMain.Text = expectedMainLabel;

            UpdateStatusUi();
            Log("Suspicion score and monitored state reset.");
        }

        private void BtnSimulateAnomaly_Click(object sender, EventArgs e)
        {
            RecordButtonClick();

            // Simulate an unexpected change
            lblMain.Text = "MODIFIED ?";
            trapNumber = 9999;
            RegisterUiChange();

            Log("Simulated anomaly created: label text changed + trap number changed.");
        }

        // =========================
        // TIMERS
        // =========================
        private void TimerScan_Tick(object sender, EventArgs e)
        {
            RunImmuneScan();
        }

        private void TimerNoise_Tick(object sender, EventArgs e)
        {
            // Small background randomness to make the app less static
            PerformHarmlessNoise();
        }

        // =========================
        // IMMUNE SYSTEM CORE
        // =========================
        private void RunImmuneScan()
        {
            int pointsAddedThisScan = 0;

            pointsAddedThisScan += IntegrityAntigenSensor();
            pointsAddedThisScan += HiddenTrapAntigenSensor();
            pointsAddedThisScan += BehaviorAntigenSensor();
            pointsAddedThisScan += TimingAntigenSensor();

            UpdateStatusUi();
            lblLastScan.Text = $"Last Scan: {DateTime.Now:HH:mm:ss} | +{pointsAddedThisScan} points";

            if (pointsAddedThisScan == 0)
            {
                Log("Scan complete: no new anomalies.");
            }
            else
            {
                Log($"Scan complete: {pointsAddedThisScan} suspicion points added.");
            }
        }

        // 1) Integrity watcher
        private int IntegrityAntigenSensor()
        {
            int added = 0;

            if (this.Text != expectedTitle)
            {
                AddSuspicion(15, "Form title mismatch detected.");
                added += 15;
            }

            if (lblMain.Text != expectedMainLabel)
            {
                AddSuspicion(20, "Main label text mismatch detected.");
                added += 20;
            }

            if (this.Controls.Count != expectedControlCount)
            {
                AddSuspicion(25, "Control count changed unexpectedly.");
                added += 25;
            }

            return added;
        }

        // 2) Hidden trap watcher
        private int HiddenTrapAntigenSensor()
        {
            int added = 0;

            if (trapFlag)
            {
                AddSuspicion(30, "Trap flag activated.");
                added += 30;
            }

            if (trapNumber != 1337)
            {
                AddSuspicion(30, $"Trap number changed unexpectedly: {trapNumber}");
                added += 30;
            }

            return added;
        }

        // 3) UI behavior watcher
        private int BehaviorAntigenSensor()
        {
            int added = 0;

            RemoveOldEntries(uiChangeTimes, 5);

            if (uiChangeTimes.Count >= 8)
            {
                AddSuspicion(20, $"Too many UI changes in 5 seconds: {uiChangeTimes.Count}");
                added += 20;
            }

            return added;
        }

        // 4) Fast timing watcher
        private int TimingAntigenSensor()
        {
            int added = 0;

            RemoveOldEntries(buttonClickTimes, 2);

            if (buttonClickTimes.Count >= 6)
            {
                AddSuspicion(15, $"Suspiciously fast repeated clicks detected: {buttonClickTimes.Count} in 2 sec");
                added += 15;
            }

            return added;
        }

        // =========================
        // HELPERS
        // =========================
        private void AddSuspicion(int amount, string reason)
        {
            suspicionScore += amount;
            Log($"[ANTIGEN] +{amount} -> {reason}");
        }

        private void UpdateStatusUi()
        {
            lblScore.Text = $"Suspicion Score: {suspicionScore}";

            if (suspicionScore < 20)
            {
                currentStatus = "Normal";
                lblStatus.Text = "Status: Normal";
                lblStatus.ForeColor = Color.DarkGreen;
            }
            else if (suspicionScore < 50)
            {
                currentStatus = "Suspicious";
                lblStatus.Text = "Status: Suspicious";
                lblStatus.ForeColor = Color.DarkOrange;
            }
            else
            {
                currentStatus = "High Risk";
                lblStatus.Text = "Status: HIGH RISK";
                lblStatus.ForeColor = Color.DarkRed;
            }
        }

        private void Log(string message)
        {
            string line = $"[{DateTime.Now:HH:mm:ss}] {message}";
            lstLog.Items.Insert(0, line);

            // Keep list manageable
            while (lstLog.Items.Count > 200)
            {
                lstLog.Items.RemoveAt(lstLog.Items.Count - 1);
            }
        }

        private void RegisterUiChange()
        {
            uiChangeTimes.Add(DateTime.Now);
        }

        private void RecordButtonClick()
        {
            buttonClickTimes.Add(DateTime.Now);
        }

        private void RemoveOldEntries(List<DateTime> list, int seconds)
        {
            DateTime limit = DateTime.Now.AddSeconds(-seconds);
            list.RemoveAll(t => t < limit);
        }

        private void PerformHarmlessNoise()
        {
            // Random small movement of decoys
            MoveControlSlightly(cbDecoy1);
            MoveControlSlightly(cbDecoy2);
            MoveControlSlightly(cbDecoy3);

            // Randomly toggle one decoy checkbox sometimes
            int roll = rng.Next(0, 3);
            if (roll == 0) cbDecoy1.Checked = !cbDecoy1.Checked;
            if (roll == 1) cbDecoy2.Checked = !cbDecoy2.Checked;
            if (roll == 2) cbDecoy3.Checked = !cbDecoy3.Checked;

            RegisterUiChange();
        }

        private void MoveControlSlightly(Control control)
        {
            int dx = rng.Next(-5, 6);
            int dy = rng.Next(-5, 6);

            int newX = Math.Max(10, Math.Min(this.ClientSize.Width - control.Width - 10, control.Left + dx));
            int newY = Math.Max(140, Math.Min(350, control.Top + dy));

            control.Location = new Point(newX, newY);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}