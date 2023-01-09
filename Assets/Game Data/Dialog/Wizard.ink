VAR wizardLocation = "Center"

LIST wizardQuestState = (None),
lessGreenStarted, lessGreenCompleted,
movingStonesStarted, movingStonesCompleted,
theOtherSideStarted, theOtherSideCompleted

=== Wizard_Start ===
-> Wizard_Options

=== Wizard_Options ===
Oh hello there.
+ {wizardLocation == "Center"} [Talk]
    I remember the times when magic could be practiced freely
    Those where the times...
+ {wizardLocation == "Wizard Office"} [Talk] I might have a work for you.
* {wizardLocation == "Wizard Office"} [Help] -> Less_Green
* {wizardLocation == "Wizard Office" && state_equals(wizardQuestState, lessGreenCompleted)} [More Help] -> Moving_Stones
* {minotaurKilled == false && state_equals(wizardQuestState, movingStonesCompleted)} [Stone] -> The_Other_Side_Start
// Minotaur Endings
* {minotaurKilled == true} [Minotaur] -> Minotaur_Killed
// Spell Learning
* {state_equals(wizardQuestState, lessGreenCompleted)} [Fireball] -> Learn_Fireball
- -> Wizard_Options

=== Less_Green ===
Recently a pack of goblins decided to take a camp at our training ground.
{
- state_reached(playerGoblinsKnowledge, GoblinsKilled):
    -> Less_Green_Goblins_Killed
- else:
    -> Less_Green_Goblins_Exist
}

=== Less_Green_Goblins_Exist ===
I've been trying to talk them into leaving this place, but they weren't really keen on it.
I think you're the perfect man for the job to show them their place.
* [👍] 
    Thank you, young lad.
    I'm glad you're one of my students. #Quest.Start.Less_Green
* [👎]
    ... :(
- -> Wizard_Options

=== Less_Green_Goblins_Killed ===
...
What? You already took care of them.
I didn't expected that, well done.
Comeback later maybe I will have something for you. #Quest.Start.Less_Green
- -> Wizard_Options

=== Moving_Stones ===
I'm planning to clear the path to the Main Hall,
but I'm a little bit worried about what we can find there,
could you gather some people that would help us with the eventual problems?
* [👍]
    Alright, 
    ~ wizardQuestState = movingStonesStarted
    Talk to me when you're ready. #Quest.Start.Moving_Stones
* [👎]
    :(
- -> Wizard_Options

=== The_Other_Side_Start ===
Alright, let's go.
{blockingStoneDestoyedBy != "Player":
~ blockingStoneDestoyedBy = "Wizard"
... #Quest.Start.The_Other_Side #RaiseGameEvent.MoveStoneWizardDestroy
- else:
... #Quest.Start.The_Other_Side #RaiseGameEvent.MoveStoneAlreadyDestroyed
}
-> Wizard_Options

=== The_Other_Side_Talk_to_Wizard ===
{ blockingStoneDestoyedBy != "Player":
<> Let's see what we can find here.
- else:
<> Great! The stone is already out of the way.
<> Let's hope the rest of our trip will go just as easy..
}
#Quest.Update.The_Other_Side.1
Oh, That's a great place for polypody.
- -> Wizard_Options

=== Minotaur_Killed ===
{ state_reached(wizardQuestState, theOtherSideCompleted):
<> Now that all our main problems are out off the way
<> we can try to expand our academy.
- else:
<> What?
<> You cleared the path and killed the minotaur!
<> That's a great news!
<> Thanks to you we can now expand the academy.
}
# RaiseGameEvent.Game End
- -> Wizard_Options

=== Learn_Fireball ===
I think you are ready to learn Fireball.
# Spellbook.Add.Fireball
- -> Wizard_Options
