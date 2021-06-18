

public class Enums 
{
    public enum Resources
    {
        darkMatter, //#3
        spacePig,   //#4
        water,      //#4
        metal,      //#3 
        energy,     //#4
        sun         //#1
    }
    public enum BuildingType 
    { 
        Road, 
        Settlement,
        City,
        None 
    };


    public enum GameState
    {
        waitingForPlayers,
        mapGeneration,
        turnDetermization,
        preGame,
        inGame,
        postGame
    }
}
