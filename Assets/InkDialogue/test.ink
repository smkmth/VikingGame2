VAR timeOfDay = 9

=== talk ===
testing talk {timeOfDay}
* {timeOfDay <= 12} ->Day1Afternoon
* {timeOfDay > 12} ->Evening


=== Day1Morning  ===

Good morning guy.

+ "first with the + style"
    and a trailing line
    -> plusstyle
+ [second with the square brackets style] -> squarebracketsstyle


=== Day1Afternoon ===

Good afternoon guy.

+ "first with the + style"
    and a trailing line
    -> plusstyle
+ [second with the square brackets style] -> squarebracketsstyle

=== Evening ===

Good afternoon guy.

+ "first with the + style"
    and a trailing line
    -> plusstyle
+ [second with the square brackets style] -> squarebracketsstyle




=== plusstyle ===
at this point i wish to call a function in unity. so i send a tag # thetag # secondtag

+ "choice"
    -> ending


=== squarebracketsstyle ===
this is what the square brackets look like
-> ending


=== ending
-> END