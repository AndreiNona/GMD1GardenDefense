# GMD1
This repository is earmarked for GMD1 projects ande blog posts

# Roll a ball

Roll a ball is the introductory project, meant to familiarize one with basic game design principles and mechanics. From the beginning, the project was rather lacking in scale and ambition. This being said, the basis of the project is split. Rolling a ball, you have a relatively good amount of control over has always proved fun for people. Even in an unfinished state, with only a play area to test mechanics, the gameplay was fun and engaging. 

This being said, the fun grew exponentially once features were added, such as obstacles and a bounce mechanic, allowing for the ball to feel more natural. This bounce also allowed the ball to feel like it has been made from different materials. With an extra springy bounce, the ball without any visual changes, already felt like rubber. Same goes for more rigid bounces, giving the impression of a metal ball ricocheting form a steel plate.

Another addition that enhanced the feeling was the creation of an on-screen display, of both the score and the win message. This system is a given for players and allows the game proper to communicate necessary information. For example, falling into the void or your ball exploding would feel rather confusing if you did not have a visual sign showing you that the game is over as intended.

As for the levels, this game would lend itself to replicability. As such the idea of creating a procedurally generated tile set, in order to essentially create an endless variety of levels is a tempting one. But due to the time constraints and explanatory nature of the project this option is but a mere dream.

As a conclusion, this project is a great general introduction to the game design world, allowing for a basic introduction as well as leaving room for plenty of improvements and personalization’s to be had.

# Milestones

Simply put, the milestones of this project will be split into 3 main categories.

1: Basic systems
  This milestone consists of achieving a basic way of navigating the game. Ranging from navigating menus to moving around in the game space.
  The milestone is considered complete when a basic system is implemented, this being said overhauling it and refining the system based on testing and in concordance with the evolving theme of the game is to be expected.
  In this stage we are looking at creating the building blocks for the game. This delivery will help guide the future design of the project as the implementation will have to be done in accordance with the systems we are creating at this stage. 
  The biggest consideration to the input system is also accounted for in this stage. The expected hardware does not have all the buttons and mouse movement expected of a PC. As such by creating a standardized button distribution and menu interaction experience, we lower the odds of unwilling overloading the controls. Otherwise, we can end up with a worst-case scenario where the number of actions exceeds the expected input limit

2: Game loop
  This milestone consists of creating a feasible game loop that provides the player with a main goal, activities and loose conditions.
  At this stage of the project a few items are expected to be implemented but not necessarily polished. They include A basic enemy and the expected behavior, a basic resource management, interactive objects and of course a map.
  In this stage we are developing the so-called "bread and butter" of the game. The most important design part being the creatin of the map, followed closely by resources and enemies. Each of these elements provides different values to the project. The map is essential for immersion and a sense of being in the world allowing for a believable or at the very least enjoyable experience. 
  The resources provide a sense of progress and reward, serving as a pseudo tutorial, they allow the player to deduce how well they performed, what actions provided the best rewards and what strategies are the easiest or most satisfying. The enemy provides a sense of danger, keeping the player alert and providing an adversary.
  
3: Improvements, additions, testing restructuring 
  This milestone is the broadest of them all. The goal of this milestone is to streamline the experience, improve the created systems, add desirable features and remove unneeded or bloating features.
  There is no set number of improvements or any feasible way to quantify the completion of this milestone, at the very least not during this early stage.
  As the game is built and finalized, through testing certain elusive truths might become apparent, then they do they are to be noted and revisited during this stage, as changing different aspects of the game might, over time deviate the game too much from the identity we strive to build for it.
  As an example: The menu system makes perfect sense on paper, is feasible when developing but for whatever reason (maybe we left it open for new features we want to implement but the features got dropped), it does not feel right when the game is complete. Overhauling the menu so that it best fits with the game would fall into the tasks covered by this milestone.

# Deve update: Basic systems

Implementing the basic systems has been completed. We are satisfied with the implementation so far and we want to present our findings and conclusions.
We will begin, as our game does to, with the menus. So many menus! The game utilizes the build in unity system for handling element order, allowing the navigation and changing of elements through only using buttons. Adhering to the expected buttons and inputs proved the biggest challenge in this stage, as all the inputs have to be easily utilized with a few keys. This means that basic elements such as sliders, buttons and drop downs had to be used.
The menu itself is held in a different scene than the game. This proves both useful and a hinderance. Due to it being a separate scene, we need not be concerned with game related variables and objectives right from the beginning. This choice also means that we are not required to change game objects on the fly while menu changes take effect. 
On the other hand, due to it being in a different scene, it makes accessing options and changing your game on during your game not possible, or at the very least, not easily achieved. For the purpose of this project, we have decided against it, as the added menu in a game space would both require us to use marginally different versions of the menus for both instances, as well as create additional factors the user will need to consider when playing the game.
Speaking of the game, how is the movement system working? So far the movement system has been faring basic, with 2 inputs for X and Y. The character is able to perform a full range of movements in 2D space. It holds the bug of diagonal walking proving faster, as the forces are additive, but this feature has been kept adding more nuance to the range of actions a player can perform.
Furthermore 2 buttons have been allocated for user actions, another 2 have been dedicated to character interactions (be it yes or no, next or previous). A last button has been designated for exiting the game.

# Deve update: Game loop

We are back to talk about our favorite part! The game loops. The game has been refined as a turn-based(ish) tower defense. Now for our Q&A!
Q: What are we defending? A: A big plant. 
Q: Why? A: Unsure our lore team (later me) has not been employed yet. 
Q: What are we defending it from? A: Pollution (caused by evil possessed robots)! 
Q: Why this set of very specific goals and enemies? A: Because paying for assets is not an option and developing all of them manually would extend the deadline of this project past the release date of Half Life 3. Another option is compiling assets from different bundles, while the idea works, shopping for assets is not our main goal.
Q: But is this just a tutorial? A: No, it is not. It is true, we are using assets that come from a project tutorial, and we are implementing things similarly at points. This being said we are using parts of the tutorial only where it makes sense, take for example the animations. We have no desire to reinvent the wheel or make new parameters for animations or different animations just for the sake of being different.
A2: One good example is our map, the original assets proved to, dead looking for our tastes, so we had our visual design team (past me) touched up the sprites in an image editor to best fit our game. The map is custom made, the enemies hold the original sprites and animations but run on our scripts, the collectables... well they are mostly the same, just changed to work with our unified health system (patent pending. maybe).
Q: You talked about your questionable yet legal use of free assets, but how does the game loop work?
A: Our game loop is a mix between a tower defense and a horde styled game, with some round breaks throwing in for good measure. In short, the game is endless, you cannot win per say, just get as far as possible before you lose. You can lose when either you or the structure you need to protect are eliminated. The enemies are many and increase in number the more you progress (also in difficulty but not entirely, apparently you need to dedicate quite a bit of time to ensure scaling enemies don't 1 shoot you after a while, or that you don't 1 shoot them).
In this endeavor the player can now place defensive structures, as well as production structures. Early on they are just a nice bonus, later on, proper management is integral to completing rounds.
This is all we have time for. Join us next time when we refine our process and finish our masterpiece. And remember, this is just a teasing of the game, if you want to experience it fully, be sure to give it a try when we release it!

# Deve update: Improvements, additions, testing restructuring 

As the last major update of the project, we are happy to announce that our project received some much-desired features.

We have added an end game screen, before this stage our project lacked the crips ending it needed. This screen shows a rundown of stats, such as structures placed enemies healed and so on. This feature allows the user to not be trusted back into the main screen without warning. We also added a button for quickly getting back into another game. Consideration for further enhancing the game with persistence have been made as such the tracked variables will be easily saved if such a design choice is made in the future.
We have also standardized the health and damage system, so that new additions and improvements can be made in the general system and be felt across the entire game. This being said our players continue to use their own health as this allows us to have more control over what we want the player to feel like, without having to implement checks for all the other entities that use the system.
We have also added lore to the game, rudimentary but needed, as the testing showed a lack of direction from players when first starting the game. In addition, we have also standardized the layout of the game so that it can properly function on a variety of devices. Propper UI element scaling has been added to compliment this change.
Overall, we would say the game is in a releasable state. With no major game braking bugs in sight, we are confident this version can and will resist more in depth testing done by our player base (we are but a small team of 1, so we can only test so much)