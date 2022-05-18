# Card-Game-Test
Description:
This is a project for making a replicate of Inscryption (under development).
Contains basic turn-based card game battle system.
Contains a 3x5 grid (row 1 at the bottom is for the player, row 2 and 3 are for the enemy AI).

# Each turn processes as of now:
1. Start of player's turn.
2. Player can use mouse to click on either player's deck (left) or squirrel deck (right) to draw a single card.
3. Player can choose to play the card onto the board (by putting the card on hold to the left of the screen and click on the desired cell on the board) or ring the bell to start the battle phase.
4. The battle phase automatically runs from left to right and switch to the enemy's turn.
5. Start of enemy's turn.
6. The battle phase automatically runs from left to right, top to bottom (top row moves first, bottom row attacks later) and switch back to player's turn.

# Controls:
Left click:
+Deck: Draw a card and put into hand
+Card: Pick a card from hand
+Cell (embedded into the white board): Play the picked card onto the selected cell
+Bell (yellow on the left): Ring the bell to signal the auto battle phase

# Notes:
Bugs or Fixes need to be patched:
+A bug where the card instantly after being drawn would snapped to the right if mouse is hovered over the card before it stablizes in hand
+Unable to put the card back to hand after picking it up (not implemented)
+No visual indication for cells to easily select
+No visual indication for deck to easily select