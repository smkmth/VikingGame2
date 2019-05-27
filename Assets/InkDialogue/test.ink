// the two slahes means its a comment and wont be seen by the game.

///i establish all the variables that this character needs to know.
//all the variables marked with _ in fron of them, like this _example
//are variables which change somthing in unity. Time of day, or current
//day are all consistant and should only be read from unity, where as 
//characterDisposition is going to change in a conversation and needs 
//to be updated in unity (ill do that bit)

//these variables are village variables, and are consistant for every
//character
VAR timeOfDay = -1
VAR currentDay = -1

//these veriables are specific to each character
VAR characterName = ""
VAR _characterDisposition = -1

//all files point to talk first; and then go to whatever they need based on 
//the variable passed. You can test it in the window to the left by setting 
//values up for the characters variables

//these ones here are tests, which make sure the values are being set in unity
//by checking they are not set to their default value
* {timeOfDay == -1} error, time of day is the test number ->ERROR
* {currentDay == -1} error, current day is the test number ->ERROR 
* {characterName == ""} error, character name not set ->ERROR
* {_characterDisposition == -1} error, character disposition is test number ->ERROR
//the writing just goes here as plain text. here we are checking if its morning 
//or evening. 

//we can print a variable to unity with the brackets, here we just print the time of day
{currentDay}
//here we branch. this is lazy form i think, a better way to do this is 
*{currentDay == 0} ->Day1 
*{currentDay >= 1} ->Day2


=== Day1 ===
*{timeOfDay < 10} ->Morning 
*{timeOfDay <= 12} ->Afternoon 
*{timeOfDay > 12} ->Evening 

=== Day2 ===

*{timeOfDay < 10} ->Morning 
*{timeOfDay <= 12} ->Afternoon 
*{timeOfDay > 12} ->Evening 

=== Morning ===

good morning man
->ending

=== Afternoon ===
good afternoon man
->ending

=== Evening ===

//Here i am updating the characters disposition to me, which will be 
//reflected in the editor. 
~ _characterDisposition = 3
good evening man
->ending

=== ending
//this is the legit ending, point here to exit out of dialogue.
-> END

=== ERROR
ERROR ERROR ERROR

time of day = {timeOfDay}
current day = {currentDay}
characterName = {characterName}
characterDisposition = {_characterDisposition}
-> END