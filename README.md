anime-quiz
==========

## MSAC Anime Quiz Application
Provides an interface for creating quiz game questions and teams, as well as a Game Master and Player windows for administrating the quiz.
## Question Set Creation/Editor
Open an existing Question Set for editing, or create a new Question Set. Question Sets can be either text questions, music, or screenshots. Either way, the question "content" is stored as a string, so in the case of music and screenshots, the file path is stored. This makes more sense for our use...
## Team Editor
Add/Remove/Edit teams and their team members.
### Game Master Window
Load an existing Question Set into the game board. Doing so would load controls allowing to give points or penalty to teams. When a question is currently loaded, this window will also show the answer as well as the weight of the question in points. Buttons for controlling answer display will also be available. Furthermore, the answering order will also be listed here and updated as teams buzz in.
### Player Window
When the GM loads a question set, all the _unanswered_ questions contained within will be displayed as buttons with the point value. The team in control will chose a question, which will then be displayed in the player window. The answer display, as well as whatever animation, is controlled by the GM.
### Leaderboards
The GM Window will show a leaderboard with individual player scores, while the Player Window will only show a leaderboard of teams.
### Settings
Available settings will be
+ Default music folder: the preferred folder where music files for the quiz can be found
+ Default screenshot folder: the preferred folder where screenshots can be found
+ Autoplay music (yes/no)
+ Question duration: set the duration for the question timers
# TODO
+ Add serial for the bluetooth
+ Test music and screenshot question display
+ Add a control for animating text questions
+ Add and implement settings