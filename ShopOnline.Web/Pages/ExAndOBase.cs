using Microsoft.AspNetCore.Components;
using System.Linq;
using System.Text.Json;

namespace ShopOnline.Web.Pages
{
	public class ExAndOBase : ComponentBase
	{
		/*Saved Points */
		public List<int> ListXSavedPoints { get; set; } = new();
		public List<int> ListOSavedPoints { get; set; } = new();

		/*Contains All Possible Answers*/
		List<List<int>> Answers { get; set; } = new();


		/*Toggles between X and O*/
		public string TogglePlayerXandO { get; set; } = "O";

		/*Cells being displayed*/
		public Dictionary<int, string> NumberOfCells { get; set; } = new();

		public string WinnerOfGame { get; set; } = null;




		
		/*Solution
		/*********************/



		/*********************
		Initaliser
		/*********************/

		protected override void OnInitialized()
		{
			InitialiseGame();
			GetPossibleAnswers();

		}

		protected void TogglePlayer()
		{
			TogglePlayerXandO = (TogglePlayerXandO == "X") ? "O" : "X";

		}


		private bool checkIfEqual(List<int> savedPoints, List<int> answersPoints)
		{
			bool allContained = answersPoints.All(point => savedPoints.Contains(point));

			return allContained;
		}

		protected async Task SubmitPoints(int pointPosition)
		{	
			if (NumberOfCells[pointPosition] == ""){
				TogglePlayer();
				if (TogglePlayerXandO == "X")
				{
					NumberOfCells[pointPosition] = "X";
					ListXSavedPoints.Add(pointPosition);
					var answers = Answers.Where(e => checkIfEqual(ListXSavedPoints, e)).ToList();

					if (answers.Count == 1)
					{
						await Task.Delay(500);
						WinnerOfGame = "X wins the game";
					}
				}


				if (TogglePlayerXandO == "O")
				{
					NumberOfCells[pointPosition] = "O";
					ListOSavedPoints.Add(pointPosition);
					var answers = Answers.Where(e => checkIfEqual(ListOSavedPoints, e)).ToList();

					if (answers.Count == 1)
					{
						await Task.Delay(500);
						WinnerOfGame = "O wins the game";
					}
				}

			}

			if (WinnerOfGame == null)
			{
				bool isDraw = NumberOfCells.All(e => e.Value != "");
				if (isDraw) WinnerOfGame = "It's a draw";
			}
			


		}


		public Dictionary<int, string> PopulateCells(int plays = 3)
		{
			var totalNumber = plays * plays;
			var grid = Enumerable.Range(0, totalNumber).ToList();

			var cellsDictionary = new Dictionary<int, string>();
			foreach (int number in grid)
			{
				cellsDictionary.Add(number, "");
			}

			return cellsDictionary;
		}

	

		protected void InitialiseGame()
		{

			NumberOfCells = PopulateCells();
			WinnerOfGame = null;
			ListXSavedPoints = new();
			ListOSavedPoints = new();
		}

		private void GetPossibleAnswers()
		{
			
			int numberOfCells = 3;
			List<List<int>> Grid = new();


			int start = 0;
			for (int j = 0; j < numberOfCells; j++)
			{
				int intialiser = start * numberOfCells;
				List<int> rows = new();
				for (int i = intialiser; i < intialiser + 3; i++)
				{
					rows.Add(i);
				}
				start++;
				Grid.Add(rows);
			}
		

			//Horizontal Line
			foreach (var item in Grid)
			{
				Answers.Add(item);
			}

			//Vertical Line
			for (int i = 0; i < Grid.Count(); i++)
			{
				var answer = new List<int>();
				for (int j = 0; j < Grid.Count(); j++)
				{
					answer.Add(Grid[j][i]);
				}
				Answers.Add(answer);
			}


			//Diagonals
			var diagonalStart = new List<int>();
			var diagonalEnd = new List<int>();
			for (int i = 0; i < Grid.Count(); i++)
			{
				diagonalStart.Add(Grid[i][i]);
				diagonalEnd.Add(Grid[i][(Grid.Count() - 1) - i]);

			}

			Answers.Add(diagonalStart);
			Answers.Add(diagonalEnd);

	
		}
	}
}
