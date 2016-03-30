namespace ADServerDAL.Entities
{
	/// <summary>
	/// Klasa przechowująca informacje o stronnicowaniu
	/// </summary>
	public class AdPaginationInfo
	{
		/// <summary>
		/// Żądany numer strony
		/// </summary>
		public int RequestedPage { get; set; }

		/// <summary>
		/// Liczba elementów na jednej stronie
		/// </summary>
		public int ItemsPerPage { get; set; }

		/// <summary>
		/// Czy sortowanie rosnące
		/// </summary>
		public bool Accending { get; set; }

		/// <summary>
		/// Nazwa sortowanego pola
		/// </summary>
		public string SortExpression { get; set; }

		/// <summary>
		/// Parametr out-owy określający liczbę znalezionych elementów
		/// </summary>
		public int OutResultsFound { get; set; }

		#region Overrided methods

		public override bool Equals(object obj)
		{
			AdPaginationInfo other = obj as AdPaginationInfo;
			if (other != null)
			{
				return other.Accending == this.Accending &&
					other.ItemsPerPage == this.ItemsPerPage &&
					other.OutResultsFound == this.OutResultsFound &&
					other.RequestedPage == this.RequestedPage &&
					other.SortExpression == this.SortExpression;
			}
			return false;
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		#endregion Overrided methods
	}
}