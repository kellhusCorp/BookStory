namespace BookStory.Domain.Details
{
    public struct BookDetails
    {
        public string Name { get; }
    
        public int PublishYear { get; }
    
        public int GenreId { get; }
    
        public string EditorialOfficeName { get; }

        public BookDetails(string name, int publishYear, int genreId, string editorialOfficeName)
        {
            Name = name;
            PublishYear = publishYear;
            GenreId = genreId;
            EditorialOfficeName = editorialOfficeName;
        }
    }
}
