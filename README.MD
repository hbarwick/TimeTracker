# TimeTracker

A CRUD console application to track activity time.
Personal use case for this application is to track number of hours spent coding/learning.

Written in C# using Visual Studio 2022, making use of SQLite database.


# Features

- [x] Allows manually entering Session Start times & end times, session duration will then be automatically calculated
- [x] Has "Clock On" feature which will start a session with the current date/time and leave End time as null
- [x] "Clock off" will then complete the session by filling in the end time and duration
- [x] User can update the start/end dates of sessions, selected by session ID
- [x] User can delete Sessions, selected by Session ID
- [x] Number of sessions displayed in the update/delete menu is configurable in app.config
- [x] Has reports section to view all sessions. Or sums of duration All Time / Last 7 days / This month / Last month


