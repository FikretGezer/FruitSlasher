# [Fruit Slasher](https://play.google.com/store/apps/details?id=com.FikretGezer.FruitSlasher)
## Screenshots
<div align="center">
  <img src="https://github.com/FikretGezer/FruitSlasher/assets/64322071/43888c03-484a-4870-a0f6-14ec489e40ee" width="400" height="220">
  <img src="https://github.com/FikretGezer/FruitSlasher/assets/64322071/84cd2f5d-62fc-44da-a669-5218cd73b112" width="400" height="220"> 
  <img src="https://github.com/FikretGezer/FruitSlasher/assets/64322071/09dee3c6-7d82-44e1-b31f-2ad6f478442f" width="400" height="220"> 
  <img src="https://github.com/FikretGezer/FruitSlasher/assets/64322071/4596d056-4cd0-4496-9dd5-d95949cd9f56" width="400" height="220">
  <img src="https://github.com/FikretGezer/FruitSlasher/assets/64322071/8918887c-4780-48be-bd0d-0b7f1aba6407" width="400" height="220"> 
  <img src="https://github.com/FikretGezer/FruitSlasher/assets/64322071/b2790966-80d9-471b-86ae-a5ec97ad59ba" width="400" height="220"> 
  <img src="https://github.com/FikretGezer/FruitSlasher/assets/64322071/e0d9dccb-d48b-4561-b478-79e9d6323350" width="400" height="220"> 
  <img src="https://github.com/FikretGezer/FruitSlasher/assets/64322071/bf1d7662-8735-4dd3-a138-332853fe3c41" width="400" height="220"> 
</div>

## What is this game about?
Fruit Slasher is a clone of a known game Fruit Ninja. There is currently 1 mode in the game and the goal of the mode is slicing fruits as much as possible while avoiding bombs. End of the each round players earns some in game currency called "Stars" and experience to level up. Once players level up, some blade or some dojos will be unlocked and they can buy these items from the in game store with stars they own and players can see in the store, which level the items will be unlocked.

Also there are:
* 10 different blades
* 6 different dojos in the game.

## Background and Development
I tried to analize what can I do and what I can't do before developing the game and wrote everything down. During the development I used OOP, implemented store, created a mission board, implement object pooling for fruits, bombs, effects etc., created different blades using unity's particle system, implement Event Manager using Observer Pattern, implement Google Play Gaming Services' cloud save, achievemnts, leaderboard and more.
* Using the [Google Play Games Services Plugin](https://github.com/playgameservices/play-games-plugin-for-unity), I implemented a leaderboard to give access to the players to see what other players scores, implemented cloud save so players keep playing the game from the other devices without losing their progress and implemented achievement to give players little bit more challenges.
* I used OOP to create a Event Manager. I created enum called Game Events and declared bunch of events to use subscribing from different scripts and when something happened to related to the event I broadcasted the event in the needed script.
* Created a store for players to buy new blades and dojos to give them something to pursue. I created a scriptable object to store these items and added one by one after adjusting the parameters (sprites, unlock level, price, etc.). So players can see which item unlocks when and when players reach out the certain level for an item, they can buy it if they have enough Stars.
* Used similar type of approach for the mission creating. I stored different type of missions in scriptable object and if there is no active missions, game randomly select 3 different mission and assign them on the board. They will stay there until players complete them.
* Implemented pooling system to avoid performance problems by creating and destroying the reusable objects.
* Created simple animations for the menus and buttons.
* Tried to created dynamic UIs with default Unity components like Layout groups.
* Implemented beginner guide to show players around by using inverted mask with materials.
* and more...
