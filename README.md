# Minecraft 
Simple game, written on C# as university course project

## Project structure

#### Infrastructure
Base class, which can be used independently from this project

#### Domain 
Game-related classes are here (like block, world, game, chunk)

#### Application
Classes which used to visualize logical objects, in addition. Also updates current data. Uses OpenTK

## Current features
*Few world generators*

*Few Player movement controls*

## Extension points
'''
1. Can add new Chunk generator through IGenerator realization
2. Adding new Commands through ICommand
3. Can fully rewrite movement physics by writing class with EntityMover as parent class
'''

## Authors
[@UltraHybrid](https://github.com/UltraHybrid)
[@LobachevDanil](https://github.com/LobachevDanil)
[@SavelevMatthew](https://github.com/SavelevMatthew)
[@DmitriuSsS](https://github.com/DmitriuSsS)