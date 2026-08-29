using Godot;

namespace BrackeysGameJam2026.Core.UserData;

public partial class Settings : Resource
{
    [Export]
    public double MainVolume { get; set; }
    
    [Export]
    public double MusicVolume { get; set; }
    
    [Export]
    public double EffectsVolume { get; set; }
    
    [Export]
    public double QuotesVolume { get; set; }
}