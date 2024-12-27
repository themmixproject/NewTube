namespace NewTube.Client.Models
{
    public class Anchor
    {
        public string? Href { get; set; }
        public string? Label { get; set; }
    
        public Anchor(string href, string label)
        {
            Href = href;
            Label = label;
        }
    }
}
