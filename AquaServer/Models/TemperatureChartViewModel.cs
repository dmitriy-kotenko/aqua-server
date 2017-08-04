using System.Collections.Generic;
using Newtonsoft.Json;

namespace AquaServer.Models
{
	public class TemperatureChartViewModel
	{
		public List<Button> Buttons { get; }

		public object[][] Data { get; private set; }

		public TemperatureChartZoneModel[] Zones { get; set; }

		public int SelectedButtonIndex { get; set; }

		public TemperatureChartViewModel(object[][] data)
		{
			Data = data;

			Buttons = new List<Button>
			{
				new Button("hour", 1),
				new Button("hour", 5, "5 hours"),
				new Button("hour", 9, "9 hours"),
				new Button("day", 1),
				new Button("week", 1),
				new Button("all", 1, "All data")
			};

			SelectedButtonIndex = 2;
		}

		public void RemoveLastButtons(int count)
		{
			Buttons.RemoveRange(Buttons.Count - count, count);

			if (Buttons.Count <= SelectedButtonIndex)
			{
				SelectedButtonIndex = Buttons.Count - 1;
			}
		}

		public class Button
		{
			[JsonProperty(PropertyName = "type")]
			public string Type { get; set; }

			[JsonProperty(PropertyName = "count")]
			public int Count { get; set; }

			[JsonProperty(PropertyName = "text")]
			public string Text { get; set; }

			public Button(string type, int count, string text)
			{
				Type = type;
				Count = count;
				Text = text;
			}

			public Button(string type, int count)
				: this(type, count, $"{count} {type}")
			{
			}
		}
	}
}