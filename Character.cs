using Godot;

namespace BrackeysGameJam2026;

[Tool]
public partial class Character : Button
{
	[Export(PropertyHint.MultilineText)] public string Hint { get; set; }
	[Export] public bool Murderer { get; set; }
	
	private TextureRect ButtonTexture => this.GetNode<TextureRect>("TextureRect");

	[Export]
	public Texture2D Texture
	{
		get => this.ButtonTexture.Texture;
		set => this.ButtonTexture.Texture = value;
	}
}
