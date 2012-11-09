Anime Quiz
==============
v0.7MDI-beta
--------------
- Initial MDI implementation

v0.6-beta
--------------
- Changed pointLabel text color to black
- Changed entry text to Entry)

v0.5-beta
--------------
- Fixed the behavior of default folder Settings: 1 folder for game save, 1 load folder for music, 1 load folder for screenshot
- Changed the behavior of the Clear button to free last saved game from Settings in order to effectively create a new game
- Fixed seeking in songs
- Added a Setting to automatically pause the song after a certain interval (alpha)

v0.4-beta
--------------
- Fixed answeredCheckBox positioning for Screenshot type Question
- Fixed a bug where saving after deleting a Question would return an error by adding a feature that automatically ignores blanks
- Increased the number of default folders saved in Settings to 4: one load, one Question-type, one Music-type, one Screenshot-type
- Switched to using WindowsMediaPlayer libraries (WMPLib) from MediaControl which didn�t accept utf-8 strings
- Added a volume control Setting
Removed:
- Save button in main window > autosave
- Refresh button in main window > autorefresh

v0.3-beta
--------------
- Changed the app name to Anime Quiz from Anime Jeopardy
- Temporarily disabled Japanese translation
- Added Save button in main window
- Added Refresh button in main window
- Implemented game loading logic in the game editor
- Embedded image resource files for the music player
- Added add/remove Question buttons in game editor

v0.2-beta
--------------
- Fixed MediaControl dll loading error by merging it with the executable

v0.1-beta
--------------
- Initial release
