namespace BookStory.Domain
{
    public class AuthorShip
    {
        public AuthorShip(int bookId, int authorId)
        {
            BookId = bookId;
            AuthorId = authorId;
        }

        public int BookId { get; set; }

        public Book Book { get; set; }

        public int AuthorId { get; set; }

        public Author Author { get; set; }
    }
}
