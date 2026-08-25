# StudyFlow SA 📅

An offline-first Android timetable and schedule manager built for South African students, designed to help you stay on top of classes without needing an internet connection.

## Overview

StudyFlow SA solves a simple but common problem: students need a reliable way to manage their class schedules that works even when data or Wi-Fi isn't available. The app stores everything locally on-device, so your timetable is always accessible — no signal required.

Built as a coursework project for IFS324E (Mobile Application Development) at the University of Fort Hare, and tested end-to-end on a physical Android device.

## Features

- 🗓️ **Timetable management** — add, edit, and delete classes and subjects
- 🔔 **Class reminders** — local notifications so you never miss a class
- 📴 **Offline-first architecture** — all data stored locally via SQLite, no internet required
- 📱 **Native Android experience** — built with .NET MAUI for a smooth, responsive UI

## Tech Stack

| Layer | Technology |
|---|---|
| Framework | .NET MAUI |
| Language | C# |
| UI | XAML |
| Local Database | SQLite |
| Platform | Android (tested on physical device) |

## Screenshots

<!-- Add screenshots here, e.g.: -->
<!-- ![Home screen](screenshots/home.png) --><img width="1080" height="2400" alt="Screenshot_20260825_030146" src="https://github.com/user-attachments/assets/89e16514-c40d-47cf-b69b-a4c3f7697ff7" />

<!-- ![Add class](screenshots/add-class.png) -->
<!-- ![Notifications](screenshots/notification.png) -->

## Getting Started

### Prerequisites
- Visual Studio 2022 (with .NET MAUI workload installed)
- .NET 8 SDK or later
- Android SDK / emulator, or a physical Android device with USB debugging enabled

### Installation

```bash
git clone https://github.com/<MihlaliMandoyi-hub>/StudyFlowSA.git
cd StudyFlowSA
```

1. Open the solution in Visual Studio.
2. Restore NuGet packages.
3. Select your target device (emulator or physical Android device).
4. Build and run (F5).

## Why This Project

Built to explore offline-first mobile architecture and local data persistence patterns using .NET MAUI — a growing cross-platform framework — while solving a real, everyday problem for students managing multiple modules, lectures, and deadlines.

## Author

**Mihlali** — BCom Information Systems student, University of Fort Hare

## License

This project was developed for practice purposes as part of my portfolio build-it.
