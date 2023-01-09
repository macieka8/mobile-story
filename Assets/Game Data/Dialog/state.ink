LIST playerGoblinsKnowledge = (None), GoblinsExist, GoblinsKilled
VAR blockingStoneDestoyedBy = ""
VAR minotaurKilled = false

=== function state_reached(stateMachine, state)
~ return LIST_VALUE(stateMachine) >= LIST_VALUE(state)

=== function state_equals(stateMachine, state)
~ return LIST_VALUE(stateMachine) == LIST_VALUE(state)
