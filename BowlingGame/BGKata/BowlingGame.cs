namespace BGKata;

public class BowlingGame
{
    
    IStorageService _storageService;

    public BowlingGame(IStorageService storageService)
    {
        _storageService = storageService;
    }
    /// <summary>
    /// This is called each time the player rolls a ball
    /// </summary>
    /// <param name="pins">number of pins knocked down</param>
    public void Roll(int pins)
    {
        if (pins > 10)
        {
            throw new ArgumentOutOfRangeException(nameof(pins), "Pins must be between 0 and 10");
        }
        _storageService.AddPin(pins);
        
    }

    public int Score() // 2,1, 3,4, 4,5
    {
        int totalScore = 0;
        IEnumerable<int> rolls = _storageService.GetRolls();
        int f = 0;
        bool isStrikeCalculation = false;
        for(int i = 0; i < 10; i++)
        {
            var currentFrameScore = 0;
            if (f < rolls.Count())
            {
                if (isStrike(rolls.ElementAt(f)))
                {
                    isStrikeCalculation = true;
                    currentFrameScore += rolls.ElementAt(f);
                    //add strike bonus
                    if (f + 1 < rolls.Count())
                    {
                        currentFrameScore += rolls.ElementAt(f + 1);
                        if (f + 2 < rolls.Count())
                        {
                            currentFrameScore += rolls.ElementAt(f + 2);
                        }
                    }
                }
                else
                {
                    currentFrameScore += rolls.ElementAt(f);
                }
               
            }
            
            if(f + 1 < rolls.Count()){ //check if we are at the penitulate roll
                if (isStrike(rolls.ElementAt(f+1)))
                {
                    //add strike bonus
                    if (f + 1 < rolls.Count())
                    {
                        currentFrameScore += rolls.ElementAt(f + 1);
                        if (f + 2 < rolls.Count())
                        {
                            currentFrameScore += rolls.ElementAt(f + 2);
                        }
                    }
                }
                else
                {
                    currentFrameScore += rolls.ElementAt(f+1);
                }
            }

            if (isStrikeCalculation == false && currentFrameScore == 10) //if spare
            {
                if (f + 2 < rolls.Count())
                { //add spare bonus
                    currentFrameScore += rolls.ElementAt(f + 2);
                }
            }
            
            
            totalScore += currentFrameScore;
            f+=2;
            isStrikeCalculation = false;
        }
        return totalScore;
    }

    private bool isStrike(int elementAt)
    {
        return elementAt == 10;
    }
}

public interface IStorageService
{
    public void AddPin(int pin);
    public IEnumerable<int> GetRolls(); 
}

public class StorageService : IStorageService
{
    List<int> rolls = new();

    public void AddPin(int pin)
    {
        rolls.Add(pin);
    }

    public IEnumerable<int> GetRolls()
    {
        return rolls;
    }
}