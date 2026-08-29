using Godot;
using Godot.Collections;

namespace BrackeysGameJam2026;

[Tool]
public partial class Character : Button
{
	[Export(PropertyHint.MultilineText)] public string Hint { get; set; }
	[Export] public bool Murderer { get; set; }

	[Export] public Array<AudioStream> Samples { get; set; } = [];

	private AudioStreamPlayer AudioPlayer => this.GetNode<AudioStreamPlayer>("AudioStreamPlayer");

	private TextureRect ButtonTexture => this.GetNode<TextureRect>("TextureRect");

	[Export]
	public Texture2D Texture
	{
		get => this.ButtonTexture.Texture;
		set => this.ButtonTexture.Texture = value;
	}

	public override void _Ready()
	{
		AudioStreamRandomizer stream = new()
		{
			StreamsCount = this.Samples.Count,
		};
		for (int i = 0; i < this.Samples.Count; i++)
		{
			stream.AddStream(i, this.Samples[i]);
		}

		this.AudioPlayer.Stream = stream;

		if (this.Samples.Count > 0)
		{
			this.Pressed += () => this.AudioPlayer.Play();
		}
	}
}