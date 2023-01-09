VAR umoHelpAcquired = false
LIST helpTest = (None), QuestStarted, TalkedWithUma, QuestFinished

=== Umo_Start ===
-> Umo_Options

=== Umo_Options ===
Oom?
+ [Talk] You like magic?
* [Help] -> Help_Umo
* {state_equals(helpTest, TalkedWithUma)} [Report back] -> Umo_Report_Back
+ {!minotaurKilled && !umoHelpAcquired && state_equals(wizardQuestState, movingStonesStarted)} [Help Wizard] -> Umo_Wizard_Help
* {state_reached(helpTest, QuestFinished)} [Learn Dash] -> Learn_Dash
- -> Umo_Options

=== Help_Umo ===
Oom!
Me Umo. You help Umo?
* [👍]
    ~ helpTest = QuestStarted
    You talk Uma. She help Umo. #Quest.Start.Help_Umo
* [👎]
    OK. Umo fine.
- -> Umo_Options

=== Talk_Umo ===
Talking...
-> Umo_Options

=== Umo_Report_Back ===
Umo Thanks You
You take it.
'Proceeds to Fistbump'
.
..
~ helpTest = QuestFinished
... #Quest.Update.Help_Umo.2
-> Umo_Options

=== Umo_Wizard_Help ===
Ooom??
+ {state_reached(helpTest, QuestFinished)} [Help Wizard]
    You help Umo, Umo help You. Oom! :) #Quest.Update.Moving_Stones.Umo
    ~ umoHelpAcquired = true
+ {!state_reached(helpTest, QuestFinished)} [Help Wizard]
    Umo can not. Need work. #Quest.Update.Moving_Stones.Umo
- -> Umo_Options

=== Learn_Dash ===
Umo show you magic.
# Spellbook.Add.Dash
- -> Umo_Options
