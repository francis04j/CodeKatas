# Bowling Game Kata

Credits
Inspired by [Uncle Bob](http://www.butunclebob.com/ArticleS.UncleBob.TheBowlingGameKata)

Bowling Rules
The game consists of 10 frames. In each frame the player has two rolls to knock down 10 pins. The score for the frame is the total number of pins knocked down, plus bonuses for strikes and spares.

A spare is when the player knocks down all 10 pins in two rolls. The bonus for that frame is the number of pins knocked down by the next roll.

A strike is when the player knocks down all 10 pins on his first roll. The frame is then completed with a single roll. The bonus for that frame is the value of the next two rolls.

In the tenth frame a player who rolls a spare or strike is allowed to roll the extra balls to complete the frame. However no more than three balls can be rolled in tenth frame.

Requirements
Write a class Game that has two methods

1. void roll(int) is called each time the player rolls a ball. The argument is the number of pins knocked down.
2. int score() returns the total score for that game.

## Solution 1

### Thought process and pseudo-code
There is no expectation given for the roll method.
In a real life scenario, i will have clarified and walkthrough my assumption
However, this is no real life and i cant talk to Uncle Bob.
So i just have to go with my assumptions.
My assumption is that we are supposed to use the two methods above to 
determine the total score of the game.
Then the question is why do we need the Roll method. Why not just use score.
So i'm guessing we want to be able to get Score at any time after a roll.
Going with that assumption, what will the behaviour of Roll?

Using first principle thinking, the simplest truthful thing that Roll can do
is record the pins. i.e. store it somewhere

Possible storages: 
 - an array: allow us to store rolls per frame. but what size? 
20? because of 2 rolls per 10 frames. 2x 10
21? so we can store that extra roll in the tenth frame
x? what if the player doesnt use 2 rolls per frame. if there is a strike or spare, roll number changes,
we are now over-allocating.
is there any data structure that can allow us to store like the array but not over/under-allocate
 - A list: offers dynamic sizing and we can still record rolls sequentially. 
List also offers the 0(1) random access like array
 - A queue is not good for random access which is need to calaculate spare/strike bonus
 - Stack and LinkedList 
 - A Dictionary will allow us to store total rolls per frame. however, when calculating spare or strike,
it wont allow us to access the value of each roll since they are now "total-ed"
 - List seems to be the simplest and most useful
List<int> rolls = new (); rolls.Add(pins)

####What about Player that rolled the ball? shouldnt we track that in the roll method?
The problem description does not specify the need to track multiple players or associate rolls with a player.
So we can just use the roll method to record the pins.

####Now the score method.
I'm guessing this can be called at any time. So we should account for that.
First test scenario is that no rolls have been made and score() is called then we should return 0.
Second test scenario is when rolls are made but with no spare or strikes.
How will this work(Pseudo-code)
for i in frame.Count
  get roll i roll(i)
   get roll :  roll(i+1)
if score is called here, the roll(i) will throw exception 
The score being called anytime won't work (BAD Assumption)

It has to be called at the end of the game
