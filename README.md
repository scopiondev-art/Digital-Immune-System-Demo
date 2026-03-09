Author: Mirza (13-year-old learning programming)
# Digital Immune System Demo

Instead of blocking threats directly, the program uses multiple “sensors” that watch for abnormal behavior and increase a suspicion score when unusual activity is detected.

## Idea

The system works similarly to how biological immune systems detect infections:

Sensors → Suspicion Score → Risk Level

Different detectors monitor the application's behavior and raise the suspicion score if something unusual happens.

## Features

- Integrity monitoring (checks if important values change)
- Behavior anomaly detection
- Suspicion score system
- Status levels (Normal → Suspicious → High Risk)
- Live event logging
- Random UI noise to simulate activity

## How it works

Every second the system runs several checks:

- **Integrity Sensor** – verifies expected UI state
- **Trap Sensor** – detects changes to hidden trap variables
- **Behavior Sensor** – detects excessive UI activity
- **Timing Sensor** – detects suspiciously fast actions

Each anomaly increases the suspicion score.

## Running the project

1. Clone the repository
2. Open `Security-Test.sln` in Visual Studio
3. Run the project

## Built With

- C#
- WinForms
- Visual Studio

## Purpose

This project is an educational experiment exploring anomaly detection and defensive software design inspired by biological immune systems.
