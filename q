[33mcommit 32197600050de4866a569b957b1f3463ea7bc76e[m[33m ([m[1;36mHEAD[m[33m -> [m[1;32mdevelopment[m[33m, [m[1;31morigin/development[m[33m)[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Thu May 29 15:05:54 2025 +0700

    Clean up CameraController

[33mcommit 3ca52085e7f78bdeb3bba5cede5a62dfe99ae4ca[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Thu May 29 13:52:53 2025 +0700

    Add UI assets

[33mcommit cbdc96ee6619a32bc76dc653a16f1aeb8a887e3d[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Thu May 29 10:13:08 2025 +0700

    Add camera screen shake effect
    
    - Camera can be shaked when pressing "V". This effect help with the impact of certain in-game actions.
    - The shake magnitude and duration can be set in the Inspector.

[33mcommit bd1939d9ac30db0619fe7439cd3517f8044bf3e4[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Thu May 29 09:40:48 2025 +0700

    Add smooth camera transition effect
    
    - Camera can be switched between two position: Menu View and Game View by pressing 1 (Game View) and 2 (Menu View) keys.
    - When switching camera, start a coroutine that smoothly change the camera's position and rotation to the target's position and rotation.
    - Camera control is disabled during transition, and enabled when transition end via EnableCanControl.
    - Camera's pitch value is adjusted arrcodingly, to prevent snapping when controlling the camera.

[33mcommit 7d0b17a80a339aa6dde8f4a8d226ada0f7e0bca1[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Wed May 28 21:49:46 2025 +0700

    Add restriction to camera's position
    
    - Camera has a max distance from the center point of the level
    - When the camera's position is too far from the center, restrict its position

[33mcommit 0d96570231d3e77710b116bf4eab0c1981693667[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Wed May 28 15:45:27 2025 +0700

    Add screen edge panning for mouse movement
    
    When moving the mouse to edges of the screen, the camera will move in that direction. The panning zone and speed can be changed in the Editor

[33mcommit 7839cc008a8aed178d3e72254cb114c58c56d0af[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Wed May 28 14:36:13 2025 +0700

    Add panning to mouse movement
    
    You can now pan the camera around when holding middle mouse button and drag it in the opposite of the direction you want to go.

[33mcommit 288213d06167c5047796926674eb67a547cf7c41[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Wed May 28 10:58:45 2025 +0700

    Add zooming functionality to the camera
    
    You can now zoom the camera by scolling the mousewheel. Min and max zooming distance can be set in the Editor to prevent Player from zooming too far or too close.

[33mcommit 29adf810c8295ecba17c33740b6c339a3ee1583d[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Wed May 28 10:33:35 2025 +0700

    Add camera handle rotation method
    
    Camera can now be rotated up and down by holding the right mouse button in Play mode. The camera's rotations are clamped within safe ranges. Avoid cases where Player might rotate under the level or flip the camera by accident

[33mcommit cfb0b2ca18cd42deb99f3e39df18f8c8ae4b30e3[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Wed May 28 09:39:34 2025 +0700

    Add CameraController for handling camera movement
    
    You can move the camera view vertically and horizontally based on the old input system

[33mcommit 644f023345601b1ce7ced650bb54a651c65fcee3[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Tue May 27 16:45:59 2025 +0700

    Make some changes to the enemy prefab

[33mcommit bf0c48dec6900d0d1ae8dfe0cf4ffeb38d9780dc[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Tue May 27 16:39:39 2025 +0700

    Add rotation visual for enemy
    
    Enemy can now rotate their body when going up or down a slope. Make for a better and smoother visual

[33mcommit 6a4257ee90617842fdf15cf35a79b87bc36a2ab3[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Tue May 27 15:44:47 2025 +0700

    Add a method of changing the level's layout
    
    - Rename from EnemyManager to WaveManager to better reflect its
      new responsibilities
    - WaveManager is now responsible for keeping track of the level's layout of
      each wave.
    - Layout changes are configured in the Inspector by assigning a new GridBuilder
      and associated enemy portals for each wave.
    - When a wave ends, WaveManager checks if a new layout is assigned. If so, it
      disables the current GridBuilder and activates the new GridBuilder and portals.

[33mcommit 2ccea58ee04a2ac2791b6f33005ed53cc79aa083[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Tue May 27 10:48:51 2025 +0700

    Remove redundant check

[33mcommit fd04ca2c712f689c6f0d561263f5071591d21cfc[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Tue May 27 10:34:30 2025 +0700

    Add a wave cooldown system
    
    - EnemyManager can check at an interval if all the active enemies have been destroyed from each portal.
    - If all active enemies have died, set the current wave as completed and run cooldown time. Setup the next wave when the time is up.

[33mcommit 4c0add46eada1a034e4ed8eaa45c2e31d2134592[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Tue May 27 09:11:18 2025 +0700

    Improve enemy waypoint switching and wave setup
    
    - Enemies now dynamically check whether to switch to the next waypoint by
      comparing their distance to the next waypoint with the distance between
      the current and next waypoints. This results in smoother movement and
      prevents enemies from fixating on a single waypoint.
    - Multiple enemy waves can now be configured in the Editor, with customizable
      enemy counts. The next wave can be manually triggered via the EnemyPortal's
      context menu.

[33mcommit 002244e41d5fd046ab6dd2bda6dda047d4876efe[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Mon May 26 16:15:20 2025 +0700

    Change waypoint management system
    
    - EnemyPortal is now responsible for collecting it own waypoints. Meaning waypoints that are setup from a portal to the player castle, will be collected by that portal
    - Each enemy collect its waypoints from the portal it spawn out of

[33mcommit f2b6c24bd138b3b00e89370712e699c9a2539e55[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Mon May 26 15:26:08 2025 +0700

    Improve enemy spawn system
    
    - Add a new EnemyPortal script which is responsible for spawning enemy at portal position
    - EnemyManager is now only keep track and setup next wave.
    - EnemyManager can set up next wave by distribute enemies to each portal in the scene.

[33mcommit c912c3a15056ca8a33a308d2e8ddd35b7ed4dde0[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Mon May 26 14:14:32 2025 +0700

    Add a way to build nav mesh when switch to road tile
    
    When switch to tile with layer road, the Editor can now bake (or build) a new nav mesh

[33mcommit ddc0d7f732e2fe8e88a1ea85a3d7a6a501dd2c56[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Mon May 26 11:20:42 2025 +0700

    Fix bridge tile prefabs position and collider

[33mcommit 0c551508ee12c2a99d279fdb997d389ee0d59e7a[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Mon May 26 11:03:15 2025 +0700

    Add two more corners prefab and delete uncessary scene

[33mcommit b44e291865643e21687590aad72d0432e0c582a2[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Mon May 26 10:04:06 2025 +0700

    Add a grid builder system and add do some changes in TileSlot
    
    - You can create and clear multiple tiles in the Editor by right-clicking the GridBuilder component in GridBuilder object.
    - Add comment for clarity in the SwitchTile method. Also add a line to change the tile's name when switching.

[33mcommit 42f05e1908bfc284eac5808fb2ecbd0bdce58784[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Sat May 24 10:34:39 2025 +0700

    Add buttons to rotate and reposition the tiles and also clear the code up a bit

[33mcommit fbbea76dce1b4c9f143513fde8134dbca7093191[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Fri May 23 15:58:40 2025 +0700

    Change some variable name for clarity

[33mcommit bea9370831b8047a545f164cccf5b461451f4519[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Fri May 23 15:43:03 2025 +0700

    Add a method to change collider when switching tile
    
    When a tile is switched only the mesh is changed, the collider is unchanged. So we have to update it with a new collider reference from the new tile.

[33mcommit 2a16971c06587bec73ce59189d7295786b31dc7c[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Fri May 23 15:19:50 2025 +0700

    Create a tile editor for switching tile
    
    Implement a tile switching system for easy level design. Tiles can be changed directly in the Inspector by using CustomEditor.

[33mcommit bb82eeddd6c6be0ebf2cfd9bfc3e6b48256300b8[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Fri May 23 11:15:10 2025 +0700

    Create tile prefabs for the levels
    
    Made roads, fields, corners, bridges and hills out of the original environment meshes into reusable prefabs.

[33mcommit 44fd14475a1c1b4ea1349fec9b6482e75df7b9cf[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Fri May 23 09:30:55 2025 +0700

    Add enemy type priority targeting system for towers
    
    Tower can now be assigned a priority enemy type. When a cluster of different enemy types are within attack range, it will prioritize the prioritize target first in tandem with targeting which enemy closest to the goal.

[33mcommit 42f4824ee0d403c4308505ccc6d65bafc3f9fbc5[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Wed May 21 16:12:32 2025 +0700

    Add a center point to enemy and improve crossbow tower visual
    
    - The enemies now have a center point object, so the tower can aim their attacks at it and not at the enemy's pivot point
    - Change the attack visual of the crossbow tower, so it follow the enemy instead of staying in place when the attack's over
    - The tower head's rotation is now enable in Awake, so it always rotate toward enemy when attacking

[33mcommit 5ec99e572bcfd664439816b5df29cc2f0b5a72b2[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Wed May 21 15:18:21 2025 +0700

    Change the name of some method

[33mcommit d61d2c1f0606f64f87882c0e9b200d7b672db7bf[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Wed May 21 15:11:12 2025 +0700

    Enhance enemy tracking for the towers
    
    - Tower can now priotity which enemy is the closest to the goal.
    - First, it gets all the enemy within its attack radius using Physics.OverlapSphereNonAlloc
    - Second, it try to get the Enemy component then add each one into a list if it exists
    - Third, it will find which enemy is the closest to the goal using the Enemy calculation of their remaining distance
    - Finally, return that enemy

[33mcommit 4bedd880a55f2fd8b0260e3ad8cb0c8d1694c0e7[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Wed May 21 11:33:35 2025 +0700

    Add calculation for the enemy's travel distance
    
    Setup the targeting priority for the tower. The idea is checking for the remaining distance of each enemy and their current waypoint. If one of the enemy has the remaining distance lower than the rest, priority that one.

[33mcommit e18ffaaa9c1049e04cb06eb4db7d0946b27ffadd[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Wed May 21 10:29:33 2025 +0700

    Add interface for enemies
    
    I implemented the IDamagable  interface for the enemies so the towers can deal damage to them through a common interface. This is for learning purposes

[33mcommit 10e1a408fc6cbc06465e505e8501914ea4fe2972[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Wed May 21 09:54:02 2025 +0700

    Make the crossbow tower head rotor move when attack
    
    The rotor part of the crossbow can now change its position along the crossbow's Z axis when the tower perform an attack.

[33mcommit bec1035e543ed8a6dcac733e6bf105a5abab351c[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Tue May 20 16:17:05 2025 +0700

    Add some string visuals to the crossbow tower
    
    I setup a bunch of game objects act as start point and end point for the crossbow string. Draw a line between those using LineRenderer similar to the attack visual. Not difficult, just really tedious.

[33mcommit 8a91c9cdb2e676bcf61fbdabac35a01c055311ac[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Tue May 20 14:31:35 2025 +0700

    Add dynamic color change on the emission material
    
    - Update the current emission color using Color.Lerp, based on the ratio of current intensity to max intensity.
    - Start the ChangeEmission coroutine in Awake so the crossbow begin charging when the game start.

[33mcommit 0b1419ae9b1576c21fe187cb77ae7e8bc9219365[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Tue May 20 11:03:54 2025 +0700

    Add emission intensity coroutine to crossbow tower
    
    Implemented a coroutine that gradually increases the emission intensity
    of the crossbow material from 0 to a maximum value over a fixed duration.

[33mcommit a752cd562f7e3995a904c59d0ca271716926f65d[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Tue May 20 10:17:11 2025 +0700

    Add attack visual effect for the crossbow tower
    
    - When the crossbow tower attack, a line visual is drawn from the center of the crossbow toward the enemy's direction. LineRender component was used with the emmission blue Material for the effect.
    - Add a couroutine to enable and disable the attack visual based on a duration.
    - Add a method to toggle the rotation of the tower's head so it stops rotating when the attack visual ends.

[33mcommit bb8bb72de6625e87a9016318516e250361ef5a92[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Tue May 20 09:17:46 2025 +0700

    Add Raycast for attacking the enemy
    
    Whenever the tower perfome attack, cast a ray from inside the tower head to the direction of the enemy.

[33mcommit 21354c0e34e517366e00971f1932439758427ac3[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Mon May 19 16:40:21 2025 +0700

    Refactor Tower into base class and add attack cooldown check
    
    - Changed variables and some methods in Tower to protected so that other tower types can inherit and override their behavior.
    - Added a cooldown check to control when towers can attack.

[33mcommit 1002007cc520bcaeab03ef7988be972e9f298765[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Mon May 19 15:43:24 2025 +0700

    Add function to find the closest enemy within range
    
    It job is to find enemies that are within the tower's attack range. I use Physics.OverlapSphereNonAlloc here for better perfomance (and don't have to deal with garbages latter).

[33mcommit 7cd8995b568540b1ca6f331a8f557eebd58d2eb4[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Mon May 19 10:33:43 2025 +0700

    Add a Tower Crossbow prefab with enemy-tracking rotation
    
    Set up a crossbow tower with a script that rotates its head to face the enemy.
    Unlike the Enemy's FaceTarget method, this version allows full-axis rotation
    and uses Euler angles for easier future adjustments.

[33mcommit 7a0dd16bbeb97407e196ae8efdc63e66b3419beb[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Mon May 19 08:39:46 2025 +0700

    Add comments to explain some methods

[33mcommit 57c11b513bfc1db428277a1d5c1e8192f6196570[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Mon May 19 08:28:18 2025 +0700

    Do some code clean up
    
    Just clean up some unused code and comments

[33mcommit e5d52af8d277ef6bbae39bc8904ca0a016b25535[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Sun May 18 20:20:02 2025 +0700

    Add wave system for spawning random enemies
    
    Implemented a wave system to spawn random enemies.
    
    - CreateEnemyWave() generates a list of enemies based on the number of each
      enemy type defined in the current wave.
    - GetRandomEnemy() selects and returns a random enemy from that list.
    - The selected enemy is then instantiated in CreateEnemy().

[33mcommit b2901d4a76aa097daba720a08e65b9068bebe3a8[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Wed May 14 15:53:51 2025 +0700

    Add Enemy Portal and Enemy Manager
    
    The Enemy Manager can take enemies prefab and spawn them at the portal's position. It have a cool down timer so it can spawn the enemies at a fix time

[33mcommit f423d812bc9fbe8fea16737a21632e61a0536c86[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Wed May 14 15:19:00 2025 +0700

    Add WaypointManager to manage stage waypoints
    
    The WaypointManager stores all waypoints in an array. At the start, enemies retrieve the waypoints by calling GetWaypoints() function in WaypointManager

[33mcommit 035432794616b9597e179f03c1535f03e60fb021[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Wed May 14 14:53:01 2025 +0700

    Add Player Castle as final destination for enemies
    
    Added a final destination for enemies: the Player Castle. When an enemy's collider touches the castle's collider, a function is triggered that destroys the enemy object.

[33mcommit b8cad0d2836b4546f1427e55a52c0765b853e110[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Wed May 14 11:04:40 2025 +0700

    Add RotateObject script to rotate each enemy's wheels
    
    Wheels now rotate along the X axis when the enemy moves.
    Each enemy has a different rotation speed based on their movement speed.

[33mcommit 625b160ffcbb386d7b968cf06decdcb85db2cd8a[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Wed May 14 10:36:59 2025 +0700

    - added a fast type enemy /n - changed the avoindancePriority of each Enemy Type so they faster one can avoid the slower one

[33mcommit da13b661fc41b741d80ec53c638cc4fd3a9a66bd[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Wed May 14 10:09:35 2025 +0700

    Created a function that control the Enemy's ratation when it's moving from waypoint to waypoint

[33mcommit 955c128353c96edb0dd8ddf7b6871261bce3e43f[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Tue May 13 21:17:37 2025 +0700

    setup some roads and fields as basic stage

[33mcommit 1b2f2e91bc465d07d1c052f74ca4333b12ad6f8a[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Tue May 13 20:27:08 2025 +0700

    added assets

[33mcommit 832bfcf7bd3bc0c43ee0fed9acee87d57f76eba0[m
Author: MinhNHN <minhnhn897@gmail.com>
Date:   Mon May 12 20:34:21 2025 +0700

    first commit

[33mcommit 7f34d7d9544c79c5a9b48619c8ae6b51840b19cf[m
Author: minh897 <minhnhn897@gmail.com>
Date:   Mon May 12 11:23:08 2025 +0700

    Initial commit
