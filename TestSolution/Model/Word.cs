using TestSolution.Model.Base;

namespace TestSolution.Model
{
    public class Word : BaseModel
    {
        public string Text { get; set; } = string.Empty;
        
        public int Frequency { get; set; }
    }
}