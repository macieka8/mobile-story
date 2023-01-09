
=== Uma_Start ===
I AM BUSY!!!
-> Uma_Options

=== Uma_Options ===
Hmm...
+ [Talk] Mix this one with that one, and that one with this one hmmm...
* {state_equals(playerGoblinsKnowledge, GoblinsExist)} [Goblins?] -> Uma_Goblins_Encounter
* {state_equals(playerGoblinsKnowledge, GoblinsKilled)} [Goblins] -> Uma_Goblins_Killed
* {state_equals(helpTest, QuestStarted)} [Help Umo] -> Uma_Help_Umo
* {state_equals(wizardQuestState, movingStonesStarted)} [Wizard] -> Uma_Help_Wizard
- -> Uma_Options

=== Uma_Help_Umo ===
My brother send you to me?
~ helpTest = TalkedWithUma
hmmm... In that case, tell him I will visit him tomorrow. #Quest.Update.Help_Umo
-> Uma_Options

=== Uma_Help_Wizard ===
I'll pick up my stuff and head over there when I'm needed. #Quest.Update.Moving_Stones.Uma
-> Uma_Options

=== Uma_Goblins_Encounter ===
Yes, we do have problems with goblins.
Try asking Wizard about it, maybe he will know how to deal with it.
-> Uma_Options

=== Uma_Goblins_Killed ===
Finally someone dealt with it.
Maybe my ingredients will stop disapearing now.
Take these potions. Just don't drink them.
They could melt a rock. # Item.Poison Throwable Potion:3
-> Uma_Options
