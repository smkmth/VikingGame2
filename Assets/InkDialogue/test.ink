// the two slahes means its a comment and wont be seen by the game.

///i establish all the variables that this character needs to know. all the variables marked with _ in fron of them, like this _example are variables which change somthing in unity. Time of day, or current day are all consistant and should only be read from unity, where as characterDisposition is going to change in a conversation and needs to be updated in unity (ill do that bit)

//these variables are village variables, and are consistant for every so they are stored in a seperate file to save space
INCLUDE VillageData.ink

//these veriables are specific to each character

VAR characterName = ""
VAR _characterDisposition = -1

//the text parser starts here. You can test it in the window to the left by setting values up for the characters variables. 

//the variables setup already are tests, which make sure the values are being set in unity by checking they are not set to their default value

{ 
-timeOfDay == -1:
    error, time of day is the test number ->ERROR
-currentDay == -1:
    error, current day is the test number ->ERROR
-characterName == "":
    error, character name not set ->ERROR
-_characterDisposition == -1:
    error, character disposition is test number ->ERROR
- else :
    ->Start
} 



=== Start ==
//we can print a variable to unity with the brackets, here we just print the current day

its day {currentDay}.

{
-currentDay == 0:
->Day1
-currentDay == 1:
->Day2
}




=== Day1 ===
{
-timeOfDay < 10:
    ->Morning
-timeOfDay <=12:
    ->Afternoon
-timeOfDay >12:
    ->Evening
}


=== Day2 ===
{
-timeOfDay < 10:
    ->Morning
-timeOfDay <=12:
    ->Afternoon
-timeOfDay >12:
    ->Evening
}


=== Morning ===

good morning man.
here are some basic divergences 
*	"What?!"
 	"further options?"
	* * 	"Detective-Inspector Japp!"
	* * 	"Captain Hastings!"
	* * 	"Myself!"
* 	"Suicide!"
	"Really, Poirot? Are you quite sure?"
	* * 	"Quite sure."
	* *		"It is perfectly obvious."
-	Mrs. Christie lowered her manuscript a moment. The rest of the writing group sat, open-mouthed.

->ending

=== Afternoon ===
good afternoon man
->ending

=== Evening ===

//Here i am updating the characters disposition to me, which will be reflected in the editor. 

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