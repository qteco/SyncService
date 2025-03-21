namespace SyncService.Core;

public class ExactIdGenerator
{
    public static string GenerateExactId()
    {
        int maxCharacters = 9;
        var random = new Random();
        string id;
        string newString = "";
        
        for (int i = 0; i < maxCharacters; i++)
        { 
            int c = random.Next(0, 9);
            id = c.ToString();
            newString += id;
        }
        
        return newString;
    }
}