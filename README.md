# IgnizMelange

This mod aims to add some of my desired fixes / adjustments to Ages of Conflict: World War Simulator \
It utilizes [BepInEx v5.4.23.2](https://github.com/BepInEx/BepInEx) to be loaded into the game.

## All wars are endless. 

By default, wars tend to peace out and make constant border wars, and I personally enjoy fights till the annexation of one country or another. \
This may be expanded in the future to be more complex.

## Accompanying gold changes to help aid endless wars. 

By default, if two nations go broke, they cannot fight and therefore peace out. \
I have made a system where gold affects combat effeciency and continues production during wars to help prevent complete stalemate. \
Stalemates still occur obviously, but without these changes wars would still randomly peace out, or stall for a bit more. \

A more detailed explanation of how gold works now is like this: 
1) Gold is still produced during wars, but at 1/3rd value
2) If a nation has double the gold of all its enemies combined, it cannot drop combat effeciency past 3
3) If a nation has 4x the gold of all its enemies it cannot drop combat effeciency past 5
4) If a nation has 50 gold or less, it cannot reach higher combat effeciency than 2
5) If a nation has less than 10 gold, it will have zero combat effeciency and cannot win battles
6) If both nations hit 0 they will not immediately peace out, hopefully the gold production and effeciency will prevent this from stalemating 

## Configurable

By default, the two options listed above (Endless wars, and gold changes) are able to be toggled on/off in BepInEx' config \
Located in ```BepInEx/config/org.IgnizHerz.mod.IgnizMelange.cfg``` [Inside the game's directory, (Where you installed BepInEx)](https://www.youtube.com/watch?v=PFt5Jw65jrM) \
By default, their value is ```true``` and you can change them to be ```false``` if you want to disable them. \
Hopefully in the future this will be integrated inside the game itself so you can toggle these options in-game. 
