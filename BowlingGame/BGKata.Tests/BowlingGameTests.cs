using Moq;

namespace BGKata.Tests;

public class BowlingGameTests
{
    private Mock<IStorageService> storageService;
    private BowlingGame game;
    
    public BowlingGameTests()
    {
        storageService = new();
        game = new BowlingGame(storageService.Object);
    }
    
    [Fact]
    public void Rolls_ShouldAddPins()
    {
        
        game.Roll(3);
        
        storageService.Verify(x => x.AddPin(3), Times.Once);
    }

    [Fact]
    public void Rolls_ShouldAddMaxTenPins()
    {
        
        Assert.Throws<ArgumentOutOfRangeException>(() => game.Roll(11));
    }

    [Fact]
    public void Score_ShouldReturnZero_WhenNoRollsHaveBeenMade()
    {
        
        storageService.Setup(x => x.GetRolls()).Returns(new List<int>());
        
        var score = game.Score();
        Assert.Equal(0, score);
    }

    [Fact]
    public void Score_ShouldReturnTotalScore_WhenRollsWithNoStrikesOrSparesHaveBeenMade()
    {
        storageService.Setup(x => x.GetRolls()).Returns(new List<int>(){ 2,1, 3,4, 4,5});

        var score = game.Score();
        
        Assert.Equal(19, score);
    }
    
    [Fact]
    public void Score_ShouldReturnTotalScoreForCompletedFrames_WhenNoStrikesOrSpares()
    {
        storageService.Setup(x => x.GetRolls()).Returns(new List<int>()
            { 5,1, 2,1, 4,3, 
                5,4, 4,4, 4,5,
                4,2, 3,4, 6,1,
                3,3
            });

            var score = game.Score();
        Assert.Equal(68, score);
    }
    
    [Fact]
    public void Score_ShouldReturnTotalScoreForFrames_WhenThereIsASpare()
    {
        var rolls = new List<int>()
        { 5,1, 9,1, //spare
            5,4 // bonus is 5
        };
        storageService.Setup(x => x.GetRolls()).Returns(rolls);
        int expectedScore = rolls.Sum() + 5;
        var score = game.Score();
        Assert.Equal(expectedScore, score);
    }
    
    [Fact]
    public void Score_ShouldReturnTotalScoreForCompletedFrames_WhenThereIsASpare()
    {
        var rolls = new List<int>()
        { 5,1, 9,1, //spare
            5,4 // bonus is 5
            ,3,4,
            6,3,
            5,2,
            5,4,
            5,2,
            4,4,
            5,3
        };
        storageService.Setup(x => x.GetRolls()).Returns(rolls);
        int expectedScore = rolls.Sum() + 5; 
        var score = game.Score();
        Assert.Equal(expectedScore, score);
    }
    
    [Fact]
    public void Score_ShouldReturnTotalScoreForFrames_WhenThereIsAStrike()
    {
        var rolls = new List<int>()
        { 5,1, 10, //strike
            5,4 // bonus is 5 + 4
        };
        storageService.Setup(x => x.GetRolls()).Returns(rolls);
        int expectedScore = rolls.Sum() + 5 + 4;
        var score = game.Score();
        Assert.Equal(expectedScore, score);
    }
}
